

namespace SharedDto.HealthRecordDtos
{
    public class CreateHealthRecordDto
    {
        
        public int AppointmentId { get; set; }

       
        public string Diagnosis { get; set; }

       
        public string Prescription { get; set; }

        public string Notes { get; set; }
    }
}