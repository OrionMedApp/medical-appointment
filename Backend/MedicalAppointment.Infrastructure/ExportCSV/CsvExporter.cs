using MedicalAppointment.Application.DTOs.Patient;
using MedicalAppointment.Application.ExportCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointment.Infrastructure.ExportCSV
{
    public class CsvExporter : ICsvExporter
    {
        public byte[] ExportPatients(IEnumerable<ReturnPatientDTO> patients)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Id,FirstName,LastName,Email,Phone,MedicalId");

            foreach (var p in patients)
            {
                sb.AppendLine($"{p.Id},{Escape(p.FirstName)},{Escape(p.LastName)},{Escape(p.Email)},{Escape(p.Phone)},{p.MedicalId}");
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        private string Escape(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            if (value.Contains(",") || value.Contains("\""))
            {
                value = value.Replace("\"", "\"\"");
                return $"\"{value}\"";
            }

            return value;
        }
    }
}
