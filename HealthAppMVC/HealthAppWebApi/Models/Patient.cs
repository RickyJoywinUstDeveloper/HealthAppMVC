using System;
using System.ComponentModel.DataAnnotations;

namespace HealthAppWebApi.Models
{
    public enum GenderType
    {
        Male,
        Female,
        Other
    }

    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

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
        public GenderType Gender { get; set; }

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

        public DateTime CreatedDate { get; set; }
    }
}