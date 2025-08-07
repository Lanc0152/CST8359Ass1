using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Assignment1.Models;
using Assignment1.Models.DataTransferObjects;
using Assignment1.Data;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Assignment1.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        VetSystemDbContext _context;
        public PetsController(VetSystemDbContext context) { 
            _context = context;
        }
        // GET: api/<PetsController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Pet>>> GetPets()
        {
            var pets = await _context.Pets
                    .Include(p => p.VetDoctor)
                    .Include(p => p.PetDetails)
                    .Select(p => new PetDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Species = p.Species,
                        MicrochipId = p.MicrochipId,
                        VetDoctor = new VetDoctorSummaryDto
                        {
                            Id = p.VetDoctor.Id,
                            Name = p.VetDoctor.Name,
                            Specialty = p.VetDoctor.Specialty
                        },
                        PetNotes = new PetDetailsSummaryDto
                        {
                            Id = p.PetDetails.Id,
                            VetNotes = p.PetDetails.VetNotes
                        }
                    })
                    .ToListAsync();

            return Ok(pets);
        }

        // GET api/<PetsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Pet>> GetPet(int id)
        {
            var pet = await _context.Pets
                .Include(p => p.VetDoctor)
                .Where(p => p.Id == id)
                .Select(p => new PetDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Species = p.Species,
                    MicrochipId = p.MicrochipId,
                    VetDoctor = new VetDoctorSummaryDto
                    {
                        Id = p.VetDoctor.Id,
                        Name = p.VetDoctor.Name,
                        Specialty = p.VetDoctor.Specialty
                    },
                    PetNotes = new PetDetailsSummaryDto
                    { 
                        Id = p.PetDetails.Id,
                        VetNotes = p.PetDetails.VetNotes
                    }
                })
                .FirstOrDefaultAsync();

            if (pet == null)
                return NotFound();

            return Ok(pet);
        }


        // POST api/<PetsController>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PetDto>> Post([FromBody] PetCreateDto dto)
        {
            var pet = new Pet
            {
                Name = dto.Name,
                MicrochipId = dto.MicrochipId,
                Species = dto.Species,
                VetDoctorId = dto.VetDoctorId
            };

            _context.Pets.Add(pet);
            await _context.SaveChangesAsync(); // Save to get the pet's Id

            var petNote = new PetDetails
            {
                PetId = pet.Id,
                VetNotes = "" // or "No notes yet", or null
            };
            
            _context.PetDetails.Add(petNote);
            await _context.SaveChangesAsync();

            var result = await _context.Pets
                .Include(p => p.VetDoctor)
                .Where(p => p.Id == pet.Id)
                .Select(p => new PetDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Species = p.Species,
                    MicrochipId = p.MicrochipId,
                    VetDoctor = new VetDoctorSummaryDto
                    {
                        Id = p.VetDoctor.Id,
                        Name = p.VetDoctor.Name
                    }
                })
                .FirstOrDefaultAsync();
            try { 
            return StatusCode(StatusCodes.Status201Created, "Pet created.");
        }
    catch (Exception)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create pet.");
    }
}


        // PUT api/<PetsController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] PetCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid request body.");

            var pet = await _context.Pets.FindAsync(id);

            if (pet == null)
                return NotFound($"Pet with ID {id} not found.");

            // Update pet fields
            pet.Name = dto.Name;
            pet.MicrochipId = dto.MicrochipId;
            pet.Species = dto.Species;
            pet.VetDoctorId = dto.VetDoctorId;

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Pet updated successfully.");
            }
            catch (Exception ex)
            {
                // You can also log ex here
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update pet.");
            }
        }


        // DELETE api/<PetsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID.");

            var pet = await _context.Pets
                .Include(p => p.PetDetails) 
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pet == null)
                return NotFound($"Pet with ID {id} not found.");

            try
            {
                // First remove the pet note (if it exists)
                if (pet.PetDetails != null)
                {
                    _context.PetDetails.Remove(pet.PetDetails);
                }

                // Then remove the pet
                _context.Pets.Remove(pet);
                await _context.SaveChangesAsync();

                return Accepted($"Pet with ID {id} deleted.");
            }
            catch (Exception ex)
            {
                // Log error (optional)
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete pet.");
            }
        }

    }
}
