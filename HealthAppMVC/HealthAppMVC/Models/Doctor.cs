using System.ComponentModel.DataAnnotations;

namespace HealthAppMVC.Models
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
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Doctor name is required")]
        [StringLength(100, MinimumLength = 3,
    ErrorMessage = "Name must be between 3 and 100 characters")]
        [RegularExpression(@"^[A-Za-z ]+$",
    ErrorMessage = "Only alphabets and spaces are allowed")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Specialisation is required")]
        public SpecialisationType Specialisation { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(
            @"^[0-9]{10}$",
            ErrorMessage = "Phone number must contain exactly 10 digits")]
        public string DoctorPhoneNo { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(
            ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string DoctorEmail { get; set; }

        [Required(ErrorMessage = "Experience is required")]
        [Range(
            0,
            50,
            ErrorMessage = "Experience must be between 0 and 50 years")]
        public int YearsOfExperience { get; set; }

        [Required(ErrorMessage = "Consultation fee is required")]
        [Range(
            150,
            100000,
            ErrorMessage = "Consultation fee must be greater than 0")]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; } = true;
    }
}