using Blazor.Database.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.Database.Components
{
    public partial class WeatherForecastList : ComponentBase, IDisposable
    {
        [Inject] private NavigationManager NavManager { get; set; }

        [Parameter] public EventCallback<int> EditRecord { get; set; }

        [Parameter] public EventCallback<int> ViewRecord { get; set; }

        [Parameter] public EventCallback<int> NewRecord { get; set; }

        //[Inject] WeatherControllerService ControllerService { get; set; }

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

        private void Edit(int id)
            => this.EditRecord.InvokeAsync(id);

        private void View(int id)
            => this.ViewRecord.InvokeAsync(id);


        private void New()
            => this.NewRecord.InvokeAsync();

        private async void EditInModal(int id)
        {
            var options = new ModalOptions();
            options.Set("Id", id);
            await this.Modal.ShowAsync<WeatherForecastEditor>(options);
        }
        private async void ViewInModal(int id)
        {
            var options = new ModalOptions();
            options.Set("Id", id);
            await this.Modal.ShowAsync<WeatherForecastViewer>(options);
        }

        private async void NewInModal()
        {
            var options = new ModalOptions();
            options.Set("Id", -1);
            await this.Modal.ShowAsync<WeatherForecastEditor>(options);
        }

        public void Dispose()
        {
            this.ControllerService.ListHasChanged -= OnListChanged;
        }
    }
}