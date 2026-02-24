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
                
            var created = await _repository.AddAsync(newPatient);
            return created;
        }




        public async Task<List<ReturnPatientDTO>> GetAllAsync(int page = 1, int pageSize = 20)
        {
            
            var patients = await _repository.GetAllAsync();

            
            var paged = patients
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ReturnPatientDTO
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Email = p.Email,
                    Phone = p.Phone,
                    MedicalId = p.MedicalId
                })
                .ToList();

            return paged;
        }
    
        public async Task<bool> DeleteAsync(Guid id)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null)
                return false;
            await _repository.DeleteAsync(patient);
            return true;

        }

        public Task<byte[]> GetAllPatientsCsvAsync()
        {
            throw new NotImplementedException();
        }
    }
}
