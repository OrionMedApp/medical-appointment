using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MedicalAppointment.Api.OpenAI;
using MedicalAppointment.Application.DTOs.Patient;
using MedicalAppointment.Application.IServices;

namespace MedicalAppointment.Api.Controllers;

[ApiController]
[Route("api/ai")]
public class AiController : ControllerBase
{
    private readonly OpenAiClient _ai;
    private readonly IPatientService _patients;

    public AiController(OpenAiClient ai, IPatientService patients)
    {
        _ai = ai;
        _patients = patients;
    }

    [HttpPost("patients")]
    public async Task<ActionResult<BulkInsertPatientsResponse>> GeneratePatients([FromQuery] int count = 10, CancellationToken ct = default)
    {
        var json = await _ai.GeneratePatientsJsonAsync(count, ct);

        var list = JsonSerializer.Deserialize<List<CreatePatientDTO>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (list == null || list.Count == 0)
            return BadRequest("AI returned an empty list.");

        var result = await _patients.BulkInsertAsync(list);
        return Ok(result);
    }
}