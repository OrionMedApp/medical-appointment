using MedicalAppointment.Application.DTOs.Patient;
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
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;

        public PatientService(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<Patient?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Patient> CreateAsync(CreatePatientDTO patient)
        {
            Guid medicalId = Guid.NewGuid();
            Patient newPatient = new Patient(patient.FirstName, patient.LastName, patient.Email, patient.Phone, medicalId);
                
            await _repository.AddAsync(newPatient);
            return newPatient;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null)
                return false;
            await _repository.DeleteAsync(patient);
            return true;
        }
    }
}
