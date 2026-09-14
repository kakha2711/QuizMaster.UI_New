using QuizMaster.Core;
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
            try
            {
                if (string.IsNullOrWhiteSpace(role) || string.IsNullOrEmpty(role))
                    throw new ArgumentException("Role cannot be null or empty.", nameof(role));

                List<Person> people = new List<Person>();

                string path = role == "Student" ? _studentPath : _lecturePath;

                if (!File.Exists(path))
                    throw new FileNotFoundException($"The file for role '{role}' was not found at path: {path}");

                string[] lines = File.ReadAllLines(path);

                if (lines.Length == 0 || lines.All(string.IsNullOrWhiteSpace) || lines.All(string.IsNullOrEmpty))
                    throw new ObjectEmptyException($"The file for role '{role}' is empty at path: {path}");

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;


                    if (role == "Student")
                    {
                        Student? student = JsonSerializer.Deserialize<Student>(line);

                        //if (student != null && !student.IsDelete)

                        if (student != null)
                            people.Add(student);
                    }
                    else if (role == "Lecturer")
                    {
                        Lecturer? lecturer = JsonSerializer.Deserialize<Lecturer>(line);

                        //if (lecturer != null && !lecturer.IsDelete)

                        if (lecturer != null)
                            people.Add(lecturer);
                    }

                }

                return people;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving students: {ex.Message}", ex);
            }
        }

        public Person GetPersonByPersonalNumber(string personalNumber, string role)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(personalNumber) || string.IsNullOrEmpty(personalNumber)
                    || string.IsNullOrWhiteSpace(role) || string.IsNullOrEmpty(role))
                    throw new ArgumentException("Personal number and role cannot be null or empty.", nameof(personalNumber));

                Person? person = GetAllStudent(role).Result.FirstOrDefault(m => m.PersonalNumber == personalNumber);

                return person ?? throw new FileNotFoundException("Person not found.");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the person by personal number: {ex.Message}", ex);
            }
        }

        public async Task<Person> GetPersonByUserName(string username, string role)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(username)
                    || string.IsNullOrWhiteSpace(role) || string.IsNullOrEmpty(role))
                    throw new ObjectEmptyException("Username and role cannot be null or empty.");

                List<Person> people = GetAllStudent(role).Result;

                if (people == null || !people.Any())
                    throw new ObjectEmptyException($"No people found for role '{role}'.");

                Person? person = people.FirstOrDefault(m => m.UserName == username);

            //if(person == null) throw new FileNotFoundException("This username is not found in the system!");

            return person ?? throw new ObjectEmptyException("Person not found.");
            }
            catch (Exception ex)
            {
                var tt = ex.Message;
                throw new Exception(ex.Message);
            }

        }

        public async Task AddStudent(Person person, int param = 0)
        {

            try
            {
                if (person == null)
                    throw new ObjectEmptyException("This object is empty!");

                if (person.Role.ToString() == "Student")
                {
                    List<Student> students = (await GetAllStudent(person.Role.ToString())).Cast<Student>().ToList();

                    person.Id = await Counter(students);

                    if (param == 0)
                        person.Password = BCrypt.Net.BCrypt.HashPassword(person.Password);

                    string studentnew = JsonSerializer.Serialize<Student>(person as Student);


                    if (string.IsNullOrWhiteSpace(studentnew) || string.IsNullOrEmpty(studentnew))
                    {
                        throw new InvalidDataException("Serialized student data is null or empty.");
                    }


                    if (students.Any(s => s.PersonalNumber == person.PersonalNumber))
                    {
                        throw new DuplicatePersonalNumberException($"A student with personal number {person.PersonalNumber} already exists.");
                    }


                    //Duplicate<Student>(students, person as Student);

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

                    if (param == 0)
                        person.Password = BCrypt.Net.BCrypt.HashPassword(person.Password);

                    string studentnew = JsonSerializer.Serialize(person);


                    if (string.IsNullOrWhiteSpace(studentnew) || string.IsNullOrEmpty(studentnew))
                    {
                        throw new InvalidDataException("Serialized student data is null or empty.");
                    }

                    if (students.Any(s => s.PersonalNumber == person.PersonalNumber))
                    {
                        throw new DuplicatePersonalNumberException($"A student with personal number {person.PersonalNumber} already exists.");
                    }

                    //Duplicate(students, person as Lecturer);

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
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding the student: {ex.Message}", ex);

            }
        }


        public async Task<string> UpdateStudent(Person student)
        {
            try
            {
                var persons = GetAllStudent(student.Role.ToString()).Result;

                var personId = persons.FindIndex(m => m.Id == student.Id);

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
                    AddStudent(item, 1);

                bool personNew = persons.FirstOrDefault(m => m.PersonalNumber == student.PersonalNumber).IsVerified;

                if (personNew)
                    return $"This {student.Email} email is successfully updated.";

                return $"This {student.Email} email could not be updated.";
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the student: {ex.Message}", ex);
            }
        }

        public async Task<string> DeleteStudent(string personalNumber)
        {
           try
            {
                if (personalNumber == null)
                    throw new ObjectEmptyException("Personal number is empty!");

                List<Person> persons = GetAllStudent("Student").Result;

                if (persons == null)
                    throw new ObjectEmptyException("The object is empty.");

                if (!persons.Any(p => p.PersonalNumber == personalNumber))
                    throw new ObjectEmptyException($"No student found with personal number {personalNumber}.");

                int personToDeleteIndex = persons.FindIndex(p => p.PersonalNumber == personalNumber);

                if (personToDeleteIndex == -1)
                    throw new ObjectEmptyException($"No student found with personal number {personalNumber}.");

                persons[personToDeleteIndex].IsDelete = true;

                string result = "";

                foreach (var item in persons)
                {
                    result = await UpdateStudent(item);
                }

                if (result.Contains("successfully"))
                    result = "Student deleted successfully";
                else
                    result = "Failed to delete student";

                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the student: {ex.Message}", ex);
            }
        }










        //public async Task Duplicate(List<Person> sourse, Person item)
        //{
        //    if (sourse.Any(s => s.PersonalNumber == item.PersonalNumber))
        //    {
        //        throw new DuplicatePersonalNumberException($"A student with personal number {item.PersonalNumber} already exists.");
        //    }
        //}


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
