using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Core.Models.CopayCard
{
    public class CopayCard
    {
        public int ProgramID { get; set; }
        public string ProgramName { get; set; }
        public List<string> TherapeuticAreas { get; set; }
        public List<string> Drugs { get; set; }
        public List<string> CoverageEligibilities { get; set; }
        public string ProgramURL { get; set; }
        public string HelpLine { get; set; }
        public bool EnrollmentReq { get; set; }
        public string EnrollmentURL { get; set; }
        public string ExpirationDate { get; set; }
        public string EligibilityDetails { get; set; }
        public bool IncomeReq { get; set; }
        public string IncomeDetails { get; set; }
        public string AnnualMax { get; set; }
        public bool OfferRenewable { get; set; }
        public string RenewalMethod { get; set; }
        public string AddRenewalDetails { get; set; }
        public string ProgramDetails { get; set; }
        public string CouponVehicle { get; set; }
        public string AssistanceType { get; set; }
        public string LastUpdated { get; set; }
    }
}
