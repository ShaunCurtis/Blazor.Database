using Blazor.Database.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.Database.Components
{
    public partial class WeatherForecastViewer : ComponentBase
    {
        [Parameter] public int ID { get; set; } = -1;

        private bool HasServices => this.ControllerService != null;

        [Inject] private WeatherControllerService ControllerService { get; set; }

        private bool IsLoaded => this.ControllerService != null && this.ControllerService.Record != null;
        
        [CascadingParameter] private IModalDialog Modal { get; set; }

        protected async override Task OnInitializedAsync()
        {
                await this.ControllerService.GetRecordAsync(ID);
        }

        private void Exit()
        {
        }

    }
}
