using QuizMaster.Core;
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
            try
            {
                if(string.IsNullOrEmpty(role) || string.IsNullOrWhiteSpace(role))
                {
                    throw new ObjectEmptyException("Role cannot be null or empty.");
                }

                List<Person> people = await _personRepository.GetAllStudent(role);

                return people;

            }
            catch (Exception ex)
            {
                ColloringConsole.Error(ex.Message);
                //throw new Exception("An error occurred while retrieving all persons: " + ex.Message);
                throw;
            }

        }

        public async Task RegisterPerson(Person person)
        {
            try
            {
                if (person == null)
                {
                    throw new ObjectEmptyException("This object is Empty");
                }

                Random random = new Random();

                person.VerificationCode = random.Next(1000, 9999).ToString();

               string resultPerson = await _personRepository.AddStudent(person);

                if(resultPerson.Contains("successfully"))
                {
                    ColloringConsole.Success(resultPerson);
                }
                else
                {
                    ColloringConsole.Error(resultPerson);
                }

                EmailService.SendEmail(person.Email, "Email Verification", $"Your verification code is: {person.VerificationCode}");
            }
            catch (Exception ex)
            {
                ColloringConsole.Error(ex.Message);
                //throw new Exception("An error occurred while registering the person: " + ex.Message);
                throw;
            }
        }


        public async Task<Person> GetPersonByPersonalNumber(string personalNumber, string role)
        {
            try
            {
                if(string.IsNullOrEmpty(personalNumber) || string.IsNullOrEmpty(role) || string.IsNullOrWhiteSpace(personalNumber) || string.IsNullOrWhiteSpace(role))
                {
                    throw new ObjectEmptyException("Personal number cannot be null or empty.");
                }

                Person person = _personRepository.GetPersonByPersonalNumber(personalNumber, role);

                if(person == null)
                {
                    throw new ObjectEmptyException("Person not found with the provided personal number.");
                }

                return person;

            }
            catch(Exception ex)
            {
                ColloringConsole.Error(ex.Message);
                //throw new Exception("An error occurred while retrieving the person by personal number: " + ex.Message);
                throw;
            }
            

        }

        public async Task VerifiPersonEmail(string email, string verifiCode, string role)
        {

            try
            {
                Person? person = role == "Lecturer" ? GetAllPerson(role).Result.FirstOrDefault(e => e.Email == email) as Lecturer
                                               : GetAllPerson(role).Result.FirstOrDefault(e => e.Email == email) as Student;

                if (person.VerificationCode == verifiCode)
                    person.IsVerified = true;

                string isVerifi = _personRepository.UpdateStudent(person).Result;
            }
            catch (Exception ex)
            {
                ColloringConsole.Error(ex.Message);
                //throw new Exception("An error occurred while verifying the email: " + ex.Message);
                throw;
            }


        }

        public async Task<Person> LogIn(string username, string Password, string role)
        {
            try
            {

            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(Password)
                || string.IsNullOrEmpty(role) || string.IsNullOrWhiteSpace(username)
                || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(role))
                throw new ObjectEmptyException("Username, password, and role cannot be null or empty.");

            Person person = new Person();

                if (role == "Student")
                {
                    person = await _personRepository.GetPersonByUserName(username, role) as Student;

                    if (person == null)
                        throw new ObjectEmptyException("You are not registered.");


                    if (person.IsDelete)
                        throw new ObjectEmptyException("This account has been deleted.");

                    var tt = BCrypt.Net.BCrypt.Verify(Password, person.Password);
                    if (!BCrypt.Net.BCrypt.Verify(Password, person.Password))
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
            catch (Exception ex)
            {
                ColloringConsole.Error(ex.Message);
                throw;
            }
        }


    }
}
