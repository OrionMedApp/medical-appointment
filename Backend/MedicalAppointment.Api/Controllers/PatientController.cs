using MedicalAppointment.Application.DTOs;
using MedicalAppointment.Application.IServices;
using MedicalAppointment.Domain.Entities;
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
        public async Task<ActionResult<Patient>> Create(PatientDTO patient)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var created = await _service.CreateAsync(patient);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the patient: {ex.Message}");
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

    }
}
