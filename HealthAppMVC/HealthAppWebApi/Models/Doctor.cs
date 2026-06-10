using System.ComponentModel.DataAnnotations;

namespace HealthAppWebApi.Models
{
    public enum SpecialisationType
    {
        GeneralPhysician,
        Cardiologist,
        Dermatologist,
        Neurologist,
        Orthopedic,
        Pediatrician,
        Psychiatrist,
        ENT,
        Gynecologist
    }

    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

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

        [Required]
        public SpecialisationType
            Specialisation
        { get; set; }

        [Required]
        [Range(
            0,
            50,
            ErrorMessage =
            "Experience must be between 0 and 50 years")]
        public int YearsOfExperience { get; set; }

        [Required]
        [Range(
            150,
            100000,
            ErrorMessage =
            "Consultation fee must be greater than 0")]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        [RegularExpression(
            @"^[0-9]{10}$",
            ErrorMessage =
            "Phone number must contain exactly 10 digits")]
        public string DoctorPhoneNo { get; set; }

        [Required]
        [EmailAddress(
            ErrorMessage =
            "Invalid email format")]
        [StringLength(100)]
        public string DoctorEmail { get; set; }
    }
}