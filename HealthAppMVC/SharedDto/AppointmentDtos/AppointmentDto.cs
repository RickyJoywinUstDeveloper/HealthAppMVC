using System;

namespace SharedDto.AppointmentDtos
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public string PatientName { get; set; }

        public string DoctorName { get; set; }

        public DateTime ScheduledDate { get; set; }

        public string TimeSlot { get; set; }

        public string Status { get; set; }

        public string CancellationReason { get; set; }
    }
}