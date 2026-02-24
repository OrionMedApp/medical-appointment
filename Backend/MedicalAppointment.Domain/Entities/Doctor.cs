using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointment.Domain.Entities
{
    public class Doctor
    {
        public Guid Id { get; private set; }

        [Required, MaxLength(120)]
        public string FirstName { get; private set; } = default!;
        [Required, MaxLength(120)]
        public string LastName { get; private set; }

        // Specialization
        [Required, MaxLength(120)]
        public Specialization Specialization { get; private set; } = default!;

        public string Email { get; set; }

        [MaxLength(40)]
        [Phone]
        public string Phone { get; set; }
        // Assigned appointments
        public List<Appointment> Appointments { get; private set; } = new();

        public List<AvailablilitySlot> AvailableSlots { get; set; } = new();
    }

    public enum Specialization
    {
        GeneralPracticioner = 0,
        Dentist =1,
        Surgeon=2
        
    }
}
