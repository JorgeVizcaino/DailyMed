using DailyMed.Core.Interfaces;
using DailyMed.Infrastructure.AI.Prompt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Infrastructure.AI
{
    public static class PromptStrategyFactory
    {
        public static IPromptStrategy GetStrategy(PromptType type)
        {
            switch (type)
            {
                case PromptType.Summarize:
                    return new SummarizePromptStrategy();
                case PromptType.Eligibility:
                    return new EligibilityPromptStrategy();
                default:
                    throw new NotImplementedException($"No strategy for prompt type {type}");
            }
        }
    }
}
