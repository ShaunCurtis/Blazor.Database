using Blazor.Database.Data;
using Blazor.Database.Services;
using Blazor.SPA.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.Database.Components
{
    public partial class WeatherForecastListForm : ListFormBase<WeatherForecast>
    {
        [Inject] private WeatherForecastControllerService ControllerService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.Service = this.ControllerService;
            await base.OnInitializedAsync();
        }
    }
}