using MedicalAppointment.Domain.Entities;
using MedicalAppointment.Domain.IRepositories;
using MedicalAppointment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointment.Infrastructure.Repositories
{
    public class AvailabilitySlotRepository : IAvailabilitySlotRepository
    {
        private readonly AppDbContext _context;

        public AvailabilitySlotRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AvailablilitySlot>> GetByDoctorAndDateAsync(Guid? doctorId, DateTime ? date)
        {
            var query = _context.AvailabilitySlots.AsQueryable();

            if (doctorId.HasValue)
            {
                query = query.Where(s => s.DoctorId == doctorId.Value);
            }

            if (date.HasValue)
            {
                var dayStart = date.Value.Date;
                var dayEnd = dayStart.AddDays(1);

                query = query.Where(s =>
                    s.StartTime >= dayStart &&
                    s.StartTime < dayEnd);
            }

            return await query.ToListAsync();
        }
    }
}
