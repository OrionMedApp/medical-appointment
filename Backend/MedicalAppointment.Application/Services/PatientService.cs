using MedicalAppointment.Application.DTOs.Patient;
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
                
            var created = await _repository.AddAsync(newPatient);
            return created;
        }

        public async Task<List<ReturnPatientDTO>> GetAllAsync()
        {
            var patients = await _repository.GetAllAsync();
            return patients.Select(p => new ReturnPatientDTO
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName
            }).ToList();
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null)
                return false;
            await _repository.DeleteAsync(patient);
            return true;

        }

        public async Task<Patient?> UpdateAsync(Guid id, UpdatePatientDTO dto)
        {
            var patient = await _repository.GetByIdAsync(id);

            if (patient == null)
                return null;

            if (string.IsNullOrWhiteSpace(dto.FirstName))
                throw new DomainValidationException("First name is required");

            if (string.IsNullOrWhiteSpace(dto.LastName))
                throw new DomainValidationException("Last name is required");

            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;
            patient.Email = dto.Email;
            patient.Phone = dto.Phone;

            await _repository.UpdateAsync(patient);

            return patient;
        }
    }
}
