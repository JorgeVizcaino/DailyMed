using DailyMed.Core.Models.CopayCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Core.Models.Program
{
    public class CopayProgram
    {
        public int ProgramId { get; set; }
        public string ProgramName { get; set; }
        public string[] CoverageEligibilities { get; set; }
        public string ProgramType { get; set; }
        public List<ProgramRequirement> Requirements { get; set; }
        public List<ProgramBenefit> Benefits { get; set; }
        public List<ProgramForm> Forms { get; set; }
        public FundingData Funding { get; set; }
        public List<ProgramDetail> Details { get; set; }
    }
}
