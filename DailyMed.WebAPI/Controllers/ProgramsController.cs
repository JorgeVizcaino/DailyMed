using DailyMed.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DailyMed.Core.Models;
using DailyMed.Application.Services;
using DailyMed.Core.Applications;
using DailyMed.Infrastructure.ICD10;
using DailyMed.Infrastructure.ICD10.SearchICD10;

namespace DailyMed.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramsController : ControllerBase
    {
        private readonly ICopayProgramApplication _repo;
        private readonly IDailyMedRepository _medRepository;
        private readonly ICopayParserJson _copayParserJson;

        public ProgramsController(ICopayProgramApplication repo
            , IDailyMedRepository medRepository
            , ICopayParserJson copayParserJson)
        {
            _repo = repo;
            _medRepository = medRepository;
            _copayParserJson = copayParserJson;
        }

        [HttpGet("{program_id}")]
        [Authorize]
        public async Task<IActionResult> GetProgram(int program_id)
        {
       
            var program = await _repo.GetCopayProgramByIdAsync(program_id);
            if (program == null) return NotFound();
            return Ok(program);
        }

        [HttpGet("CreateMockProgram")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateMockProgram()
        {

            var program = await _repo.MockProgramApplication();
            if (program == null) return NotFound();
            return Ok(program);
        }

        [HttpGet("dupixent-indications")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetDupixentIndications()
        {
            var result = await _medRepository.SearchIndicationsAsync("dupixent");
            return Ok(result);
        }

        [HttpGet("StandardizeAIRequirements")]
        [Authorize]
        public async Task<IActionResult> StandardizeAIRequirements()
        {
            var reply = await _copayParserJson.DoConvertAsync();
            return Ok(reply);
        }

        [HttpGet("ICD10/IndicationProcessing")]
        [Authorize]
        public async Task<IActionResult> IndicationProcessing(string search)
        {
            
            var searchContext = new SearchRepository();

            var codeList = new List<ICD10Code>();

            searchContext.SetStrategy(new SynonymSearchStrategy());
            var synomynResults = searchContext.ExecuteSearch(search).ToList();
            codeList.AddRange(synomynResults);

            searchContext.SetStrategy(new CodeSearchStrategy());
            var codeResults = searchContext.ExecuteSearch(search).ToList();
            codeList.AddRange(codeResults);

            searchContext.SetStrategy(new DescriptionSearchStrategy());
            var DescriptionResults = searchContext.ExecuteSearch(search).ToList();
            codeList.AddRange(DescriptionResults);

            if ((synomynResults.Count + codeResults.Count + DescriptionResults.Count) == 0)
            {
                return Ok("UNMAPPABLE");
            }
            return Ok(codeList);
        }
    }
}
