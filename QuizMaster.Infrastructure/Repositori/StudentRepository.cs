using QuizMaster.Core;
using QuizMaster.Core.Enum;
using QuizMaster.Core.Interface;
using QuizMaster.Core.Model;
using QuizMaster.Service.Exceptions;
using System.Text.Json;

namespace QuizMaster.Infrastructure.Repositori
{
    public class StudentRepository : IPersonRepository
    {
        private readonly string _studentPath = "C:\\Users\\Kakha\\source\\repos\\QuizMaster.UI\\QuizMaster.Infrastructure\\Data\\Student.txt";
        private readonly string _lecturePath = "C:\\Users\\Kakha\\source\\repos\\QuizMaster.UI\\QuizMaster.Infrastructure\\Data\\Lecture.txt";



        public async Task<List<Person>> GetAllStudent(string role)
        {
            List<Person> people = new List<Person>();

            string path = role == "Student" ? _studentPath : _lecturePath;

            if (!File.Exists(path))
                return people;

            string[] lines = File.ReadAllLines(path);



            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    if (role == "Student")
                    {
                        Student? student = JsonSerializer.Deserialize<Student>(line);

                        if (student != null && !student.IsDelete)
                            people.Add(student);
                    }
                    else if (role == "Lecturer")
                    {
                        Lecturer? lecturer = JsonSerializer.Deserialize<Lecturer>(line);

                        if (lecturer != null && !lecturer.IsDelete)
                            people.Add(lecturer);
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"JSON Error: {ex.Message}");
                }
            }

            return people;
        }




        public Person GetPersonByPersonalNumber(string personalNumber, string role)
        {
            return GetAllStudent(role).Result.FirstOrDefault(m => m.PersonalNumber == personalNumber);
        }

        public  async Task<Person> GetPersonByUserName(string username, string role)
        {
            var yy = GetAllStudent(role).Result;
            var tt = yy.FirstOrDefault(m => m.UserName == username);
            return tt;
        }

        public async Task AddStudent(Person person)
        {

            if (person == null)
                throw new ObjectEmptyException("This object is empty!");

            if (person.Role.ToString() == "Student")
            {
                List<Student> students = (await GetAllStudent(person.Role.ToString())).Cast<Student>().ToList();

                person.Id = await Counter(students);

                person.Password = BCrypt.Net.BCrypt.HashPassword(person.Password);
                string studentnew = JsonSerializer.Serialize<Student>(person as Student);


                if (string.IsNullOrWhiteSpace(studentnew) || string.IsNullOrEmpty(studentnew))
                {
                    throw new InvalidDataException("Serialized student data is null or empty.");
                }


                Duplicate<Student>(students, person as Student).Wait();

                if (students.Count == 0)
                    File.AppendAllText(_studentPath, studentnew);
                else
                    File.AppendAllText(_studentPath, Environment.NewLine + studentnew);

                Student? addedStudent = (Student)GetPersonByPersonalNumber(person.PersonalNumber, person.Role.ToString());

                if (addedStudent != null)
                    ColloringConsole.Success($"Student with personal number {addedStudent.PersonalNumber} added successfully.");
                else
                    ColloringConsole.Error($"Failed to add student with personal number {person.PersonalNumber}.");

            }


            if (person.Role.ToString() == "Lecturer")
            {
                List<Lecturer> students = (await GetAllStudent(person.Role.ToString())).Cast<Lecturer>().ToList();

                person.Id = await Counter(students);

                person.Password = BCrypt.Net.BCrypt.HashPassword(person.Password);

                string studentnew = JsonSerializer.Serialize(person);


                if (string.IsNullOrWhiteSpace(studentnew) || string.IsNullOrEmpty(studentnew))
                {
                    throw new InvalidDataException("Serialized student data is null or empty.");
                }

                Duplicate<Lecturer>(students, person as Lecturer).Wait();

                if (students.Count == 0)
                    File.AppendAllText(_lecturePath, studentnew);
                else
                    File.AppendAllText(_lecturePath, Environment.NewLine + studentnew);

                Lecturer? addedStudent = (Lecturer)GetPersonByPersonalNumber(person.PersonalNumber, person.Role.ToString());

                if (addedStudent != null)
                    ColloringConsole.Success($"Lecturer with personal number {addedStudent.PersonalNumber} added successfully.");
                else
                    ColloringConsole.Error($"Failed to add Lecturer with personal number {person.PersonalNumber}.");


            }
        }


        public async Task<string> UpdateStudent(Person student)
        {
            var persons =  GetAllStudent(student.Role.ToString()).Result;

            var personId = persons.FindIndex(m => m.Id == student.Id);

            //var personId = student.Id;


                persons[personId].Id = student.Id;
                persons[personId].FirsName = student.FirsName;
                persons[personId].Lastname = student.Lastname;
                persons[personId].Email = student.Email;
                persons[personId].PhoneNumber = student.PhoneNumber;
                persons[personId].PersonalNumber = student.PersonalNumber;
                persons[personId].UserName = student.UserName;
                persons[personId].Password = student.Password;
                persons[personId].IsDelete = student.IsDelete;
                persons[personId].VerificationCode = student.VerificationCode;
                persons[personId].IsVerified = student.IsVerified;
                persons[personId].Role = student.Role;
                persons[personId].Gender = student.Gender;

                if (student.Role.ToString() == "Student")
                    (persons[personId] as Student).Grade = (student as Student).Grade;


            if (student.Role.ToString() == "Student")
                File.WriteAllText(_studentPath, string.Empty);
            else
                File.WriteAllText(_lecturePath, string.Empty);

            foreach (var item in persons)
            {
                AddStudent(item);
            }

            var personNew = persons.FirstOrDefault(m => m.PersonalNumber == student.PersonalNumber).IsVerified;

            if (personNew)
                return  $"This {student.Email} email is successfully updated.";

            return $"This {student.Email} email could not be updated.";
        }

        public Task DeleteStudent(string personalNumber)
        {
            throw new NotImplementedException();
        }

      







    
        public async Task Duplicate<T>(List<T> sourse, T item) where T : Person
        {
            if (sourse.Any(s => s.PersonalNumber == item.PersonalNumber))
            {
                throw new DuplicatePersonalNumberException($"A student with personal number {item.PersonalNumber} already exists.");
            }
        }


        public async Task<int> Counter<T>(List<T> sourse) where T : Person
        {
            int Id = 0;

            if (sourse.Count == 0)
                Id = 1;
            else
                Id = sourse.Max(l => l.Id) + 1;

            return Id;
        }


    }
}
