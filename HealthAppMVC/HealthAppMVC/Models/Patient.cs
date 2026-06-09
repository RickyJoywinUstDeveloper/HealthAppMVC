using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HealthAppMVC.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [StringLength(100)]
        [RegularExpression(@"^[A-Za-z ]+$",
    ErrorMessage = "Name can contain only letters and spaces")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public GenderType Gender { get; set; }


        [Required]
        [RegularExpression(@"^[0-9]{10}$",
            ErrorMessage = "Phone number must contain 10 digits")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50)]
        public string InsuranceId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}