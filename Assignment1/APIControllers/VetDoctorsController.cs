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
    public class VetDoctorsController : ControllerBase
    {
        VetSystemDbContext _context;

        public VetDoctorsController(VetSystemDbContext context)
        {
            _context = context;
        }
        // GET: api/<VetDoctorsController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<VetDoctor>>> GetVets()
        {
            var vets = await _context.VetDoctors
                    .Include(v => v.Pets)
                    .Select(v => new VetDoctorDto
                    {
                        Id = v.Id,
                        Name = v.Name,
                        Specialty = v.Specialty,
                        Pets = v.Pets.Select(p => new PetSummaryDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Species = p.Species,
                            MicrochipId = p.MicrochipId
                        }).ToList()
                    })
                    .ToListAsync();

            return Ok(vets);
        }

        // GET api/<VetDoctorsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VetDoctor>> GetVet(int id)
        {
            var vet = await _context.VetDoctors
                .Include(v => v.Pets)
                .Where(v => v.Id == id)
                .Select(v => new VetDoctorDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    Specialty = v.Specialty,
                    Pets = v.Pets.Select(p => new PetSummaryDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Species = p.Species,
                        MicrochipId = p.MicrochipId
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (vet == null)
                return NotFound();

            return Ok(vet);
        }

        // POST api/<VetDoctorsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] VetDoctorCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid vet data.");

            var vet = new VetDoctor
            {
                Name = dto.Name,
                Specialty = dto.Specialty
            };

            try
            {
                _context.VetDoctors.Add(vet);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status201Created, "VetDoctor created.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create vet.");
            }
        }


        // PUT api/<VetDoctorsController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] VetDoctorCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid request body.");

            var vet = await _context.VetDoctors.FindAsync(id);

            if (vet == null)
                return NotFound($"VetDoctor with ID {id} not found.");

            vet.Name = dto.Name;
            vet.Specialty = dto.Specialty;

            try
            {
                await _context.SaveChangesAsync();
                return Ok("VetDoctor updated successfully.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update vet.");
            }
        }


        // DELETE api/<VetDoctorsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var vet = await _context.VetDoctors.FindAsync(id);

            if (vet == null)
                return NotFound($"VetDoctor with ID {id} not found.");

            try
            {
                _context.VetDoctors.Remove(vet);
                await _context.SaveChangesAsync();
                return Accepted("VetDoctor deleted.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete vet.");
            }
        }

    }
}
