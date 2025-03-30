using DailyMed.Core.Interfaces;
using DailyMed.Core.Models.CopayCard;
using DailyMed.Infrastructure.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DailyMed.Infrastructure.Data
{
    public class CopayParserJson : ICopayParserJson
    {

        private readonly IGenerativeAIService _generativeAIService;
        private readonly IFileReader _fileReader;

        public CopayParserJson(IGenerativeAIService generativeAIService, IFileReader fileReader)
        {
            _generativeAIService = generativeAIService;
            _fileReader = fileReader;
        }

        public async Task<StandardCopayCardDto> DoConvertAsync()
        {
            var card = await ReadFromJsonAsync("StaticFiles\\dupixent.json");
            return await ConvertAsync(card);
        }

        private async Task<CopayCard> ReadFromJsonAsync(string filePath)
        {
            var json = await _fileReader.ReadAllTextAsync(filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<CopayCard>(json, options)!;
        }

        private async Task<StandardCopayCardDto> ConvertAsync(CopayCard card)
        {
            var strategyEligibilitty = PromptStrategyFactory.GetStrategy(PromptType.Summarize);
            var stringCoverage = string.Join(",",card.CoverageEligibilities);
            List<string> coverage;
            try
            {
                coverage = await _generativeAIService.InferCoverageEligibilityAsync(stringCoverage, strategyEligibilitty);
            }
            catch (Exception ex)
            {
                coverage = new List<string> { "" };
            }


            var strategy = PromptStrategyFactory.GetStrategy(PromptType.Eligibility);
            List<RequirementDto> requirements;
            try
            {
                requirements = await _generativeAIService.InferRequirementsFromEligibilityAsync(card.EligibilityDetails, strategy);
            }
            catch (Exception ex)
            {
                requirements = new List<RequirementDto>();
            }

            return new StandardCopayCardDto
            {
                ProgramName = card.ProgramName,
                CoverageEligibilities = coverage,
                ProgramType = card.AssistanceType ?? "Coupon",
                Requirements = requirements,
                Benefits = new List<BenefitDto>
                {
                    new BenefitDto
                    {
                        Name = "max_annual_savings",
                        Value = card.AnnualMax?.Replace("$", "") ?? ""
                    }
                },
                Forms = new List<FormDto>
                {
                    new FormDto
                    {
                        Name = "Enrollment Form",
                        Link = card.EnrollmentURL
                    }
                },
                Funding = new FundingDto
                {
                    Evergreen = card.OfferRenewable ? "true" : "false",
                    CurrentFundingLevel = "Data Not Available"
                },
                Details = new List<DetailItemDto>
                {
                    new DetailItemDto
                    {
                        Eligibility = card.EligibilityDetails,
                        Program = card.ProgramDetails,
                        Renewal = card.AddRenewalDetails,
                        Income = card.IncomeReq ? "Required" : "Not required"
                    }
                }
            };
        }
    }
}
