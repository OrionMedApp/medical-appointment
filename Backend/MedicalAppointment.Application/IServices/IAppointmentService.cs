using MedicalAppointment.Application.DTOs.Appointment;
using MedicalAppointment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointment.Application.IServices
{
    public interface IAppointmentService
    {
        Task<Appointment> GetByIdAsync(Guid guid);
        Task<Appointment> CreateAsync(CreateAppointmentDTO createAppointmentDTO);

        Task<Appointment> UpdateAsync(Guid Id,UpdateAppointmentDTO updateAppointmentDTO);

        Task<bool> DeleteAsync(Guid id);

    }
}
