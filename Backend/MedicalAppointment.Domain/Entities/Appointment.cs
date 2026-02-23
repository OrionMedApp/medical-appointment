using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointment.Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }

        // Patient
        [Required]
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = default!;

        // Doctor
        [Required]
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; } = default!;

        // Type + Status
        public AppointmentType Type { get; set; }
        public AppointmentStatus Status { get; set; }

        // Date & time
        public DateTime DateTime { get; set; }

        public int Duration { get; set; }
        // Notes
        [MaxLength(2000)]
        public string? Notes { get; set; }


    }
    public enum AppointmentType
    {
        Consulatation=0,
        FollowUp=1,
        Emergency=2

    }
    public enum AppointmentStatus
    {
        Scheduled = 0,
        Completed = 1,
        Canceled = 2
    }
}
