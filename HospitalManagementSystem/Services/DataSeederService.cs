using Bogus;
using Bogus.DataSets;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using System.Windows.Forms;

namespace HospitalManagementSystem.Services
{
    public class DataSeederService
    {
        private static Faker faker = new("ru");
        public static void SeedDatabase()
        {
            using var context = new HospitalDbContext();

            CreatePatients(context);
        }

        private static void CreatePatients(HospitalDbContext context)
        {
            for(int i = 0; i < 100; i++)
            {
                var patient = new Patient();
                var (randomGender, fakerGender) = GetGender();
                patient.FirstName = faker.Name.FirstName(fakerGender);
                patient.LastName = faker.Name.LastName(fakerGender);
                patient.PhoneNumber = faker.Phone.PhoneNumber("+998##-###-##-##");
                patient.Birthdate = GetRandomBirthdate();
                patient.Gender = randomGender;

                context.Patients.Add(patient);
            }

            context.SaveChanges();
        }

        private static DateOnly GetRandomBirthdate()
        {
            var minDate = new DateOnly(1940, 1, 1);
            var maxDate = new DateOnly(2023, 1, 1);

            return faker.Date.BetweenDateOnly(minDate, maxDate);
        }

        private static (Gender, Name.Gender) GetGender()
        {
            var randomGender = faker.Random.Enum<Gender>();
            var fakerGender = randomGender == Gender.Male ? Name.Gender.Male : Name.Gender.Female;

            return (randomGender, fakerGender);
        }
    }
}
