using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAppWebApi.Models
{
    public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }

    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ScheduledDate { get; set; }

        [Required(
            ErrorMessage =
            "Please select a time slot")]
        public string TimeSlot { get; set; }

        public AppointmentStatus
            Status
        { get; set; }

        public string
            CancellationReason
        { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient
            Patient
        { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor
            Doctor
        { get; set; }

        public virtual ICollection
            <HealthRecord>
            HealthRecords
        { get; set; }
    }
}
