namespace Assignment1.Models.DataTransferObjects
{
    public class VetDoctorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialty { get; set; }

        public ICollection<PetSummaryDto> Pets { get; set; }

    }
}
