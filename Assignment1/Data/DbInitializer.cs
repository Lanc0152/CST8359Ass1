using System.ComponentModel;
using Assignment1.Models;

namespace Assignment1.Data
{
    public class DbInitializer
    {
        public static void Initialize(VetSystemDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.VetDoctors.Any())
            {
                return;   // DB has been seeded
            }

            // Add VetDoctors
            var vets = new VetDoctor[]
            {
                new VetDoctor { Name = "Carson", Specialty = "Dogs" },
                new VetDoctor { Name = "Alexander", Specialty = "Cats" },
                new VetDoctor { Name = "Alonso", Specialty = "Reptiles" },
                new VetDoctor { Name = "Meredith", Specialty = "Birds" },
            };
            foreach (var vet in vets)
            {
                context.VetDoctors.Add(vet);
            }
            context.SaveChanges(); 

            // Assign pets to vets using VetDoctorId
            var pets = new Pet[]
            {
                new Pet { Name = "Bandit", Species = "Dog", MicrochipId = "1", VetDoctorId = vets[0].Id },
                new Pet { Name = "Spot", Species = "Dog", MicrochipId = "2", VetDoctorId = vets[0].Id },
                new Pet { Name = "Chucky", Species = "Dog", MicrochipId = "3", VetDoctorId = vets[0].Id },
                new Pet { Name = "Scratch", Species = "Cat", MicrochipId = "4", VetDoctorId = vets[1].Id },
                new Pet { Name = "Karl", Species = "Cat", MicrochipId = "5", VetDoctorId = vets[1].Id },
                new Pet { Name = "Oreo", Species = "Cat", MicrochipId = "6", VetDoctorId = vets[1].Id },
                new Pet { Name = "Wendy", Species = "Snake", MicrochipId = "7", VetDoctorId = vets[2].Id },
                new Pet { Name = "Gizzard", Species = "Lizard", MicrochipId = "8", VetDoctorId = vets[2].Id },
                new Pet { Name = "Roger", Species = "Parrot", MicrochipId = "9", VetDoctorId = vets[3].Id },
                new Pet { Name = "Bluey", Species = "Blue Jay", MicrochipId = "10", VetDoctorId = vets[3].Id },
            };
            foreach (var pet in pets)
            {
                context.Pets.Add(pet);
            }
            context.SaveChanges();


            var petDetails = new PetDetails[]
            {
                new PetDetails { VetNotes = "Healthy Dog, Kinda silly however", PetId = pets[0].Id },
                new PetDetails { VetNotes = "Healthy-ish Dog", PetId = pets[1].Id },
                new PetDetails { VetNotes = "Healthy Dog, Barks alot", PetId = pets[2].Id },
                new PetDetails { VetNotes = "Healthy Cat, but angry", PetId = pets[3].Id },
                new PetDetails { VetNotes = "Good cat, but needs seasoning", PetId = pets[4].Id },
                new PetDetails { VetNotes = "Healthy Cat, Super sleepy", PetId = pets[5].Id },
                new PetDetails { VetNotes = "Healthy Snake, Way too long though", PetId = pets[6].Id },
                new PetDetails { VetNotes = "Healthy Lizard, This thing is cool", PetId = pets[7].Id },
                new PetDetails { VetNotes = "Healthy Parrot, Talks too much", PetId = pets[8].Id },
                new PetDetails { VetNotes = "Healthy Blue Jay, Why is this a pet???", PetId = pets[9].Id },
            };
            foreach (var detail in petDetails)
            {
                context.PetDetails.Add(detail);
            }
            context.SaveChanges();

        }
    }
}
