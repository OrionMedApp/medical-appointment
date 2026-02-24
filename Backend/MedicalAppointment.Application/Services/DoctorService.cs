using MedicalAppointment.Application.DTOs.Doctor;
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
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;

        public DoctorService(IDoctorRepository repository)
        {
            _repository = repository;
        }
        public async Task<Doctor> CreateAsync(CreateDoctorDTO doctor)
        {
            Guid medicalId = Guid.NewGuid();
            Doctor _doctor = new Doctor(doctor.FirstName,doctor.LastName,doctor.Email,doctor.Phone,doctor.Specialization);
            await _repository.AddAsync(_doctor);
            return _doctor;
        }

        public async Task<List<ReturnDoctorDTO>> GetAllAsync(int page = 1, int pageSize = 20)
        {
            var doctors = await _repository.GetAllAsync();


            var paged = doctors
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ReturnDoctorDTO
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Email = p.Email,
                    Phone = p.Phone,
                    Specialization = p.Specialization
                })
                .ToList();

            return paged;
        }

        public async Task<Doctor?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
