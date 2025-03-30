using DailyMed.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DailyMed.Infrastructure.FDA
{
    public class IndicationsParser : IIndicationsParser
    {
        public string ExtractIndications(string rawText)
        {
            if (string.IsNullOrWhiteSpace(rawText))
                return "";

            // search for "indicated for".
            var pattern = @"is\s+indicated\s+for\s*\s*(?<rest>.+)";
            var match = Regex.Match(rawText, pattern, RegexOptions.IgnoreCase);

            if (!match.Success)
            {                
                return rawText;
            }

            var rest = match.Groups["rest"].Value;          

            return rest;
        }
    }
}
