using MedicalAppointment.Application.DTOs.Appointment;
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
        public byte[] ExportAppointments(IEnumerable<ReturnAppointmentDTO> appointments)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Id,PatientId,PatientFullName,PatientEmail,PatientPhone," +
                          "DoctorId,DoctorFullName,DoctorEmail,DoctorPhone,Specialization," +
                          "Type,Status,StartTime,EndTime,Notes");

            foreach (var a in appointments)
            {
                sb.AppendLine(
                    $"{a.Id}," +
                    $"{a.Patient.Id}," +
                    $"{Escape(a.Patient.FullName)}," +
                    $"{Escape(a.Patient.Email)}," +
                    $"{Escape(a.Patient.Phone)}," +
                    $"{a.Doctor.Id}," +
                    $"{Escape(a.Doctor.FullName)}," +
                    $"{Escape(a.Doctor.Email)}," +
                    $"{Escape(a.Doctor.Phone)}," +
                    $"{a.Doctor.Specialization}," +
                    $"{a.Type}," +
                    $"{a.Status}," +
                    $"{a.StartTime:O}," +
                    $"{a.EndTime:O}," +
                    $"{Escape(a.Notes)}"
                );
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

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
