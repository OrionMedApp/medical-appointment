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
    public class DoctorRepository:IDoctorRepository
    {
        private readonly AppDbContext _context;

        public DoctorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Doctor?> GetByIdAsync(Guid id)
        {
            return await _context.Doctors
                .Include(p => p.Appointments)
                .Include(p=>p.AvailableSlots)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Doctor>> GetAllAsync()
        {
            return await _context.Doctors.ToListAsync();
        }
    }
}
