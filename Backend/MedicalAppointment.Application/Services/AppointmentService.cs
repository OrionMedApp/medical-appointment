using MedicalAppointment.Application.DTOs.Appointment;
using MedicalAppointment.Application.DTOs.Doctor;
using MedicalAppointment.Application.DTOs.Patient;
using MedicalAppointment.Application.ExportCSV;
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
        private readonly ICsvExporter _csvExporter;
        private readonly IAvailabilitySlotRepository _slotRepository;
        public AppointmentService(IAppointmentRepository repository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            ICsvExporter csvExporter,
            IAvailabilitySlotRepository slotRepository)
        {
            _repository = repository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _csvExporter = csvExporter;
            _slotRepository = slotRepository;
        }
        public async Task<Appointment> CreateAsync(CreateAppointmentDTO appoinment)
        {
            var patient = await _patientRepository.GetByIdAsync(appoinment.PatientId);
            if (patient is null)
                throw new DomainValidationException("Patient does not exist");

            var doctor = await _doctorRepository.GetByIdAsync(appoinment.DoctorId);
            if (doctor is null)
                throw new DomainValidationException("Doctor does not exist");
            if (!Enum.IsDefined(typeof(AppointmentType), appoinment.Type))

                throw new DomainValidationException("Invalid appointment type.");

            if (!Enum.IsDefined(typeof(AppointmentStatus), appoinment.Status))

                throw new DomainValidationException("Invalid appointment status.");
            Appointment app = new Appointment(appoinment.PatientId, appoinment.DoctorId, appoinment.Type, appoinment.StartTime, appoinment.EndTime, appoinment.Notes);
            return await _repository.AddAsync(app);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var appointment = await _repository.GetByIdAsync(id);

            if (appointment == null)
                return false;

            await _repository.DeleteAsync(id);

            return true;
        }

        public async Task<Appointment?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }


        public async Task<Appointment> UpdateAsync(Guid Id, UpdateAppointmentDTO dto)
        {
            var app = await _repository.GetByIdAsync(Id);
            if (app is null)
                throw new DomainValidationException("Appointment does not exist");

            var patient = await _patientRepository.GetByIdAsync(dto.PatientId);
            if (patient is null)
                throw new DomainValidationException("Patient does not exist");

            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);
            if (doctor is null)
                throw new DomainValidationException("Doctor does not exist");

            if (!Enum.IsDefined(typeof(AppointmentType), dto.Type))
                throw new DomainValidationException("Invalid appointment type.");

            if (!Enum.IsDefined(typeof(AppointmentStatus), dto.Status))
                throw new DomainValidationException("Invalid appointment status.");

            if (dto.StartTime >= dto.EndTime)
                throw new DomainValidationException("Start time must be before end time");

            if (dto.StartTime < DateTime.UtcNow)
                throw new DomainValidationException("Appointment cannot be scheduled in the past");

            if (!string.IsNullOrWhiteSpace(dto.Notes) && dto.Notes.Length > 2000)
                throw new DomainValidationException("Notes cannot exceed 2000 characters");

            // 🔹 zapamti stari slot
            var oldDoctorId = app.DoctorId;
            var oldStart = app.StartTime;
            var oldEnd = app.EndTime;

            bool slotChanged =
                oldDoctorId != dto.DoctorId ||
                oldStart != dto.StartTime ||
                oldEnd != dto.EndTime;

            bool statusChangedToCanceled =
                app.Status != AppointmentStatus.Canceled &&
                dto.Status == AppointmentStatus.Canceled;

            // 🔴 Ako se termin otkazuje → oslobodi slot
            if (statusChangedToCanceled)
            {
                var oldSlot = await _slotRepository.GetExactAsync(oldDoctorId, oldStart, oldEnd);
                if (oldSlot != null)
                {
                    oldSlot.IsBooked = false;
                    await _slotRepository.UpdateAsync(oldSlot);
                }
            }
            // 🔵 Ako se menja slot (doktor ili vreme)
            else if (slotChanged)
            {
                // novi slot mora postojati
                var newSlot = await _slotRepository.GetExactAsync(dto.DoctorId, dto.StartTime, dto.EndTime);
                if (newSlot is null)
                    throw new DomainValidationException("Selected slot does not exist for this doctor.");

                if (newSlot.IsBooked)
                    throw new DomainValidationException("Selected slot is already booked.");

                // oslobodi stari slot
                var oldSlot = await _slotRepository.GetExactAsync(oldDoctorId, oldStart, oldEnd);
                if (oldSlot != null)
                {
                    oldSlot.IsBooked = false;
                    await _slotRepository.UpdateAsync(oldSlot);
                }

                // rezerviši novi slot
                newSlot.IsBooked = true;
                await _slotRepository.UpdateAsync(newSlot);
            }

            // 🔹 update appointment podataka
            app.Status = dto.Status;
            app.PatientId = dto.PatientId;
            app.DoctorId = dto.DoctorId;
            app.Type = dto.Type;
            app.StartTime = dto.StartTime;
            app.EndTime = dto.EndTime;
            app.Notes = dto.Notes;

            return await _repository.UpdateAsync(app);
        }

        public async Task<List<ReturnAppointmentDTO>> GetAllAsync()
        {
            var appointments = await _repository.GetAllAsync();

            return appointments.Select(a => new ReturnAppointmentDTO
            {
                Id = a.Id,
                Type = a.Type,
                Status = a.Status,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                Notes = a.Notes ?? string.Empty,

                Doctor = new ReturnDoctorDTO
                {
                    Id = a.Doctor.Id,
                    FirstName = a.Doctor.FirstName,
                    LastName = a.Doctor.LastName,
                    Specialization = a.Doctor.Specialization,
                    Email = a.Doctor.Email ?? string.Empty,
                    Phone = a.Doctor.Phone ?? string.Empty
                },

                Patient = new ReturnPatientDTO
                {
                    Id = a.Patient.Id,
                    FirstName = a.Patient.FirstName,
                    LastName = a.Patient.LastName,
                    Email = a.Patient.Email,
                    Phone = a.Patient.Phone,
                    MedicalId = a.Patient.MedicalId
                }
            }).ToList();
        }


        public async Task<List<ReturnAppointmentDTO>> GetAllFilteredAsync(
    Guid? doctorId,
    Guid? patientId,
    AppointmentType? type,
    AppointmentStatus? status,
    DateTime? startFrom,
    DateTime? startTo,
    string? notesContains,
    string? sortBy,
    bool sortDesc)
        {
            // opcionalno: validacija enum-a (korisno ako stiže kao broj)
            if (type.HasValue && !Enum.IsDefined(typeof(AppointmentType), type.Value))
                throw new DomainValidationException("Invalid appointment type.");

            if (status.HasValue && !Enum.IsDefined(typeof(AppointmentStatus), status.Value))
                throw new DomainValidationException("Invalid appointment status.");

            var appointments = await _repository.GetAllFilteredAsync(
                doctorId, patientId, type, status, startFrom, startTo, notesContains, sortBy, sortDesc);

            return appointments.Select(a => new ReturnAppointmentDTO
            {
                Id = a.Id,
                Type = a.Type,
                Status = a.Status,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                Notes = a.Notes ?? string.Empty,

                Doctor = new ReturnDoctorDTO
                {
                    Id = a.Doctor.Id,
                    FirstName = a.Doctor.FirstName,
                    LastName = a.Doctor.LastName,
                    Specialization = a.Doctor.Specialization,
                    Email = a.Doctor.Email ?? string.Empty,
                    Phone = a.Doctor.Phone ?? string.Empty
                },

                Patient = new ReturnPatientDTO
                {
                    Id = a.Patient.Id,
                    FirstName = a.Patient.FirstName,
                    LastName = a.Patient.LastName,
                    Email = a.Patient.Email,
                    Phone = a.Patient.Phone,
                    MedicalId = a.Patient.MedicalId
                }
            }).ToList();
        }
        public async Task<byte[]> GetAllAppointmentsCsvAsync()
        {
            var appointments = await _repository.GetAllAsync();

            var dtoList = appointments.Select(a => new ReturnAppointmentDTO
            {
                Id = a.Id,
                Type = a.Type,
                Status = a.Status,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                Notes = a.Notes,
                Doctor = new ReturnDoctorDTO
                {
                    Id = a.Doctor.Id,
                    FirstName = a.Doctor.FirstName,
                    LastName = a.Doctor.LastName,
                    Email = a.Doctor.Email,
                    Phone = a.Doctor.Phone,
                    Specialization = a.Doctor.Specialization,
                },
                Patient = new ReturnPatientDTO
                {
                    Id = a.Patient.Id,
                    FirstName = a.Patient.FirstName,
                    LastName = a.Patient.LastName,
                    Email = a.Patient.Email,
                    Phone = a.Patient.Phone,
                    MedicalId = a.Patient.MedicalId
                }
            }).ToList();

            return _csvExporter.ExportAppointments(dtoList);
        }
    }
}

