using Blazr.SPA.Brokers;
using Blazr.SPA.Connectors;

namespace Blazr.Database.Core.Connectors
{
    public class WeatherDataServiceConnector : ModelDataServiceConnector
    {
        public WeatherDataServiceConnector(IDataBroker dataBroker, ILoggingBroker loggingBroker) : base(dataBroker, loggingBroker) { }
    }
}
