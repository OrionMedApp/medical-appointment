using MedicalAppointment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointment.Domain.IRepositories
{
    public interface IAvailabilitySlotRepository
    {
        Task<List<AvailablilitySlot>> GetByDoctorAndDateAsync(Guid? doctorId, DateTime? date);
        Task<AvailablilitySlot?> GetExactAsync(Guid doctorId, DateTime start, DateTime end);
        Task UpdateAsync(AvailablilitySlot slot);

    }
}
