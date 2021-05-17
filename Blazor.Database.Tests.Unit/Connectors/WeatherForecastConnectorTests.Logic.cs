using Blazor.Database.Data;
using Blazor.SPA.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Blazor.Database.Tests.Unit.Connectors
{
    public partial class WeatherForecastConnectorTests
    {
        [Fact]
        public async Task ShouldGetWeatherForecastAsync()
        {
            // define
            WeatherForecast randomWeatherForecast = CreateRandomWeatherForecast();

            WeatherForecast inputWeatherForecast = randomWeatherForecast;
            WeatherForecast retrievedWeatherForecast = inputWeatherForecast;
            WeatherForecast expectedWeatherForecast = retrievedWeatherForecast;

            int inputid = randomWeatherForecast.ID;
            int expectedid = inputid;

            this.dataBrokerMock.Setup(broker =>
           broker.SelectRecordAsync<WeatherForecast>(inputid))
                .ReturnsAsync(inputWeatherForecast);

            // test

            retrievedWeatherForecast = await this.dataServiceConnector.GetRecordByIdAsync<WeatherForecast>(inputid);

            // assert

            retrievedWeatherForecast.Should().BeEquivalentTo(expectedWeatherForecast);
            this.dataBrokerMock.Verify(broker => broker.SelectRecordAsync<WeatherForecast>(inputid), Times.Once);
            this.dataBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddWeatherForecastAsync()
        {
            // define
            WeatherForecast randomWeatherForecast = CreateRandomWeatherForecast();
            WeatherForecast inputWeatherForecast = randomWeatherForecast;

            int id = randomWeatherForecast.ID;

            var successDbTaskResult = CreateSuccessDbTaskResult(id);
            var inputDbTaskResult = successDbTaskResult;
            var returnDbTaskResult = inputDbTaskResult;
            var expectedDbTaskResult = returnDbTaskResult;

            this.dataBrokerMock.Setup(broker =>
           broker.InsertRecordAsync<WeatherForecast>(inputWeatherForecast))
                .ReturnsAsync(inputDbTaskResult);

            // test

            returnDbTaskResult = await this.dataServiceConnector.AddRecordAsync<WeatherForecast>(inputWeatherForecast);

            // assert

            expectedDbTaskResult.Should().BeEquivalentTo(returnDbTaskResult);
            this.dataBrokerMock.Verify(broker => broker.InsertRecordAsync<WeatherForecast>(inputWeatherForecast), Times.Once);
            this.dataBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldModifyWeatherForecastAsync()
        {
            // define
            WeatherForecast randomWeatherForecast = CreateRandomWeatherForecast();
            WeatherForecast inputWeatherForecast = randomWeatherForecast;

            int id = randomWeatherForecast.ID;

            var successDbTaskResult = CreateSuccessDbTaskResult(id);
            var inputDbTaskResult = successDbTaskResult;
            var returnDbTaskResult = inputDbTaskResult;
            var expectedDbTaskResult = returnDbTaskResult;

            this.dataBrokerMock.Setup(broker =>
           broker.UpdateRecordAsync<WeatherForecast>(inputWeatherForecast))
                .ReturnsAsync(inputDbTaskResult);

            // test

            returnDbTaskResult = await this.dataServiceConnector.ModifyRecordAsync<WeatherForecast>(inputWeatherForecast);

            // assert

            expectedDbTaskResult.Should().BeEquivalentTo(returnDbTaskResult);
            this.dataBrokerMock.Verify(broker => broker.InsertRecordAsync<WeatherForecast>(inputWeatherForecast), Times.Once);
            this.dataBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldDeleteWeatherForecastAsync()
        {
            // define
            WeatherForecast randomWeatherForecast = CreateRandomWeatherForecast();
            WeatherForecast inputWeatherForecast = randomWeatherForecast;

            int id = randomWeatherForecast.ID;

            var successDbTaskResult = CreateSuccessDbTaskResult(id);
            var inputDbTaskResult = successDbTaskResult;
            var returnDbTaskResult = inputDbTaskResult;
            var expectedDbTaskResult = returnDbTaskResult;

            this.dataBrokerMock.Setup(broker =>
           broker.DeleteRecordAsync<WeatherForecast>(inputWeatherForecast))
                .ReturnsAsync(inputDbTaskResult);

            // test

            returnDbTaskResult = await this.dataServiceConnector.RemoveRecordAsync<WeatherForecast>(inputWeatherForecast);

            // assert

            expectedDbTaskResult.Should().BeEquivalentTo(returnDbTaskResult);
            this.dataBrokerMock.Verify(broker => broker.DeleteRecordAsync<WeatherForecast>(inputWeatherForecast), Times.Once);
            this.dataBrokerMock.VerifyNoOtherCalls();
        }
    }
}
