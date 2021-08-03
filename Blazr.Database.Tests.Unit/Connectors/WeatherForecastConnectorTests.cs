using Blazor.Database.Data;
using Blazor.SPA.Brokers;
using Blazor.SPA.Connectors;
using Blazor.SPA.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tynamix.ObjectFiller;

namespace Blazor.Database.Tests.Unit.Connectors
{
    public partial class WeatherForecastConnectorTests
    {
        private readonly Mock<IDataBroker> dataBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IDataServiceConnector dataServiceConnector;

        public WeatherForecastConnectorTests()
        {
            this.dataBrokerMock = new Mock<IDataBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dataServiceConnector = new ModelDataServiceConnector(dataBroker: dataBrokerMock.Object, loggingBroker: loggingBrokerMock.Object);
        }

        private static List<WeatherForecast> CreateRandomWeatherForecastList(int numberOfRecords = 50)
        {
            var list = new List<WeatherForecast>();
            for (var i = 0; i < numberOfRecords; i++)
            {
                list.Add(CreateRandomWeatherForecast());
            }
            return list;
        }

        private static WeatherForecast CreateRandomWeatherForecast()
            => CreateWeatherForecastFiller().Create();

        private static Filler<WeatherForecast> CreateWeatherForecastFiller()
        {
            var filler = new Filler<WeatherForecast>();
            filler.Setup().OnType<DateTimeOffset>().Use(DateTimeOffset.UtcNow);
            return filler;
        }
        private static DbTaskResult CreateSuccessDbTaskResult(int id)
            => DbTaskResult.OK(id);
    }
}
