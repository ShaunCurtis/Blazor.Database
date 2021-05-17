/// ======================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: MIT
/// ======================================

using Blazor.Database.Data;
using Blazor.Database.Services;
using Blazor.SPA.Components;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
namespace Blazor.Database.Components
{
    public partial class WeatherForecastEditorForm : EditRecordFormBase<WeatherForecast>
    {

        [Inject] private WeatherForecastViewService ViewService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            this.Service = ViewService;
            await LoadRecordAsync();
        }

    }
}
