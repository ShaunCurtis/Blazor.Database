using Blazor.Database.Data;
using Blazor.Database.Services;
using Blazor.SPA.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.Database.Components
{
    public partial class WeatherForecastViewerForm : RecordFormBase<WeatherForecast>
    {

        [Inject] private WeatherForecastControllerService ControllerService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            this.Service = this.ControllerService;
            await base.OnInitializedAsync();
        }
    }
}
