// ==========================================================
//  Original code:
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// ============================================================

/// =================================
/// Mods Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


#nullable disable warnings

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Blazr.SPA.Components
{
    /// <summary>
    /// Displays the specified page component, rendering it inside its layout
    /// and any further nested layouts.
    /// Customized replacement version of RouteView Component
    /// Handles Dynamic Layout changes and changing RouteViews with no routing
    /// </summary>
    public class RouteViewManager : IComponent
    {
        private ViewData _ViewData { get; set; }
        private bool _RenderEventQueued;
        private RenderHandle _renderHandle;

        [Inject] private EditStateService EditStateService { get; set; }
        [Inject] private IJSRuntime _js { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }
        [Inject] private RouteViewService RouteViewService { get; set; }

        [Parameter] public RouteData RouteData { get; set; }

        [Parameter] public Type DefaultLayout { get; set; }

        [Parameter] public int ViewHistorySize { get; set; } = 10;

        public ViewData ViewData
        {
            get => this._ViewData;
            protected set
            {
                this.AddViewToHistory(this._ViewData);
                this._ViewData = value;
            }
        }

        public SortedList<DateTime, ViewData> ViewHistory { get; private set; } = new SortedList<DateTime, ViewData>();

        public ViewData LastViewData
        {
            get
            {
                var newest = ViewHistory.Max(item => item.Key);
                if (newest != default) return ViewHistory[newest];
                else return null;
            }
        }

        public bool IsCurrentView(Type view) => this.ViewData?.ViewType == view;

        public bool HasView => this._ViewData?.ViewType != null;

        public void Attach(RenderHandle renderHandle)
            => _renderHandle = renderHandle;

        public async Task SetParametersAsync(ParameterView parameters)
        {
            // Sets the component parameters
            parameters.SetParameterProperties(this);

            // Check if we have either RouteData or ViewData
            if (RouteData == null)
            {
                throw new InvalidOperationException($"The {nameof(RouteView)} component requires a non-null value for the parameter {nameof(RouteData)}.");
            }
            // we've routed and need to clear the ViewData
            this._ViewData = null;
            // Render the component
            await this.RenderAsync();
        }

        public async Task LoadViewAsync(ViewData viewData = null)
        {
            if (viewData != null) this.ViewData = viewData;
            if (ViewData == null)
            {
                throw new InvalidOperationException($"The {nameof(RouteViewManager)} component requires a non-null value for the parameter {nameof(ViewData)}.");
            }
            await this.RenderAsync();
        }

        public async Task LoadViewAsync(Type viewtype)
            => await this.LoadViewAsync(new ViewData(viewtype, new Dictionary<string, object>()));

        public async Task LoadViewAsync<TView>(Dictionary<string, object> data = null)
            => await this.LoadViewAsync(new ViewData(typeof(TView), data));

        private RenderFragment _renderDelegate => builder =>
        {
            _RenderEventQueued = false;
            // Adds cascadingvalue for the ViewManager
            builder.OpenComponent<CascadingValue<RouteViewManager>>(0);
            builder.AddAttribute(1, "Value", this);
            // Get the layout render fragment
            builder.AddAttribute(2, "ChildContent", this._layoutViewFragment);
            builder.CloseComponent();
        };

        private RenderFragment _layoutViewFragment => builder =>
        {
            Type _pageLayoutType = RouteData?.PageType.GetCustomAttribute<LayoutAttribute>()?.LayoutType
                ?? RouteViewService.Layout
                ?? DefaultLayout;

            builder.OpenComponent<LayoutView>(0);
            builder.AddAttribute(1, nameof(LayoutView.Layout), _pageLayoutType);
            if (this.EditStateService.IsDirty && this.EditStateService.DoFormReload is not true)
                builder.AddAttribute(2, nameof(LayoutView.ChildContent), _dirtyExitFragment);
            else
            {
                this.EditStateService.DoFormReload = false;
                builder.AddAttribute(3, nameof(LayoutView.ChildContent), _renderComponentWithParameters);
            }
            builder.CloseComponent();
        };

        private RenderFragment _dirtyExitFragment => builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "dirty-exit");
            {
                builder.OpenElement(2, "div");
                builder.AddAttribute(3, "class", "dirty-exit-message");
                builder.AddContent(4, "You are existing a form with unsaved data");
                builder.CloseElement();
            }
            {
                builder.OpenElement(2, "div");
                builder.AddAttribute(3, "class", "dirty-exit-message");
                {
                    builder.OpenElement(2, "button");
                    builder.AddAttribute(3, "class", "dirty-exit-button");
                    builder.AddAttribute(5, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, this.DirtyExit));
                    builder.AddContent(6, "Exit and Clear Unsaved Data");
                    builder.CloseElement();
                }
                {
                    builder.OpenElement(2, "button");
                    builder.AddAttribute(3, "class", "load-dirty-form-button");
                    builder.AddAttribute(5, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, this.LoadDirtyForm));
                    builder.AddContent(6, "Reload Form");
                    builder.CloseElement();
                }
                builder.CloseElement();
            }
            builder.CloseElement();
        };

        private RenderFragment _renderComponentWithParameters => builder =>
        {
            Type componentType = null;
            IReadOnlyDictionary<string, object> parameters = new Dictionary<string, object>();

            if (_ViewData != null)
            {
                componentType = _ViewData.ViewType;
                parameters = _ViewData.ViewParameters;
            }
            else if (RouteData != null)
            {
                componentType = RouteData.PageType;
                parameters = RouteData.RouteValues;
            }

            if (componentType != null)
            {
                builder.OpenComponent(0, componentType);
                foreach (var kvp in parameters)
                {
                    builder.AddAttribute(1, kvp.Key, kvp.Value);
                }
                builder.CloseComponent();
            }
            else
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, "No Route or View Configured to Display");
                builder.CloseElement();
            }
        };

        public async Task RenderAsync() => await InvokeAsync(() =>
        {
            if (!this._RenderEventQueued)
            {
                this._RenderEventQueued = true;
                _renderHandle.Render(_renderDelegate);
            }
        }
        );

        protected Task InvokeAsync(Action workItem) 
            => _renderHandle.Dispatcher.InvokeAsync(workItem);

        protected Task InvokeAsync(Func<Task> workItem) 
            => _renderHandle.Dispatcher.InvokeAsync(workItem);

        private void AddViewToHistory(ViewData value)
        {
            while (this.ViewHistory.Count >= this.ViewHistorySize)
            {
                var oldest = ViewHistory.Min(item => item.Key);
                this.ViewHistory.Remove(oldest);
            }
            this.ViewHistory.Add(DateTime.Now, value);
        }

        private Task DirtyExit(MouseEventArgs d)
        {
            this.EditStateService.ClearEditState();
            this.SetPageExitCheck(false);
            return RenderAsync();
        }

        private void LoadDirtyForm(MouseEventArgs e)
        {
            this.EditStateService.DoFormReload = true;
            NavManager.NavigateTo(this.EditStateService.EditFormUrl);
        }

        private void SetPageExitCheck(bool action)
            => _js.InvokeAsync<bool>("cecblazor_setEditorExitCheck", action);
    }
}
