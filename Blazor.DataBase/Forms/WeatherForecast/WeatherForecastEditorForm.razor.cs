/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazor.Database.Data;
using Blazor.Database.Services;
using Blazor.SPA.Forms;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.Database.Forms
{
    public partial class WeatherForecastEditorForm : EditRecordFormBase<WeatherForecast, EditWeatherForecast>
    {

        [Inject] private WeatherForecastViewService ViewService { get; set; }

        public override Guid FormId { get; } = new Guid("68eb8db6-65f4-40b4-b88a-be54d95ee855"); 

        protected async override Task OnInitializedAsync()
        {
            this.Service = ViewService;
            await LoadRecordAsync();
        }
    }
}
