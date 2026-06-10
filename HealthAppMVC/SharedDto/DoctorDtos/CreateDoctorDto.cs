
using System.ComponentModel.DataAnnotations;
namespace SharedDto.DoctorDtos
{
    public class CreateDoctorDto
    {

        [Required(
              ErrorMessage =
              "Doctor name is required")]
        [StringLength(
              100,
              MinimumLength = 3)]
        [RegularExpression(
              @"^[A-Za-z ]+$",
              ErrorMessage =
              "Only alphabets and spaces are allowed")]

        public string FullName { get; set; }


        [Required(ErrorMessage =
                    "Specialisation is required")]

        public string Specialisation { get; set; }


        [Required(ErrorMessage =
                   "Years of experience is required")]

        public int YearsOfExperience { get; set; }


        [Required(ErrorMessage =
                    "Consultation fee is required")]

        [Range(1, 10000,
            ErrorMessage =
            "Invalid consultation fee")]

        public decimal ConsultationFee { get; set; }


        [EmailAddress(ErrorMessage =
                    "Invalid email address")]

        public string DoctorEmail { get; set; }


        [Required(ErrorMessage =
                    "Phone number is required")]

        [RegularExpression(
                    @"^[0-9]{10}$",
                    ErrorMessage =
                    "Phone number must be 10 digits")]

        public string DoctorPhoneNo { get; set; }
    }
}