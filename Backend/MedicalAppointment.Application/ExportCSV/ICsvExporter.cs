using MedicalAppointment.Application.DTOs.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointment.Application.ExportCSV
{
    public interface ICsvExporter
    {
        byte[] ExportPatients(IEnumerable<ReturnPatientDTO> patients);
    }
}
