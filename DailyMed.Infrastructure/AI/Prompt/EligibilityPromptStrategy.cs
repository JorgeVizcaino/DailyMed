using DailyMed.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Infrastructure.AI.Prompt
{
    public class EligibilityPromptStrategy : IPromptStrategy
    {
        public string BuildPrompt(string input)
        {
            // Possibly your instructions for returning a JSON structure
            return $@"
            You are a helpful AI. 
            Convert the following text into valid JSON key-value pairs
            describing eligibility requirements. 
            The output must be valid JSON (an array of objects).

            TEXT:
            {input}

            Example output:
            [
               {{ ""name"": ""insurance_coverage"", ""value"": ""commercial_only"" }},
               {{ ""name"": ""us_residency"", ""value"": ""true"" }}
            ]
        ";
        }
    }
}
