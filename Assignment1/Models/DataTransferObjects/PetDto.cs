namespace Assignment1.Models.DataTransferObjects
{
    public class PetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string MicrochipId { get; set; }

        public VetDoctorSummaryDto VetDoctor { get; set; }

        public PetDetailsSummaryDto PetNotes { get; set; }
    }


}
