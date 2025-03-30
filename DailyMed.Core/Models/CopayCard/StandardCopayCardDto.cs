using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Core.Models.CopayCard
{
    public class StandardCopayCardDto
    {
        public string ProgramName { get; set; }
        public List<string> CoverageEligibilities { get; set; }
        public string ProgramType { get; set; }

        public List<RequirementDto> Requirements { get; set; }
        public List<BenefitDto> Benefits { get; set; }
        public List<FormDto> Forms { get; set; }

        public FundingDto Funding { get; set; }
        public List<DetailItemDto> Details { get; set; }
    }
}
