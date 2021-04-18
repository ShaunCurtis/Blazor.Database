// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// Mods by Cold Elm Coders

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Blazor.Database.Components.Base
{
    public class TestComponentBase : IComponent, IHandleEvent, IHandleAfterRender
    {
        private readonly RenderFragment _renderFragment;
        private RenderHandle _renderHandle;
        private bool _initialized;
        private bool _hasNeverRendered = true;
        private bool _hasPendingQueuedRender;
        private bool _hasCalledOnAfterRender;

        public int SetParamsAsync { get; private set; } =  0;
        public int InitRun { get; private set; } = 0;
        public int ParamsSet { get; private set; } = 0;
        public int Rendered { get; private set; } = 1;

        public TestComponentBase()
        {
            _renderFragment = builder =>
            {
                _hasPendingQueuedRender = false;
                _hasNeverRendered = false;
                BuildRenderTree(builder);
            };
        }

        protected virtual void BuildRenderTree(RenderTreeBuilder builder)
        {
        }

        protected virtual void OnInitialized()
        {
        }

        protected virtual Task OnInitializedAsync()
        {
            this.InitRun++;
            return Task.CompletedTask;
        }

        protected virtual void OnParametersSet()
        {
        }

        protected virtual Task OnParametersSetAsync()
        {
            this.ParamsSet++;
            return Task.CompletedTask;
        }

        protected void StateHasChanged()
        {
            if (_hasPendingQueuedRender)
                return;

            if (_hasNeverRendered || ShouldRender())
            {
                this.Rendered++;
                _hasPendingQueuedRender = true;

                try
                {
                    _renderHandle.Render(_renderFragment);
                }
                catch
                {
                    _hasPendingQueuedRender = false;
                    throw;
                }
            }
        }

        protected virtual bool ShouldRender()
            => true;

        protected virtual void OnAfterRender(bool firstRender)
        {
        }

        protected virtual Task OnAfterRenderAsync(bool firstRender)
            => Task.CompletedTask;

        protected Task InvokeAsync(Action workItem)
            => _renderHandle.Dispatcher.InvokeAsync(workItem);

        protected Task InvokeAsync(Func<Task> workItem)
            => _renderHandle.Dispatcher.InvokeAsync(workItem);

        void IComponent.Attach(RenderHandle renderHandle)
        {
            if (_renderHandle.IsInitialized)
            {
                throw new InvalidOperationException($"The render handle is already set. Cannot initialize a {nameof(ComponentBase)} more than once.");
            }

            _renderHandle = renderHandle;
        }

        public virtual Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            this.SetParamsAsync++;
            if (!_initialized)
            {
                _initialized = true;

                return RunInitAndSetParametersAsync();
            }
            else
                return CallOnParametersSetAsync();
        }

        private async Task RunInitAndSetParametersAsync()
        {
            OnInitialized();
            var task = OnInitializedAsync();

            if (task.Status != TaskStatus.RanToCompletion && task.Status != TaskStatus.Canceled)
            {
                StateHasChanged();

                try
                {
                    await task;
                }
                catch // avoiding exception filters for AOT runtime support
                {
                    if (!task.IsCanceled)
                        throw;
                }

                // Don't call StateHasChanged here. CallOnParametersSetAsync should handle that for us.
            }

            await CallOnParametersSetAsync();
        }

        private Task CallOnParametersSetAsync()
        {
            OnParametersSet();
            var task = OnParametersSetAsync();
            var shouldAwaitTask = task.Status != TaskStatus.RanToCompletion &&
                task.Status != TaskStatus.Canceled;

            StateHasChanged();

            return shouldAwaitTask ?
                CallStateHasChangedOnAsyncCompletion(task) :
                Task.CompletedTask;
        }

        private async Task CallStateHasChangedOnAsyncCompletion(Task task)
        {
            try
            {
                await task;
            }
            catch // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancellations, but don't bother issuing a state change.
                if (task.IsCanceled)
                    return;

                throw;
            }
            StateHasChanged();
        }

        Task IHandleEvent.HandleEventAsync(EventCallbackWorkItem callback, object? arg)
        {
            var task = callback.InvokeAsync(arg);
            var shouldAwaitTask = task.Status != TaskStatus.RanToCompletion &&
                task.Status != TaskStatus.Canceled;
            StateHasChanged();
            return shouldAwaitTask ?
                CallStateHasChangedOnAsyncCompletion(task) :
                Task.CompletedTask;
        }

        Task IHandleAfterRender.OnAfterRenderAsync()
        {
            var firstRender = !_hasCalledOnAfterRender;
            _hasCalledOnAfterRender |= true;

            OnAfterRender(firstRender);

            return OnAfterRenderAsync(firstRender);
        }
    }
}
