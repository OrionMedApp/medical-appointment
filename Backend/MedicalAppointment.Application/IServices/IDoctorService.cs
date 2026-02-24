using MedicalAppointment.Application.DTOs.Doctor;
using MedicalAppointment.Application.DTOs.Patient;
using MedicalAppointment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointment.Application.IServices
{
    public interface IDoctorService
    {
        Task<Doctor?> GetByIdAsync(Guid id);
        Task<Doctor> CreateAsync(CreateDoctorDTO doctor);
        Task<Doctor?> UpdateAsync(Guid id, UpdateDoctorDTO dto);
    }
}
