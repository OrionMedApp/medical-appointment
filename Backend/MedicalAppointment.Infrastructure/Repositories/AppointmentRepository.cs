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
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;
        public AppointmentRepository(AppDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<Appointment> AddAsync(Appointment appointment)
        {

            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null) return false;

            _context.Appointments.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Appointment?> GetByIdAsync(Guid id)
        {
            return await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Appointment> UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .OrderByDescending(a => a.StartTime)
                .ToListAsync();
        }
    }
}
