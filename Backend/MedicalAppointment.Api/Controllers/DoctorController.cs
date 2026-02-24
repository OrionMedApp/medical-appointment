using MedicalAppointment.Application.DTOs.Doctor;
using MedicalAppointment.Application.DTOs.Patient;
using MedicalAppointment.Application.IServices;
using MedicalAppointment.Domain.Entities;
using MedicalAppointment.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : Controller
    {
        private readonly IDoctorService _service;

        public DoctorController(IDoctorService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<Doctor>> Create([FromBody] CreateDoctorDTO doctor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var created = await _service.CreateAsync(doctor);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (DomainValidationException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (DbUpdateException)
            {
                return StatusCode(500, "Database error while saving doctor");
            }
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Doctor>> GetById(Guid id)
        {
            var doctor = await _service.GetByIdAsync(id);

            if (doctor == null)
                return NotFound();

            return Ok(doctor);
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Doctor>> Update(Guid id, [FromBody] UpdateDoctorDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _service.UpdateAsync(id, dto);

                if (updated == null)
                    return NotFound();

                return Ok(updated);
            }
            catch (DomainValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Database error while updating doctor");
            }
        }
    }
}
