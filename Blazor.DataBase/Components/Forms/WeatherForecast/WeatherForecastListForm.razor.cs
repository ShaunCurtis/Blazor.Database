using Blazor.Database.Services;
using Blazor.SPA.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.Database.Components
{
    public partial class WeatherForecastListForm : ComponentBase, IDisposable
    {
        [Inject] private NavigationManager NavManager { get; set; }

        [Parameter] public EventCallback<int> EditRecord { get; set; }

        [Parameter] public EventCallback<int> ViewRecord { get; set; }

        [Parameter] public EventCallback<int> NewRecord { get; set; }

        [Parameter] public EventCallback ExitAction { get; set; }

        [Inject] private WeatherForecastControllerService ControllerService { get; set; }

        private bool _isLoaded => this.ControllerService?.HasRecords ?? false;

        private bool _hasService => this.ControllerService != null;

        protected override async Task OnInitializedAsync()
        {
            if (_hasService)
            {
                await this.ControllerService.GetRecordsAsync();
                this.ControllerService.ListHasChanged += OnListChanged;
            }
        }

        private void OnListChanged(object sender, EventArgs e)
            => this.InvokeAsync(this.StateHasChanged);

        private void Edit(int id)
            => this.EditRecord.InvokeAsync(id);

        private void View(int id)
            => this.ViewRecord.InvokeAsync(id);

        private void New()
            => this.NewRecord.InvokeAsync();

        protected void Exit()
        {
            if (ExitAction.HasDelegate)
                ExitAction.InvokeAsync();
            else
                this.NavManager.NavigateTo("/");
        }

        public void Dispose()
            => this.ControllerService.ListHasChanged -= OnListChanged;
    }
}