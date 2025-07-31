using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Assignment1.Models
{
    public class VetDoctor
    {
        [Key]
        public int Id {  get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name="Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Specialty")]
        public string Specialty { get; set; }

        public ICollection<Pet> Pets { get; set; }
    }
}
