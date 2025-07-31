using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment1.Models
{
    public class Pet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Microchip Id")]
        public string MicrochipId { get; set; }


        [Required]
        [StringLength(50)]
        [Display(Name = "Species")]
        public string Species { get; set; }

        [ForeignKey("Vet Id")]
        public int VetDoctorId { get; set; }

        public VetDoctor VetDoctor { get; set; }

        public PetDetails PetDetails { get; set; }
    }
}
