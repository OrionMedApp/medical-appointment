using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace MedicalAppointment.Api.OpenAI;

public sealed class OpenAiClient
{
    private readonly HttpClient _http;
    private readonly OpenAiOptions _opt;

    public OpenAiClient(HttpClient http, IOptions<OpenAiOptions> opt)
    {
        _http = http;
        _opt = opt.Value;
    }

    public async Task<string> GeneratePatientsJsonAsync(int count, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(_opt.ApiKey))
            throw new InvalidOperationException("OpenAI ApiKey is missing. Configure OpenAI:ApiKey.");

        if (count <= 0) count = 10;
        if (count > 200) count = 200;

        var system =
            "You generate realistic Serbian patient data. " +
            "Return ONLY valid JSON. No markdown. No explanation. No extra keys.";

        var user =
    "Generate " + count + " patients as a JSON array.\n\n" +
    "Each item MUST have EXACTLY these keys:\n" +
    "- FirstName (string, max 20 chars)\n" +
    "- LastName  (string, max 20 chars)\n" +
    "- Email     (valid email, unique)\n" +
    "- Phone     (valid Serbian phone, unique)\n\n" +
    "Return ONLY the JSON array. Example:\n" +
    "[\n" +
    "  {\n" +
    "    \"FirstName\": \"Ana\",\n" +
    "    \"LastName\": \"Jovic\",\n" +
    "    \"Email\": \"ana.jovic1@example.com\",\n" +
    "    \"Phone\": \"+381641234567\"\n" +
    "  }\n" +
    "]";

        using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _opt.ApiKey);

        var body = new
        {
            model = _opt.Model,
            temperature = 0.3,
            messages = new object[]
            {
                new { role = "system", content = system },
                new { role = "user", content = user }
            }
        };

        req.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

        using var resp = await _http.SendAsync(req, ct);
        var respText = await resp.Content.ReadAsStringAsync(ct);

        if (!resp.IsSuccessStatusCode)
            throw new InvalidOperationException($"OpenAI error {(int)resp.StatusCode}: {respText}");

        using var doc = JsonDocument.Parse(respText);
        var content = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        if (string.IsNullOrWhiteSpace(content))
            throw new InvalidOperationException("OpenAI returned empty content.");

        // Safety: strip ```json fences if model returns them anyway
        content = content.Trim();
        if (content.StartsWith("```"))
        {
            content = content.Replace("```json", "", StringComparison.OrdinalIgnoreCase)
                             .Replace("```", "")
                             .Trim();
        }

        // Validate that it is actually JSON array
        using var test = JsonDocument.Parse(content);
        if (test.RootElement.ValueKind != JsonValueKind.Array)
            throw new InvalidOperationException("OpenAI did not return a JSON array.");

        return content;
    }
}