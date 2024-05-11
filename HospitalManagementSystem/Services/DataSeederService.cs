using Bogus;
using Bogus.DataSets;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Services
{
    public class DataSeederService
    {
        private static Faker faker = new();
        public static void SeedDatabase()
        {
            using var context = new HospitalDbContext();

            CreatePatients(context);
            CreateDoctors(context);
            CreateSpecializations(context);
        }

        private static void CreatePatients(HospitalDbContext context)
        {
            for (int i = 0; i < 100; i++)
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
        private static void CreateDoctors(HospitalDbContext context)
        {
            for (int i = 0; i < 20; i++)
            {
                var doctor = new Doctor();
                var (gender, fakerGender) = GetGender();
                doctor.FirstName = faker.Name.FirstName(fakerGender);
                doctor.LastName = faker.Name.LastName(fakerGender);
                doctor.PhoneNumber = faker.Phone.PhoneNumber("+998##-###-##-##");

                context.Doctors.Add(doctor);
            }

            context.SaveChanges();
        }
        private static void CreateSpecializations(HospitalDbContext context)
        {
            for (int i = 0; i < 20; i++)
            {
                var specialization = new Specialization();
                var spec = faker.Random.Enum<DoctorSpecializationEnum>();
                specialization.Name = spec.ToString();

                context.Specializations.Add(specialization);
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
