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
    public class AvailabilityService: IAvailabilityService
    {
        private readonly IAvailabilitySlotRepository _repository;
        private readonly IAppointmentRepository _appointments;
        public AvailabilityService(IAvailabilitySlotRepository repository, IAppointmentRepository appointments)
        {
            _repository = repository;
            _appointments = appointments;
        }
        public async Task BookSlots(Guid doctorId, DateTime start, DateTime end)
        {
            var slots = await _repository.GetSlotsInRangeAsync(doctorId, start, end);

            if (!slots.Any())
                throw new DomainValidationException("No availability slots found.");

            if (slots.Any(x => x.IsBooked))
                throw new DomainValidationException("One or more slots already booked.");

            foreach (var slot in slots)
            {
                slot.MarkAsBooked();
            }

            await _repository.SaveChangesAsync();
        }



        public async Task ReleaseSlots(Guid doctorId, DateTime start, DateTime end)
        {
            var slots = await _repository.GetSlotsInRangeAsync(doctorId, start, end);

            if (slots == null || slots.Count == 0)
                return; 

            foreach (var slot in slots)
                slot.Release();

            await _repository.SaveChangesAsync();
        }

        public async Task<(DateTime start, DateTime end)> FindFirstAvailableSlotForDoctorAsync(
                Guid doctorId,
                Guid patientId,
                DateTime fromUtc,
                int daysAhead,
                int durationMinutes)
        {
            // prolazimo dan po dan, i uzimamo slotove za tog doktora
            for (int i = 0; i < daysAhead; i++)
            {
                var date = fromUtc.Date.AddDays(i);

                var daySlots = await _repository.GetByDoctorAndDateAsync(doctorId, date);

                // uzmi samo slobodne slotove i one koje su posle "sad"
                var free = daySlots
                    .Where(s => !s.IsBooked && s.StartTime >= fromUtc)
                    .OrderBy(s => s.StartTime)
                    .ToList();

                // tražimo "kontinuiran" blok slotova koji pokriva durationMinutes
                var candidate = FindContinuousRange(free, durationMinutes);
                if (candidate == null)
                    continue;

                var start = candidate.Value.start;
                var end = candidate.Value.end;

                // no double-booking za pacijenta (i doktora ako želiš duplu proveru)
                var overlap = await _appointments.HasOverlapAsync(doctorId, patientId, start, end, excludeAppointmentId: null);
                if (!overlap)
                    return (start, end);
            }

            throw new InvalidOperationException("No available slot found in next days.");
        }

        private static (DateTime start, DateTime end)? FindContinuousRange(List<AvailablilitySlot> slots, int durationMinutes)
        {
            if (slots.Count == 0) return null;

            // Pretpostavka: slotovi su npr 30min, ali ne mora. Radimo generalno.
            for (int i = 0; i < slots.Count; i++)
            {
                var start = slots[i].StartTime;
                var end = slots[i].EndTime;

                // širi dok god su slotovi "nalepljeni" jedan na drugi
                int j = i;
                while (j + 1 < slots.Count && slots[j + 1].StartTime == end)
                {
                    end = slots[j + 1].EndTime;
                    j++;
                }

                var totalMinutes = (int)(end - start).TotalMinutes;
                if (totalMinutes >= durationMinutes)
                {
                    // uzmi baš tačnu dužinu (npr 30min) od start-a
                    return (start, start.AddMinutes(durationMinutes));
                }

                i = j; // preskoči već obrađene slotove
            }

            return null;
        }
    }
}
