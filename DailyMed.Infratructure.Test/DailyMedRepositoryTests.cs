using DailyMed.Core.Interfaces;
using DailyMed.Core.Models.DailyMed;
using DailyMed.Core.Models.FDA;
using DailyMed.Infrastructure.Data;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DailyMed.Infratructure.Test
{
    public class DailyMedRepositoryTests
    {
        [Fact]
        public async Task SearchIndicationsAsync_ShouldReturnIndications_IfDataFound()
        {
            // Arrange
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockParser = new Mock<IIndicationsParser>();
            var mockIDrugIndicationsRepository = new Mock<IDrugIndicationsRepository>();

            // The parser now returns a single string rather than a list
            mockParser
                .Setup(p => p.ExtractIndications(It.IsAny<string>()))
                .Returns("Asthma - Eczema");

            // 1) Mock the "DailyMed" client returning the search result with a valid setid
            var dailyMedSearchJson = new DailyMedSearchResult
            {
                Data = new List<SplDailyMedData> {
                    new SplDailyMedData { SetId = "fake-setid-123" }
                }
            };
            var dailyMedClient = CreateMockHttpClient(JsonSerializer.Serialize(dailyMedSearchJson), HttpStatusCode.OK);

            // 2) Mock the "fdaApi" client returning a label with "indications_and_usage"
            var fdaRoot = new RootFDABody
            {
                results = new List<Result>
                {
                    new Result {
                        indications_and_usage = new List<string> { "DUPIXENT is indicated for: - Asthma - Eczema" }
                    }
                }
            };
            var fdaClient = CreateMockHttpClient(JsonSerializer.Serialize(fdaRoot), HttpStatusCode.OK);

            // Setup the factory to return these mock clients
            mockHttpClientFactory
                .Setup(x => x.CreateClient("DailyMed"))
                .Returns(dailyMedClient);

            mockHttpClientFactory
                .Setup(x => x.CreateClient("fdaApi"))
                .Returns(fdaClient);

            // Instantiate the repository with mocks
            var repo = new DailyMedRepository(mockHttpClientFactory.Object, mockParser.Object, mockIDrugIndicationsRepository.Object);

            // Act
            var result = await repo.SearchIndicationsAsync("dupixent");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("fake-setid-123", result.SetId);

            // Because our parser mock returns "Asthma - Eczema"
            Assert.Equal("Asthma - Eczema", result.Indications);
        }

        [Fact]
        public async Task SearchIndicationsAsync_ShouldThrow_IfNoSetId()
        {
            // Arrange
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockParser = new Mock<IIndicationsParser>();
            var mockIDrugIndicationsRepository = new Mock<IDrugIndicationsRepository>();

            // Return empty data from dailyMed
            var dailyMedSearchJson = new DailyMedSearchResult
            {
                Data = new List<SplDailyMedData>() // empty => no results
            };
            var dailyMedClient = CreateMockHttpClient(JsonSerializer.Serialize(dailyMedSearchJson), HttpStatusCode.OK);

            mockHttpClientFactory
                .Setup(x => x.CreateClient("DailyMed"))
                .Returns(dailyMedClient);

            // The "fdaApi" client won't matter because we won't get that far
            var fdaClient = CreateMockHttpClient("{}", HttpStatusCode.OK);
            mockHttpClientFactory
                .Setup(x => x.CreateClient("fdaApi"))
                .Returns(fdaClient);

            var repo = new DailyMedRepository(mockHttpClientFactory.Object, mockParser.Object, mockIDrugIndicationsRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => repo.SearchIndicationsAsync("dupixent")
            );
        }

        private HttpClient CreateMockHttpClient(string json, HttpStatusCode statusCode)
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(json)
                });

            // The BaseAddress is arbitrary; it's never actually called in a real network sense
            return new HttpClient(handler.Object)
            {
                BaseAddress = new Uri("https://test/")
            };
        }
    }
}
