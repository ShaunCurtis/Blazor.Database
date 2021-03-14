using Blazor.Database.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.Database.Components
{
    public partial class WeatherForecastList : ComponentBase
    {
        [Inject] private NavigationManager NavManager { get; set; }

        [Parameter] public EventCallback<int> EditRecord { get; set; }

        [Parameter] public EventCallback<int> ViewRecord { get; set; }

        [Parameter] public EventCallback<int> NewRecord { get; set; }

        //[Inject] WeatherControllerService ControllerService { get; set; }

        [Inject] WeatherForecastControllerService ControllerService { get; set; }

        private bool _isLoaded => this.ControllerService.?.HasRecords ?? false;

        private BaseModalDialog Modal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.ControllerService.GetRecordsAsync();
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
    }
}