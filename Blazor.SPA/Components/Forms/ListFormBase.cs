using Blazor.SPA.Components;
using Blazor.SPA.Data;
using Blazor.SPA.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.SPA.Components
{
    public class WeatherForecastListForm<TRecord> : ComponentBase, IDisposable
        where TRecord : class, IDbRecord<TRecord>, new()
    {
        [Inject] private NavigationManager NavManager { get; set; }

        [Parameter] public EventCallback<int> EditRecord { get; set; }

        [Parameter] public EventCallback<int> ViewRecord { get; set; }

        [Parameter] public EventCallback<int> NewRecord { get; set; }

        [Parameter] public EventCallback ExitAction { get; set; }

        [Inject] private IFactoryControllerService<TRecord> Service { get; set; }

        private bool _isLoaded => this.Service?.HasRecords ?? false;

        private bool _hasService => this.Service != null;

        protected override async Task OnInitializedAsync()
        {
            if (_hasService)
            {
                //TODO - got to here
                await this.Service.GetRecordsAsync();
                this.Service.ListHasChanged += OnListChanged;
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
            => this.Service.ListHasChanged -= OnListChanged;
    }
}