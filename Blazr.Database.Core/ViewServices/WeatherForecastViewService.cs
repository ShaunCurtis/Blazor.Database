/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.SPA.Core;

namespace Blazr.Database.Core
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
