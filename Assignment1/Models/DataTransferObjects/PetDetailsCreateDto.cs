namespace Assignment1.Models.DataTransferObjects
{
    public class PetDetailsCreateDto
    {
        public int Id { get; set; }

        public string VetNotes { get; set; }

        public PetSummaryDto Pet { get; set; }

    }
}
