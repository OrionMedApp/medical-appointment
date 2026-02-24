using MedicalAppointment.Application.DTOs.Appointment;
using MedicalAppointment.Application.IServices;
using MedicalAppointment.Domain.Entities;
using MedicalAppointment.Domain.Exceptions;
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
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        public AppointmentService(IAppointmentRepository repository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository)
        {
            _repository = repository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }
        public async Task<Appointment> CreateAsync(CreateAppointmentDTO appoinment)
        {
            var patient = await _patientRepository.GetByIdAsync(appoinment.PatientId);
            if (patient is null)
                throw new DomainValidationException("Patient does not exist");

            var doctor = await _doctorRepository.GetByIdAsync(appoinment.DoctorId);
            if (doctor is null)
                throw new DomainValidationException("Doctor does not exist");
            Appointment app = new Appointment(appoinment.PatientId, appoinment.DoctorId, appoinment.Type, appoinment.StartTime, appoinment.EndTime, appoinment.Notes);
            return  await _repository.AddAsync(app);
        }

        public async Task<Appointment?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }


        public async Task<Appointment> UpdateAsync(Guid Id,UpdateAppointmentDTO appoinment)
        {
            var app = await _repository.GetByIdAsync(Id);
            if (app is null)
                throw new DomainValidationException("Appointment does not exist");

            var patient = await _patientRepository.GetByIdAsync(appoinment.PatientId);
            if (patient is null)
                throw new DomainValidationException("Patient does not exist");

            var doctor = await _doctorRepository.GetByIdAsync(appoinment.DoctorId);
            if (doctor is null)
                throw new DomainValidationException("Doctor does not exist");
            app.Status = appoinment.Status;
            app.PatientId = appoinment.PatientId;
            app.DoctorId = appoinment.DoctorId;
            app.Type = appoinment.Type;
            app.StartTime = appoinment.StartTime;
            app.EndTime = appoinment.EndTime;
            app.Notes = appoinment.Notes;

           
            return await _repository.UpdateAsync(app);
        }
    }
}
