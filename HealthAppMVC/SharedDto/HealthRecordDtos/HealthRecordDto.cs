using System;

namespace SharedDto.HealthRecordDtos
{
    public class HealthRecordDto
    {
        public int HealthRecordId { get; set; }

        public DateTime VisitDate { get; set; }

        public string PatientName { get; set; }

        public string DoctorName { get; set; }

        public string Diagnosis { get; set; }

        public string Prescription { get; set; }

        public string Notes { get; set; }
    }
}