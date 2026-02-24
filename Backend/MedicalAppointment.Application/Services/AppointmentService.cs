using MedicalAppointment.Application.DTOs.Appointment;
using MedicalAppointment.Application.IServices;
using MedicalAppointment.Domain.Entities;
using MedicalAppointment.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointment.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;
        public AppointmentService(IAppointmentRepository repository)
        {
            _repository=repository;
        }
        public async Task<Appointment> CreateAsync(CreateAppointmentDTO appoinment)
        {
            Appointment app = new Appointment(appoinment.PatientId, appoinment.DoctorId, appoinment.Type, appoinment.StartTime, appoinment.EndTime, appoinment.Notes);
            return  await _repository.AddAsync(app);
        }

        public async Task<Appointment?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
