
using QuizMaster.Core;
using QuizMaster.Core.Enum;
using QuizMaster.Core.Model;
using QuizMaster.Service;
using Spectre.Console;

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
            try
            {

                //await _questionTestRepositoryService.GetLiderBoard();
                string personRole = Role();

                RegisterRole(personRole, _questionTestRepositoryService, _studentService);
            }
            catch (Exception ex)
            {
                ColloringConsole.Error(ex.Message);
            }



            //log-ირება დავამატო

        }



        static string Role()
        {

            var personRole = AnsiConsole.Prompt(
                             new SelectionPrompt<string>()
                            .Title("Selected role:")
                            .AddChoices("Lecturer", "Student"));

            AnsiConsole.MarkupLine($"You selected: [green]{personRole}[/]");

            return personRole;
        }

        static async Task RegisterRole(string personRole, QuestionTestRepositoryService _questionTestRepositoryService, StudentService _studentService)
        {


            var registerLogIn = AnsiConsole.Prompt(
                             new SelectionPrompt<string>()
                            .Title("Selected register or log in:")
                            .AddChoices("register", "log in"));

            AnsiConsole.MarkupLine($"You selected: [green]{registerLogIn}[/]");


            Person person = new Person();

            switch (registerLogIn)
            {
                case "register":

                    Console.Write("Enter FirsName: ");
                    person.FirsName = Console.ReadLine();

                    Console.Write("Enter Lastname: ");
                    person.Lastname = Console.ReadLine();

                    Console.Write("Enter Email: ");
                    person.Email = Console.ReadLine();

                    Console.Write("Enter PhoneNumber: ");
                    person.PhoneNumber = Console.ReadLine();

                    Console.Write("Enter PersonalNumber: ");
                    person.PersonalNumber = Console.ReadLine();

                    Console.Write("Enter UserName: ");
                    person.UserName = Console.ReadLine();

                    Console.Write("Enter Password: ");
                    person.Password = Console.ReadLine();

                    Console.Write("Enter Gender: ");
                    person.Gender = Enum.Parse<Gender>(Console.ReadLine(), true);




                    person.Role = (Role)Enum.Parse(typeof(Role), personRole, true);

                    await _studentService.RegisterPerson(person);

                    break;

                case "log in":

                    Console.Write("Enter Username: ");
                    string? userName = Console.ReadLine();

                    Console.Write("Enter password: ");
                    string? password = Console.ReadLine();

                    if (string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userName) || string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
                    {
                        ColloringConsole.Error("Username and password cannot be empty.");
                        return;
                    }

                    person = await _studentService.LogIn(userName, password, personRole);

                    if (!person.IsVerified)
                    {
                        ColloringConsole.Error("Your account is unverified.");

                        VerifiMail(personRole, _studentService);
                        return;
                    }

                    if (person.Role.ToString() == "Lecturer")
                    {
                        await LecturersEnvironment(personRole, _questionTestRepositoryService, _studentService, person as Lecturer);
                    }

                    if (person.Role.ToString() == "Student")
                    {
                        await StudentEnvironment(personRole, _questionTestRepositoryService, _studentService, person as Student);
                    }

                    break;
            }

            static async void VerifiMail(string personRole, StudentService _studentService)
            {
                Console.WriteLine($"Enter {personRole} email");
                string? studentEmail = Console.ReadLine();

                Console.WriteLine($"Enter {personRole} VerificationCode");
                string? StudentVerificationCode = Console.ReadLine();

                await _studentService.VerifiPersonEmail(studentEmail, StudentVerificationCode, personRole);

            }
        }

        static async Task LecturersEnvironment(string role, QuestionTestRepositoryService _questionTestRepositoryService, StudentService _studentService, Lecturer lecturer)
        {
            Console.WriteLine("1. View all students");
            Console.WriteLine("2. Create a new test");
            Console.WriteLine("3. View all tests");
            Console.WriteLine("4. Edit a test");
            Console.WriteLine("5. Delete a test");

            Console.WriteLine("6. Create a new question");
            Console.WriteLine("7. View all questions");
            Console.WriteLine("8. Edit a question");
            Console.WriteLine("9. Delete a question");

            Console.WriteLine("10. Update lecturer");
            Console.WriteLine("11. Delete lecturer");

            Console.WriteLine("Write the appropriate number.");
            string? input = Console.ReadLine();

            TestCatalog questionTest = new TestCatalog();

            switch (input)
            {
                case "1":
                    var students = await _studentService.GetAllPerson("Student");

                    foreach (var item in students)
                    {
                        Console.WriteLine(item.ToString());
                    }

                    break;
                case "2":

                    Console.WriteLine("Enter TestTitle");
                    string? testTitle = Console.ReadLine();
                    questionTest.TestTitle = testTitle;

                    Console.WriteLine("Enter Topic");
                    string? topic = Console.ReadLine();
                    questionTest.Topic = topic;

                    Console.WriteLine("Enter QuestionsNumber");
                    string? questionsNumber = Console.ReadLine();
                    questionTest.QuestionsNumber = double.Parse(questionsNumber);

                    Console.WriteLine("Enter MaximumScore");
                    string? maximumScore = Console.ReadLine();
                    questionTest.MaximumScore = double.Parse(maximumScore);

                    Console.WriteLine("Enter DateTime");
                    string? dateTime = Console.ReadLine();
                    questionTest.DateTime = byte.Parse(dateTime);

                    Console.WriteLine("Enter PassingPercentage");
                    string? passingPercentage = Console.ReadLine();
                    questionTest.PassingPercentage = byte.Parse(passingPercentage);

                    await _questionTestRepositoryService.AddTestCatalog(questionTest);

                    break;
                case "3":

                    List<TestCatalog> testCatalogs = await _questionTestRepositoryService.GetAllTestsCatalog();

                    foreach (var item in testCatalogs)
                    {
                        Console.WriteLine(item.ToString());
                    }

                    break;
                case "4":

                    break;
                case "5":
                    break;
                case "6":

                    QuestionTest question = new QuestionTest();

                    AnswerTest[] answerTests = new AnswerTest[4];

                    List<TestCatalog> testCatalogs1 = await _questionTestRepositoryService.GetAllTestsCatalog();

                    int num = 0;
                    int testQuestioncount = 0;

                    while (num <= testQuestioncount - 1)
                    {

                        Console.Write($"Enter {num + 1} Question: ");
                        question.QuestionText = Console.ReadLine();

                        Console.Write("Enter ChoiceQuestion: ");

                        var fruit = AnsiConsole.Prompt(
                             new SelectionPrompt<string>()
                            .Title("Enter ChoiceQuestion:")
                            .AddChoices("SingleChoiceQuestion", "MultiChoiceQuestion"));

                        AnsiConsole.MarkupLine($"You selected: [green]{fruit}[/]");

                        question.ChoiceQuestion = Enum.Parse<ChoiceQuestion>(fruit, true);

                        Console.WriteLine("Choose which test to add the question to.");

                        var selectedQuestion = AnsiConsole.Prompt(
                               new SelectionPrompt<TestCatalog>()
                                   .Title("Select a [green]book[/]")
                                   .PageSize(10)
                                   .UseConverter(Test => $"{Test.Id} by {Test.TestTitle}")
                                   .AddChoices(testCatalogs1));

                        AnsiConsole.MarkupLine($"You selected: [yellow]{selectedQuestion.TestTitle}[/]");

                        int testQuestionId = Convert.ToInt32(selectedQuestion.Id);

                        question.TestCatalogId = testQuestionId;

                        testQuestioncount = Convert.ToInt32(selectedQuestion.QuestionsNumber);

                        for (int i = 0; i < 4; i++)
                        {
                            answerTests[i] = new AnswerTest();

                            Console.Write($"Enter {i + 1} Answer: ");
                            answerTests[i].Answer = Console.ReadLine();

                            Console.Write("Enter IsCorrect: ");

                            string? answer = AnsiConsole.Prompt(
                                 new SelectionPrompt<string>()
                                .Title("Enter IsCorrect:")
                                .AddChoices("True", "False"));

                            AnsiConsole.MarkupLine($"You selected: [green]{answer}[/]");

                            bool isvalis = bool.TryParse(answer, out bool result);

                            answerTests[i].IsCorrect = result;

                        }

                        _questionTestRepositoryService.AddQuestionTest(question, answerTests.ToArray());
                        num++;

                    }

                    break;
                case "7":
                    break;
                case "8":
                    break;
                case "9":
                    break;
                case "10":
                    break;
                case "11":
                    break;
            }

        }


        static async Task StudentEnvironment(string role, QuestionTestRepositoryService _questionTestRepositoryService, StudentService _studentService, Student student)
        {
            Console.WriteLine("Welcome to the Student Environment!");
            Console.WriteLine("Please select an option:");



            string? input = AnsiConsole.Prompt(
                                 new SelectionPrompt<string>()
                                .Title("Enter IsCorrect:")
                                .AddChoices("View all tests", "Start a test"));

            AnsiConsole.MarkupLine($"You selected: [green]{input}[/]");

            List<TestCatalog> testCatalogs = await _questionTestRepositoryService.GetAllTestsCatalog();

            List<QuestionTest> questionTests = new List<QuestionTest>();

            List<AnswerTest> answerTests = new List<AnswerTest>();



            switch (input)
            {
                case "View all tests":

                    foreach (var item in testCatalogs)
                    {
                        Console.WriteLine($"{item.Id}: {item.TestTitle}: {item.MaximumScore}");
                    }

                    break;
                case "Start a test":

                    var selectedQuestionId = AnsiConsole.Prompt(
                               new SelectionPrompt<TestCatalog>()
                                   .Title("Select a [green]book[/]")
                                   .PageSize(10)
                                   .UseConverter(Test => $"{Test.Id} by {Test.TestTitle}")
                                   .AddChoices(testCatalogs)).Id;

                    AnsiConsole.MarkupLine($"You selected: [yellow]{selectedQuestionId}[/]");

                    List<QuestionTest> questions = await _questionTestRepositoryService.GetQuestionQuizi(selectedQuestionId);

                    int indexQuestion = 0;
                    //int indexAnswer = 0;

                    int[] questionTestArray = new int[questions.Count];
                    List<int> answerTestArray = new List<int>();
                    List<int> isCorectAnswer = new List<int>();

                    foreach (var item in questions)
                    {
                        questionTests.Add(item);

                        List<AnswerTest> answers = await _questionTestRepositoryService.GetAnswerQuizi(item.Id);
                        answerTests.AddRange(answers);

                        questionTestArray[indexQuestion] = item.Id;
                        indexQuestion++;
                    }

                    indexQuestion = 0;
                    int selectedAnswerId = 0;

                    foreach (var item in questionTests)
                    {
                        Console.WriteLine($"{item.Id}: {item.QuestionText}");

                        var answersForQuestion = answerTests.Where(a => a.QuestionTestId == item.Id).ToList();


                        List<string> selected = AnsiConsole.Prompt(
                            new MultiSelectionPrompt<string>()
                            .Title("Select [green]notification plugins[/] to install:")
                            .AddChoices(answersForQuestion.Select(a => $"{a.Id}: {a.Answer}")));

                        AnsiConsole.MarkupLine($"[blue]Installing {selected.Count} plugin(s)...[/]");

                        
                        foreach (var item1 in answersForQuestion)
                        {
                            if (item1.IsCorrect)
                                isCorectAnswer.Add(item1.Id);
                        }

                        //if (selected.Count == 4)
                        //{
                        foreach (var item1 in selected)
                        {
                            answerTestArray.Add(Convert.ToInt32(item1.Split(':')[0]));
                        }

                        _questionTestRepositoryService
                                .AddQuestionAnswer(student.Id, selectedQuestionId, item.Id, answerTestArray.ToArray(), isCorectAnswer.ToArray());

                        answerTestArray.Clear();
                        isCorectAnswer.Clear();

                    }

                    //ეს არის გასაგრძელებელი არ არის დამთავრებული

                    break;
            }
        }
    }
}
