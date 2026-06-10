using System;


namespace SharedDto.AppointmentDtos
{
    public class CreateAppointmentDto
    {
       
        public int PatientId { get; set; }

       
        public int DoctorId { get; set; }

      
        public DateTime ScheduledDate { get; set; }

      
        public string TimeSlot { get; set; }
    }
}