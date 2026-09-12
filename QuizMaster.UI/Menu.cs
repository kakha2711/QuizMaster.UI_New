
using QuizMaster.Core;
using QuizMaster.Core.Enum;
using QuizMaster.Core.Model;
using QuizMaster.Service;

namespace QuizMaster.UI
{
    internal class Menu
    {
        private readonly StudentService _studentService;
        private readonly QuestionTestRepositoryService _questionTestRepositoryService;

        public Menu(StudentService studentService, QuestionTestRepositoryService questionTestRepositoryService)
        {
            _studentService = studentService;
            _questionTestRepositoryService = questionTestRepositoryService;
        }

        public async Task Show()
        {
            Console.WriteLine("elected role");
            Console.WriteLine("1: Lecturer");
            Console.WriteLine("2: Student");

            string lecturerStudent = Console.ReadLine();
            string personRole = string.Empty;

            switch (lecturerStudent)
            {
                case "1":
                    personRole = "Lecturer";
                    break;
                case "2":
                    personRole = "Student";
                    break;
            }


            Console.WriteLine("Please log in to your account.");

            Console.Write("Enter Username: ");
            string userName = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            var person = await _studentService.LogIn(userName, password, personRole);

            if (person == null)
            {
                ColloringConsole.Error("You are not registered.");
                return;
            }

            if (!person.IsVerified)
            {
                ColloringConsole.Error("Your email is not verified.\n Please verify.");
                return;
            }


            Console.WriteLine("1. Register as a new Person");
            Console.WriteLine("2. View all students");
            Console.WriteLine("3. View Person from personalnumber");
            Console.WriteLine("4. Delete Person from personalnumber"); //dasaceria
            Console.WriteLine("5. Verifi Person email");
            Console.WriteLine("6. LogIn Person");//dasaceria
            Console.WriteLine("7. Exit");

            string choice = Console.ReadLine();

            Person student;

            if (personRole == "Lecturer")
                student = new Lecturer();
            else
                student = new Student();



                switch (choice)
                {
                    case "1":

                        Console.Write("Enter FirsName: ");
                        student.FirsName = Console.ReadLine();

                        Console.Write("Enter Lastname: ");
                        student.Lastname = Console.ReadLine();

                        Console.Write("Enter Email: ");
                        student.Email = Console.ReadLine();

                        Console.Write("Enter PhoneNumber: ");
                        student.PhoneNumber = Console.ReadLine();

                        Console.Write("Enter PersonalNumber: ");
                        student.PersonalNumber = Console.ReadLine();

                        Console.Write("Enter UserName: ");
                        student.UserName = Console.ReadLine();

                        Console.Write("Enter Password: ");
                        student.Password = Console.ReadLine();

                        Console.Write("Enter Gender: ");
                        student.Gender = Enum.Parse<Gender>(Console.ReadLine(), true);

                        student.Role = (Role)Enum.Parse(typeof(Role), personRole, true);

                        await _studentService.RegisterPerson(student);

                        break;

                    case "2":
                        List<Person> students ;
                        if (personRole == "Lecturer")
                            students = await _studentService.GetAllPerson(personRole);
                        else
                            students = await _studentService.GetAllPerson(personRole);

                        foreach (var item in students)
                        {
                            //Console.WriteLine($"Id: {item.Id},\n FirsName: {item.FirsName},\n Lastname: {item.Lastname},\n Email: {item.Email},\n PhoneNumber: {item.PhoneNumber},\n PersonalNumber: {item.PersonalNumber},\n Password: {item.Password},\n VerificationCode: {item.VerificationCode},\n IsVerified: {item.IsVerified},\n Role: {item.Role},\n Gender: {item.Gender},\n Grade: {item.Grade}\n\n");
                            Console.WriteLine(item.ToString());
                        }

                        break;

                    case "3":

                    Console.WriteLine("Enter PersonalNumber");
                    string? findFromPersonalNumber = Console.ReadLine();

                    Person studentPeronalNumber = await _studentService.GetPersonByPersonalNumber(findFromPersonalNumber, personRole);

                    Console.WriteLine(studentPeronalNumber.ToString());
                    break;

                    case "4":

                        //Console.WriteLine("Enter PersonalNumber");
                        //string? deleteFromPersonalNumber = Console.ReadLine();

                        //await _studentService.DeleteStudentByPersonalNumber(deleteFromPersonalNumber);
                        break;

                    case "5":

                    Console.WriteLine($"Enter {personRole} email");
                    string? studentEmail = Console.ReadLine();

                    Console.WriteLine($"Enter {personRole} VerificationCode");
                    string? StudentVerificationCode = Console.ReadLine();

                    await _studentService.VerifiPersonEmail(studentEmail, StudentVerificationCode, personRole);

                        break;

                    case "6":

                        //Console.WriteLine("Enter StudentUsername");
                        //string? studentUserName = Console.ReadLine();

                        //Console.WriteLine("Enter StudentPassword");
                        //string? studentPassword = Console.ReadLine();

                        //var studentLogIn = await _studentService.LogIn<Person>(studentUserName, studentPassword, personRole);

                        //Console.WriteLine(studentLogIn.ToString());

                        break;

                    case "7":
                        //isvalid = false;
                        break;
                }
            
        }
    }
}
