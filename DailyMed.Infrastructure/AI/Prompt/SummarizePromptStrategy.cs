using DailyMed.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Infrastructure.AI.Prompt
{
    public class SummarizePromptStrategy : IPromptStrategy
    {
        public string BuildPrompt(string input)
        {
            // Maybe your instructions for summarizing:
            return $@"
            You are a helpful AI. 
            Please summarize the following text and save the resposne in array of String:

            TEXT:
            {input}
        ";
        }
    }
}
