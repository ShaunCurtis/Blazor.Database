/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.Database.Core;
using Blazr.SPA.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazr.Database.Forms
{
    public partial class WeatherForecastEditorForm : EditRecordFormBase<WeatherForecast, EditWeatherForecast>
    {

        [Inject] private WeatherForecastViewService ViewService { get; set; }

        public override Guid FormId { get; } = new Guid("68eb8db6-65f4-40b4-b88a-be54d95ee855");

        protected async override Task OnInitializedAsync()
        {
            //this.FormUrl = "/Weather/Edit";
            this.Service = ViewService;
            await LoadRecordAsync();
        }
    }
}
