using MedicalAppointment.Application.DTOs.Patient;
using MedicalAppointment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointment.Application.IServices
{
    public interface IPatientService
    {
        Task<Patient?> GetByIdAsync(Guid id);
        Task<Patient> CreateAsync(CreatePatientDTO patient);
        Task<List<ReturnPatientDTO>> GetAllAsync();
        Task<bool> DeleteAsync(Guid id);
        Task<Patient?> UpdateAsync(Guid id, UpdatePatientDTO dto);

    }
}
