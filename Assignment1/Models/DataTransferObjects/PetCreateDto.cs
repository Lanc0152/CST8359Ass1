namespace Assignment1.Models.DataTransferObjects
{
    public class PetCreateDto
    {
        public string Name { get; set; }

        public string MicrochipId { get; set; }

        public string Species { get; set; }

        public int VetDoctorId { get; set; }  
    }

}
