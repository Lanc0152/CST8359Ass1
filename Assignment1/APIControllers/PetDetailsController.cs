using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Assignment1.Models;
using Assignment1.Data;
using Microsoft.EntityFrameworkCore;
using Assignment1.Models.DataTransferObjects;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Assignment1.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetDetailsController : ControllerBase
    {
        VetSystemDbContext _context;

        public PetDetailsController(VetSystemDbContext context)
        {
            _context = context;
        }
        // GET: api/<PetDetailsController>
        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<IEnumerable<PetDetails>>> GetNotes()
        //{
        //    var vets = await _context.PetDetails
        //            .Include(pd => pd.Pet)
        //            .Select(pd => new PetDetailsDto
        //            {
        //                Id = pd.Id,
        //                VetNotes = pd.VetNotes,
        //                Pet = new PetSummaryDto
        //                {
        //                    Id = pd.Pet.Id,
        //                    Name = pd.Pet.Name,
        //                    Species = pd.Pet.Species,
        //                    MicrochipId = pd.Pet.MicrochipId
        //                }
        //            })
        //            .ToListAsync();

        //    return Ok(vets);
        //}

        // GET api/<PetDetailsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PetDetails>> GetNote(int id)
        {
            var pet = await _context.PetDetails
                .Include(p => p.Pet)
                .Where(p => p.Pet.Id == id)
                .Select(p => new PetDetailsDto
                {
                    Id = p.Id,
                    VetNotes = p.VetNotes,
                    Pet = new PetSummaryDto
                    {      
                        Id = p.Pet.Id,
                        Name = p.Pet.Name,
                        Species = p.Pet.Species,
                        MicrochipId = p.Pet.MicrochipId
                    }
                })
                .FirstOrDefaultAsync();

            if (pet == null)
                return NotFound();

            return Ok(pet);
        }

        // POST api/<PetDetailsController>
        [HttpPost("{petId}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(int petId, [FromBody] PetDetailsCreateDto dto)
        {
            var petDetails = await _context.PetDetails.FirstOrDefaultAsync(p => p.PetId == petId);
            if (petDetails == null)
                return NotFound($"PetDetails for Pet ID {petId} not found.");

            petDetails.VetNotes = dto.VetNotes;

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Pet notes updated successfully.");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update pet notes.");
            }
        }


        // PUT api/<PetDetailsController>/5
        [HttpPut("{petId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int petId, [FromBody] PetDetailsCreateDto dto)
        {
            return await Post(petId, dto); // Reuse logic from Post
        }


        // DELETE api/<PetDetailsController>/5
        //[HttpDelete("{petId}")]
        //[ProducesResponseType(StatusCodes.Status202Accepted)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> Delete(int petId)
        //{
        //    //var petDetails = await _context.PetDetails.FirstOrDefaultAsync(p => p.PetId == petId);
        //    //if (petDetails == null)
        //    //    return NotFound($"PetDetails for Pet ID {petId} not found.");

        //    //try
        //    //{
        //    //    _context.PetDetails.Remove(petDetails);
        //    //    await _context.SaveChangesAsync();
        //    //    return Accepted($"Pet notes for Pet ID {petId} deleted.");
        //    //}
        //    //catch
        //    //{
        //    //    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete pet notes.");
        //    //}
        //}
    }
}
