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
            CreateDoctorSpecializations(context);
        }

        private static void CreatePatients(HospitalDbContext context)
        {
            if (context.Patients.Any()) return;

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
            if (context.Doctors.Any()) return;

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
            if (context.Specializations.Any()) return;

            var values = Enum.GetNames(typeof(DoctorSpecializationType));

            foreach (var value in values)
            {
                var specialization = new Specialization
                {
                    Name = value,
                    Description = faker.Lorem.Text()
                };

                context.Specializations.Add(specialization);
            }

            context.SaveChanges();
        }
        
        private static void CreateDoctorSpecializations(HospitalDbContext context)
        {
            if (context.DoctorSpecializations.Any()) return;

            var doctorIds = context.Doctors.Select(x => x.Id).ToArray(); // [1, 2, 3]
            var specializatoinIds = context.Specializations.Select(x => x.Id).ToArray(); // [5, 9, 15]

            foreach(var doctorId in doctorIds) // 1
            {
                var specializationsCount = faker.Random.Number(1, 3); // 2
                HashSet<int> specializations = new(); // [9, 5]

                for(int i = 0; i < specializationsCount; i++) // i = 0, i = 1
                {
                    var randomSpecializationId = faker.PickRandom(specializatoinIds); // 9, 5
                    specializations.Add(randomSpecializationId); // 5
                }

                foreach(var specializationId in specializations) // [9, 5]
                {
                    var doctorSpecialization = new DoctorSpecialization
                    {
                        DoctorId = doctorId, // 1
                        SpecializationId = specializationId // 5
                    };
                    context.DoctorSpecializations.Add(doctorSpecialization); // 1 - 5
                }
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
