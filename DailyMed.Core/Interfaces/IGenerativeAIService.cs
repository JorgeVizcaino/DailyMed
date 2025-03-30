using DailyMed.Core.Models.CopayCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Core.Interfaces
{
    public interface IGenerativeAIService
    {
        Task<List<RequirementDto>> InferRequirementsFromEligibilityAsync(string freeText, IPromptStrategy promptStrategy);
        Task<List<string>> InferCoverageEligibilityAsync(string freeText, IPromptStrategy promptStrategy);
    }
}
