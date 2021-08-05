/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.SPA.Data;
using Blazr.SPA.Core;

namespace Blazr.Database.Core.Connectors
{
    public class WeatherDataServiceConnector : ModelDataServiceConnector
    {
        public WeatherDataServiceConnector(IDataBroker dataBroker, ILoggingBroker loggingBroker) : base(dataBroker, loggingBroker) { }
    }
}
