﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazor.Database.Data;
using Blazor.SPA.Connectors;
using Blazor.SPA.Services;

namespace Blazor.Database.Services
{
    public class WeatherForecastViewService : 
        BaseModelViewService<WeatherForecast>, 
        IModelViewService<WeatherForecast>
    {
        // Used by the demo data form to show stepping
        public int FormStep { get; set; } = 1;

        public WeatherForecastViewService(IDataServiceConnector dataServiceConnector) : base(dataServiceConnector) { }
    }
}
