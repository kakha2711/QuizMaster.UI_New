using QuizMaster.Core.Interface;
using QuizMaster.Core.Model;
using QuizMaster.Service.Exceptions;

namespace QuizMaster.Service
{
    public class StudentService
    {
        private readonly IPersonRepository _personRepository;

        public StudentService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<List<Person>> GetAllPerson(string role)
        {
            return await _personRepository.GetAllStudent(role);
        }

        public async Task RegisterPerson(Person person)
        {
            if (person == null)
            {
                throw new ObjectEmptyException("This object is Empty");
            }

            Random random = new Random();

            person.VerificationCode = random.Next(1000, 9999).ToString();

            await _personRepository.AddStudent(person);

            EmailService.SendEmail(person.Email, "Email Verification", $"Your verification code is: {person .VerificationCode}");
        }


        public async Task<Person> GetPersonByPersonalNumber(string personalNumber, string role)
        {
            return _personRepository.GetPersonByPersonalNumber(personalNumber, role);
            
        }

        public async Task VerifiPersonEmail(string email, string verifiCode, string role)
        {
            //var person = GetAllPerson(role).Result.FirstOrDefault(m => m.Email == email);

            Person? person = role == "Lecturer" ? GetAllPerson(role).Result.FirstOrDefault(e => e.Email == email) as Lecturer
                                                : GetAllPerson(role).Result.FirstOrDefault(e => e.Email == email) as Student;

            if (person.VerificationCode == verifiCode)
                person.IsVerified = true;

            string isVerifi = _personRepository.UpdateStudent(person).Result;


        }

        public async Task<Person> LogIn(string username, string Password, string role)
        {
            Person person = new Person();

            if (role == "Student")
            {
                person = await _personRepository.GetPersonByUserName(username, role) as Student;

                if (person == null)
                    throw new ObjectEmptyException("You are not registered.");
                

                if (person.IsDelete)
                    throw new ObjectEmptyException("This account has been deleted.");
                
                if(!BCrypt.Net.BCrypt.Verify(Password, person.Password))
                    throw new ObjectEmptyException("Password is incorrect.");
                

            }

            if (role == "Lecturer")
            {
                 person = await _personRepository.GetPersonByUserName(username, role) as Lecturer;

                if (person == null)
                    throw new ObjectEmptyException("You are not registered.");
               
                if (person.IsDelete)
                    throw new ObjectEmptyException("This account has been deleted.");

                var tt = BCrypt.Net.BCrypt.Verify(Password, person.Password);

                if (!BCrypt.Net.BCrypt.Verify(Password, person.Password))
                    throw new ObjectEmptyException("Password is incorrect.");
            }
            return person;
        }



    }
}
