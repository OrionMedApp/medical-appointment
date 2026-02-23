using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointment.Domain.Entities
{
    public class Patient
    {
        public Guid Id { get; set; }

        [Required, MaxLength(120)]
        public string FirstName { get; set; } = default!;
        [Required, MaxLength(120)]
        public string LastName { get; set; }

        // Contact info
        [MaxLength(120)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(40)]
        [Phone]
        public string? Phone { get; set; }

        // Medical ID (jedinstven)
      
        public Guid MedicalId { get; set; }

        // Appointment history
        public List<Appointment> Appointments { get; set; } = new();
    }
}
