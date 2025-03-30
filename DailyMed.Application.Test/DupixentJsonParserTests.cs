using DailyMed.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Application.Test
{
    public class DupixentJsonParserTests
    {
        [Fact]
        public void Parse_ValidJson_ReturnsCopayProgramWithExpectedFields()
        {
            //// arrange
            //var rawJson = @"{
            //    ""ProgramName"": ""Dupixent MyWay Copay Card"",
            //    ""CoverageEligibilities"": [""Commercially insured""],
            //    ""AnnualMax"": ""$13,000"",
            //    ""EnrollmentURL"": ""https://www.dupixent.com/support-savings/copay-card"",
            //    ""AssistanceType"": ""Coupon""
            //}";

            //// act
            //var result = DupixentJsonParser.Parse(rawJson);

            //// assert
            //Assert.Equal("Dupixent MyWay Copay Card", result.ProgramName);
            //Assert.Single(result.CoverageEligibilities);
            //Assert.Equal("Coupon", result.ProgramType);
            //Assert.Contains(result.Benefits, b => b.Name == "max_annual_savings" && b.Value == "13,000");
        }
    }
}
