using DailyMed.Core.Interfaces;
using DailyMed.Core.Models;
using DailyMed.Core.Models.CopayCard;
using DailyMed.Core.Models.Program;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Application.Services
{
    public class CopayProgramApplication : ICopayProgramApplication
    {
        private readonly ICopayProgramRepository _copayProgramRepository;

        public CopayProgramApplication(ICopayProgramRepository copayProgramRepository)
        {
            _copayProgramRepository = copayProgramRepository;
        }
        public async Task<int> MockProgramApplication()
        {
            var program = new CopayProgram
            {
                ProgramName = "Dupixent MyWay Copay Card",
                CoverageEligibilities = new[] { "Commercially insured" },
                ProgramType = "Coupon",
                Requirements = new List<ProgramRequirement>
                            {
                                new ProgramRequirement { Name = "us_residency", Value = "true" },
                                new ProgramRequirement { Name = "minimum_age", Value = "18" },
                                new ProgramRequirement { Name = "insurance_coverage", Value = "true" },
                                new ProgramRequirement { Name = "eligibility_length", Value = "12m" },
                            },
                Benefits = new List<ProgramBenefit>
                            {
                                new ProgramBenefit { Name = "max_annual_savings", Value = "13000.00" },
                                new ProgramBenefit { Name = "min_out_of_pocket", Value = "0.00" }
                            },
                Forms = new List<ProgramForm>
                            {
                                new ProgramForm { Name = "Enrollment Form", Link = "https://www.dupixent.com/support-savings/copay-card" }
                            },
                Funding = new FundingData
                {
                    Evergreen = true,
                    CurrentFundingLevel = "Data Not Available"
                },
                Details = new List<ProgramDetail>
                            {
                                new ProgramDetail
                                {
                                    Eligibility = "Patient must have commercial insurance and be a legal resident of the US",
                                    Program = "Patients may pay as little as $0 for every month of Dupixent",
                                    Renewal = "Automatically re-enrolled every January 1st if used within 18 months",
                                    Income = "Not required"
                                }
                            }
            };

            return await _copayProgramRepository.CreateCopayProgramAsync(program);
        }

        public async Task<CopayProgram> GetCopayProgramByIdAsync(int programId)
        {
            return await _copayProgramRepository.GetCopayProgramByIdAsync(programId);
        }
    }
}
