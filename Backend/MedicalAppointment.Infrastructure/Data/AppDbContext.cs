using MedicalAppointment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MedicalAppointment.Infrastructure.Data
{

    public class AppDbContext : DbContext
    {

        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Patient>(e =>
            {
                e.Property(p => p.FirstName).IsRequired().HasMaxLength(120);
                e.Property(p => p.LastName).IsRequired().HasMaxLength(120);

                e.Property(p => p.MedicalId).IsRequired();
                e.HasIndex(p => p.MedicalId).IsUnique();


                e.Property(p => p.Email).HasMaxLength(120);
                e.Property(p => p.Phone).HasMaxLength(40);

                e.HasIndex(p => p.Email).IsUnique();
                e.HasIndex(p => p.Phone).IsUnique();
            });

  
            modelBuilder.Entity<Doctor>(e =>
            {
                e.Property(d => d.FirstName).IsRequired().HasMaxLength(120);
                e.Property(d => d.LastName).IsRequired().HasMaxLength(120);

                e.Property(d => d.Phone).IsRequired().HasMaxLength(40);
                e.HasIndex(d => d.Phone).IsUnique();

                e.Property(d => d.Specialization).IsRequired();
            });

   
            modelBuilder.Entity<Appointment>(e =>
            {
                e.Property(a => a.DateTime).IsRequired(); 
                e.Property(a => a.Duration).IsRequired();

                e.Property(a => a.Type).IsRequired();   
                e.Property(a => a.Status).IsRequired();

                e.Property(a => a.Notes).HasMaxLength(2000);

                e.HasOne(a => a.Patient)
                 .WithMany(p => p.Appointments)
                 .HasForeignKey(a => a.PatientId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(a => a.Doctor)
                 .WithMany(d => d.Appointments)
                 .HasForeignKey(a => a.DoctorId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasIndex(a => a.DateTime);
                e.HasIndex(a => new { a.DoctorId, a.DateTime });
                e.HasIndex(a => new { a.PatientId, a.DateTime });

                e.ToTable(t => t.HasCheckConstraint(
                    "CK_Appointments_Duration_Positive",
                    "[Duration] > 0"));

                e.ToTable(t => t.HasCheckConstraint(
                    "CK_Appointments_Date_NotPast",
                    "[DateTime] >= GETDATE()"));

            });
        }

    }
}
