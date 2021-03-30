﻿using Blazor.Database.Services;
using Blazor.SPA.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.Database.Components
{
    public partial class WeatherForecastListModalForm : ComponentBase, IDisposable
    {
        [Inject] private NavigationManager NavManager { get; set; }

        [Parameter] public EventCallback<int> EditRecord { get; set; }

        [Parameter] public EventCallback<int> ViewRecord { get; set; }

        [Parameter] public EventCallback<int> NewRecord { get; set; }

        [Inject] private WeatherForecastControllerService ControllerService { get; set; }

        private bool _isLoaded => this.ControllerService?.HasRecords ?? false;

        private bool _hasService => this.ControllerService != null;

        private BaseModalDialog Modal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (_hasService)
            {
                await this.ControllerService.GetRecordsAsync();
                this.ControllerService.ListHasChanged += OnListChanged;
            }
        }

        private void OnListChanged(object sender, EventArgs e)
        {
            this.InvokeAsync(this.StateHasChanged);
        }

        private async void EditInModal(int id)
        {
            var options = new ModalOptions();
            options.Set("Id", id);
            await this.Modal.ShowAsync<WeatherForecastEditorForm>(options);
        }
        private async void ViewInModal(int id)
        {
            var options = new ModalOptions();
            options.Set("Id", id);
            await this.Modal.ShowAsync<WeatherForecastViewerForm>(options);
        }

        private async void NewInModal()
        {
            var options = new ModalOptions();
            options.Set("Id", -1);
            await this.Modal.ShowAsync<WeatherForecastEditorForm>(options);
        }

        public void Dispose()
        {
            this.ControllerService.ListHasChanged -= OnListChanged;
        }
    }
}