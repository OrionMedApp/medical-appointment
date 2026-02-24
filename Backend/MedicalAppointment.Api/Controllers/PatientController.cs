using MedicalAppointment.Application.DTOs.Patient;
using MedicalAppointment.Application.IServices;
using MedicalAppointment.Domain.Entities;
using MedicalAppointment.Domain.Exceptions;
using MedicalAppointment.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _service;

        public PatientController(IPatientService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<Patient>> Create([FromBody] CreatePatientDTO patient)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var created = await _service.CreateAsync(patient);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (DomainValidationException ex)
            {
                return BadRequest(ex.Message);
            }
          
            catch (DbUpdateException) {
                return StatusCode(500, "Databse error while saving patient");
            }
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Patient>> GetById(Guid id)
        {
            var patient = await _service.GetByIdAsync(id);

            if (patient == null)
                return NotFound();

            return Ok(patient);
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

    }
}
