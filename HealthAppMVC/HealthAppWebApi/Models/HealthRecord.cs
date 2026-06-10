using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAppWebApi.Models
{
    public class HealthRecord
    {
        [Key]
        public int HealthRecordId { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        

        [Required]
        public DateTime VisitDate { get; set; }

        [Required]
        [StringLength(500)]
        public string Diagnosis { get; set; }

        [Required]
        [StringLength(500)]
        public string Prescription { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }

        [NotMapped]
        public string PatientName { get; set; }

        [NotMapped]
        public string DoctorName { get; set; }

        [NotMapped]
        public string Specialisation { get; set; }

        [ForeignKey("AppointmentId")]
        public virtual Appointment
            Appointment
        { get; set; }
    }
}