using System;
using System.ComponentModel.DataAnnotations;


namespace SharedDto.AppointmentDtos
{
    public class CreateAppointmentDto
    {

        [Required(ErrorMessage =
                    "Patient is required")]

        public int PatientId { get; set; }


        [Required(ErrorMessage =
                   "Doctor is required")]

        public int DoctorId { get; set; }


        [Required(ErrorMessage =
                    "Appointment date is required")]

        public DateTime ScheduledDate { get; set; }


        [Required(ErrorMessage =
                   "Time slot is required")]

        public string TimeSlot { get; set; }
    }
}