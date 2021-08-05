/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Database.Core;
using Blazr.SPA.Components;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Blazr.Database.Forms
{
    public partial class WeatherForecastViewerForm : RecordFormBase<WeatherForecast>
    {

        [Inject] private WeatherForecastViewService ViewService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            this.Service = this.ViewService;
            await base.OnInitializedAsync();
        }
    }
}
