using DailyMed.Core.Interfaces;
using DailyMed.Core.Models.CopayCard;
using DailyMed.Infrastructure.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Infratructure.Test
{
    public class CopayParserJsonTests
    {
        [Fact]
        public async Task DoConvertAsync_ReturnsStandardCopayCardDto()
        {
            // Arrange
            var mockAIService = new Mock<IGenerativeAIService>();
            var mockFileReader = new Mock<IFileReader>();
            var mockIPromptStrategy = new Mock<IPromptStrategy>();

            var mockCopayCard = new CopayCard
            {
                ProgramName = "Test Program",
                CoverageEligibilities = new List<string> { "Private Insurance" },
                AssistanceType = "Card",
                EligibilityDetails = "Some details",
                AnnualMax = "$2000",
                EnrollmentURL = "http://example.com",
                OfferRenewable = true,
                ProgramDetails = "Some program details",
                AddRenewalDetails = "Renew every year",
                IncomeReq = true
            };

            var mockJson = System.Text.Json.JsonSerializer.Serialize(mockCopayCard);
            mockFileReader.Setup(r => r.ReadAllTextAsync(It.IsAny<string>())).ReturnsAsync(mockJson);
            mockAIService.Setup(a =>  a.InferRequirementsFromEligibilityAsync(It.IsAny<string>(), mockIPromptStrategy.Object))
                     .ReturnsAsync(new List<RequirementDto> 
                                        { new RequirementDto { Name = "Must have private insurance", Value = "" } });


            var parser = new CopayParserJson(mockAIService.Object, mockFileReader.Object);

            // Act
            var result = await parser.DoConvertAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Program", result.ProgramName);
            Assert.Equal("2000", result.Benefits[0].Value);
            Assert.Equal("true", result.Funding.Evergreen);
        }
    }
}
