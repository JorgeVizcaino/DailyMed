using DailyMed.Core.Interfaces;
using DailyMed.Infrastructure.FDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Infratructure.Test
{
    public class IndicationsParserTests
    {
        private readonly IIndicationsParser _parser;

        public IndicationsParserTests()
        {
            _parser = new IndicationsParser();
        }

        [Fact]
        public void ExtractIndications_ShouldReturnEmpty_IfTextIsNullOrEmpty()
        {
            var result = _parser.ExtractIndications("");
            Assert.Empty(result);
        }

        [Fact]
        public void ExtractIndications_ShouldParse_WhenPatternMatches()
        {
            var text = "DUPIXENT is indicated for: - Asthma - Atopic Dermatitis - CRSwNP";
            var result = _parser.ExtractIndications(text);

            Assert.NotEmpty(result);
            Assert.Contains("Asthma", result);
            Assert.Contains("Atopic Dermatitis", result);
            Assert.Contains("CRSwNP", result);
        }

        [Fact]
        public void ExtractIndications_ShouldFallback_WhenNoRegexMatch()
        {
            var text = "Use: This medication is for these conditions: Possibly Eczema...";
            var result = _parser.ExtractIndications(text);

            // Because it didn't match the pattern, we returned raw text
            Assert.Equal(text, result);
        }
    }
}
