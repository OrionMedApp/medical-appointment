using MedicalAppointment.Application.IServices;
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
        public AvailabilityService(IAvailabilitySlotRepository repository)
        {
            _repository = repository;
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
    }
}
