using DailyMed.Core.Applications;
using DailyMed.Core.Models.CopayCard;
using Microsoft.AspNetCore.Mvc;

namespace DailyMed.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrugIndicationsController : Controller
    {
        private readonly IDrugIndicationsApplication _repository;

        public DrugIndicationsController(IDrugIndicationsApplication repository)
        {
            _repository = repository;
        }

        // GET: api/DrugIndications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DrugIndication>>> GetAll()
        {
            var items = await _repository.GetAllAsync();
            return Ok(items);
        }

        // GET: api/DrugIndications/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DrugIndication>> GetById(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
                return NotFound(); // 404

            return Ok(item);
        }

        // POST: api/DrugIndications
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] DrugIndication model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int newId = await _repository.CreateDrugIndicationsAsync(model);

            return CreatedAtAction(
                nameof(GetById),        
                new { id = newId },     
                model                   
            );
        }

        // PUT: api/DrugIndications/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] DrugIndication model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Ensure the url ID matches the model ID
            if (id != model.Id)
                return BadRequest("ID mismatch between URL and body");

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();
            
            bool updated = await _repository.UpdateAsync(model);

            if (!updated)
                return NotFound();

            return NoContent(); 
        }

        // DELETE: api/DrugIndications/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            bool deleted = await _repository.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
