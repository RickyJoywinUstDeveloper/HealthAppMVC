using System;
using System.ComponentModel.DataAnnotations;

namespace SharedDto.PatientDtos
{
    public class CreatePatientDto
    {
        [Required]
        [StringLength(100)]
        [RegularExpression(
           @"^[A-Za-z ]+$",
           ErrorMessage =
           "Name can contain only letters and spaces")]
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [EmailAddress(
            ErrorMessage =
            "Invalid email format")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [RegularExpression(
          @"^[0-9]{10}$",
          ErrorMessage =
          "Phone number must contain exactly 10 digits")]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string InsuranceId { get; set; }
    }
}