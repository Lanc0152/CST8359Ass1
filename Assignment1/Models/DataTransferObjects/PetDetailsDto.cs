using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assignment1.Models.DataTransferObjects
{
    public class PetDetailsDto
    {
        public int Id { get; set; }

        public string VetNotes { get; set; }

        public PetSummaryDto Pet { get; set; }
    }
}
