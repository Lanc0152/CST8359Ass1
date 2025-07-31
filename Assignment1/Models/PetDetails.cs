using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment1.Models
{
    public class PetDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Vet Notes")]
        public string VetNotes { get; set; }
        
        [ForeignKey("Pet Id")]
        public int PetId { get; set; }

        public Pet Pet { get; set; }

    }
}
