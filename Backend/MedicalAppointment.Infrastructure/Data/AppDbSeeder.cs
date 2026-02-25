using Bogus;
using MedicalAppointment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointment.Infrastructure.Data
{
    public static class AppDbContextSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            var faker = new Faker("en");

            if (!context.Doctors.Any())
            {
                var doctorFaker = new Faker<Doctor>()
                    .CustomInstantiator(f => new Doctor(
                        f.Name.FirstName(),
                        f.Name.LastName(),
                        f.Internet.Email(),
                        SeedHelpers.GenerateSerbianPhone(f),
                        f.PickRandom<Specialization>()
                    ));

                context.Doctors.AddRange(doctorFaker.Generate(20));
                await context.SaveChangesAsync();
            }

            if (!context.Patients.Any())
            {
                var patientFaker = new Faker<Patient>()
                    .CustomInstantiator(f => new Patient(
                        f.Name.FirstName(),
                        f.Name.LastName(),
                        f.Internet.Email(),
                        SeedHelpers.GenerateSerbianPhone(f),
                        Guid.NewGuid()
                    ));

                context.Patients.AddRange(patientFaker.Generate(80));
                await context.SaveChangesAsync();
            }

            var doctors = context.Doctors.ToList();
            var patients = context.Patients.ToList();

            if (!context.AvailabilitySlots.Any())
            {
                var allSlots = new List<AvailablilitySlot>();

                foreach (var doctor in doctors)
                {
                    var startHour = faker.Random.Int(7, 9);
                    var workHours = faker.Random.Int(6, 9);
                    var endHour = startHour + workHours;

                    TimeSpan? breakStart = null;
                    TimeSpan? breakEnd = null;

                    if (faker.Random.Bool(0.7f))
                    {
                        var breakHour = faker.Random.Int(startHour + 2, endHour - 2);
                        breakStart = new TimeSpan(breakHour, 0, 0);
                        breakEnd = breakStart.Value.Add(TimeSpan.FromMinutes(30));
                    }

                    var slots = SeedHelpers.GenerateForDays(
                        doctor.Id,
                        DateTime.UtcNow.Date.AddDays(1),
                        7,
                        new TimeSpan(startHour, 0, 0),
                        new TimeSpan(endHour, 0, 0),
                        breakStart,
                        breakEnd
                    );

                    allSlots.AddRange(slots);
                }

                context.AvailabilitySlots.AddRange(allSlots);
                await context.SaveChangesAsync();
            }

            var allAvailableSlots = context.AvailabilitySlots.ToList();

        }
    }
}
