
using QuizMaster.Core.Interface;
using QuizMaster.Core.Model;
using QuizMaster.Service.Exceptions;
using System.Text.Json;

namespace QuizMaster.Infrastructure.Repositori
{
    public class QuestionTestRepository : IQuestionTestRepository
    {
        private readonly string questionTestPath = "C:\\Users\\Kakha\\source\\repos\\QuizMaster.UI\\QuizMaster.Infrastructure\\Data\\QuestionTest.txt";
        private readonly string testCatalogPath = "C:\\Users\\Kakha\\source\\repos\\QuizMaster.UI\\QuizMaster.Infrastructure\\Data\\TestCatalog.txt";
        private readonly string answerPath = "C:\\Users\\Kakha\\source\\repos\\QuizMaster.UI\\QuizMaster.Infrastructure\\Data\\Answer.txt";
        private readonly string testResult = "C:\\Users\\Kakha\\source\\repos\\QuizMaster.UI\\QuizMaster.Infrastructure\\Data\\StudentsTestResultFile.txt";


        public Task<List<TestCatalog>> GetAllTestsCatalog()
        {
            List<TestCatalog> testCatalogs = new List<TestCatalog>();

            string[] lines = File.ReadAllLines(testCatalogPath);

            foreach (string line in lines)
            {
                TestCatalog? testCatalog = JsonSerializer.Deserialize<TestCatalog>(line);

                if(testCatalog != null && !testCatalog.IsDelete)
                    testCatalogs.Add(testCatalog);
            }

            return Task.FromResult(testCatalogs);
        }

        public Task<TestCatalog> GetTestCatalogById(int id = 0)
        {
            TestCatalog? testCatalogById;

            if (id > 0)
                testCatalogById = GetAllTestsCatalog().Result.FirstOrDefault(x => x.Id == id);
            else
                throw new ObjectEmptyException("Test catalog not found");

            return Task.FromResult(testCatalogById);
        }

        public Task<string> AddTestCatalog(TestCatalog testCatalog)
        {
            if(testCatalog == null)
                throw new ObjectEmptyException("Test catalog is null");

            var testCatalogs = GetAllTestsCatalog().Result;

            int oldCount = testCatalogs.Count;

            int newId = testCatalogs.Count > 0 ? testCatalogs.Max(x => x.Id) + 1 : 1;
            testCatalog.Id = newId;

            string serializedTestCatalog = JsonSerializer.Serialize(testCatalog);

            if(testCatalogs.Count == 0)
            {
                File.AppendAllText(testCatalogPath, serializedTestCatalog);
            }
            else
            {
                File.AppendAllText(testCatalogPath, Environment.NewLine + serializedTestCatalog);
            }

            int newCount = GetAllTestsCatalog().Result.Count;
            string message = newCount > oldCount ? "Test catalog added successfully" : "Failed to add test catalog";

            return Task.FromResult(message);
        }

        public Task<string> UpdateTestCatalog(TestCatalog testCatalog)
        {
            if(testCatalog == null)
                throw new ObjectEmptyException("Test catalog is null");

            List<TestCatalog> testCatalogs = GetAllTestsCatalog().Result;

            var existingTestCatalog = testCatalogs.FirstOrDefault(x => x.Id == testCatalog.Id);

            var tt = testCatalogs.IndexOf(existingTestCatalog);

            int TestCatalogId = testCatalogs.FindIndex(x => x.Id == testCatalog.Id);

            if (existingTestCatalog == null)
                throw new ObjectEmptyException("Test catalog not found");

            if (TestCatalogId <= 0)
                throw new ObjectEmptyException("Test catalog not found");

            testCatalogs[TestCatalogId] = testCatalog;

            testCatalogs[TestCatalogId].Id = testCatalog.Id;
            testCatalogs[TestCatalogId].TestTitle = testCatalog.TestTitle;
            testCatalogs[TestCatalogId].Topic = testCatalog.Topic;
            testCatalogs[TestCatalogId].QuestionsNumber = testCatalog.QuestionsNumber;
            testCatalogs[TestCatalogId].MaximumScore = testCatalog.MaximumScore;
            testCatalogs[TestCatalogId].DateTime = testCatalog.DateTime;
            testCatalogs[TestCatalogId].PassingPercentage = testCatalog.PassingPercentage;
            testCatalogs[TestCatalogId].IsDelete = testCatalog.IsDelete;


            string result = testCatalogs[TestCatalogId].Equals(testCatalog) ? "Test catalog updated successfully" : "Failed to update test catalog";

            return Task.FromResult(result);
        }

        public async Task<string> DeleteTestCatalog(TestCatalog testCatalog)
        {
            if (testCatalog == null)
                throw new ObjectEmptyException("Test catalog is null");

           List<TestCatalog> testCatalogDeleted = GetAllTestsCatalog().Result;

            int testCatalogDeletedId = testCatalogDeleted.FindIndex(x => x.Id == testCatalog.Id);

            if (testCatalogDeletedId <=0)
                throw new ObjectEmptyException("Test catalog not found");

            testCatalogDeleted[testCatalogDeletedId].IsDelete = true;

            foreach (var item in testCatalogDeleted)
            {
                UpdateTestCatalog(item);
            }

            TestCatalog testCatalog1 = await GetTestCatalogById(testCatalog.Id);

            string result = testCatalog1.IsDelete ? "Test catalog deleted successfully" : "Failed to delete test catalog";

            return await Task.FromResult(result);
        }







        public Task<List<QuestionTest>> GetAllQuestions()
        {
           List<QuestionTest> questionTests = new List<QuestionTest>();

            string[] testCatalogs = File.ReadAllLines(questionTestPath);

            foreach (var item in testCatalogs)
            {
                QuestionTest? testCatalog = JsonSerializer.Deserialize<QuestionTest>(item);

                if (testCatalog != null && !testCatalog.IsDelete)
                {
                    questionTests.Add(testCatalog);
                }
            }


            return Task.FromResult(questionTests);
        }


        public async Task<List<QuestionTest>> GetQuestionsByTestCatalogId(int testCatalogId)
        {
            if (testCatalogId <= 0)
                throw new ObjectEmptyException("Invalid question test ID");

            List<QuestionTest> question = await GetAllQuestions();

            List<QuestionTest> questionTest = new List<QuestionTest>();

            questionTest = question.FindAll(x => x.Id == testCatalogId);


            return await Task.FromResult(questionTest);
        }

        public Task<List<AnswerTest>> GetAllAnswer()
        {
            List<AnswerTest> answerTests = new List<AnswerTest>();

            string[] answer = File.ReadAllLines(answerPath);

            foreach (var item in answer)
            {
                AnswerTest? answerTest  = JsonSerializer.Deserialize<AnswerTest>(item);

                if(answerTest != null || !answerTest.isDelete)
                    answerTests.Add(answerTest);
            }

            return Task.FromResult(answerTests);
        }

        public async Task<AnswerTest[]> GetAnswerById(int id)
        {
            if (id <= 0) throw new ObjectEmptyException("The ID is zero or less.");

            AnswerTest[] answer =  GetAllAnswer().Result.FindAll(x=>x.Id == id).ToArray();

            return answer;
        }

        public Task<string> AddQuestionTest(QuestionTest questionTest, AnswerTest[] answerTests)
        {
            string result;

            if(answerTests.Length <= 0)
                throw new ObjectEmptyException("AnswerTests test is null");

            if (questionTest == null)
                throw new ObjectEmptyException("Question test is null");

            List<QuestionTest> questionTests = GetAllQuestions().Result;

            List<AnswerTest> answer = GetAllAnswer().Result;

            int oldCountQuestion = questionTests.Count;

            int oldCountAswer = answer.Count;

            var tt = answer.Count > 0;

            int questionTestsId = questionTests.Count > 0 ? questionTests.Max(x => x.Id) + 1 : 1;

            int answerTestsId = answer.Count > 0 ? answer.Max(x => x.Id) + 1 : 1;


            questionTest.Id = questionTestsId;

            string serializedQuestionTest = JsonSerializer.Serialize(questionTest);

            if (questionTestsId == 1)
                File.AppendAllText(questionTestPath, serializedQuestionTest);
            else
                File.AppendAllText(questionTestPath, Environment.NewLine + serializedQuestionTest);

            
            foreach (var item in answerTests)
            {
                item.Id = answerTestsId;
                item.QuestionTestId = questionTestsId;

                string serializedAnswer = JsonSerializer.Serialize(item);

                if (answerTestsId == 1)
                    File.AppendAllText(answerPath, serializedAnswer);
                else
                    File.AppendAllText(answerPath, Environment.NewLine + serializedAnswer);

                answerTestsId++;
            }


            int newCountQuestion = GetAllQuestions().Result.Count;
            int newCountAswer = GetAllAnswer().Result.Count;


            result = newCountQuestion > oldCountQuestion && newCountAswer> oldCountAswer ? "Question test added successfully" : "Failed to add question test";

            return Task.FromResult(result);
        }

        public Task<string> UpdateQuestionTest(QuestionTest questionTest)
        {
            if (questionTest == null)
                throw new ObjectEmptyException("Question test is null");

            List<QuestionTest> questionTests = GetAllQuestions().Result;

            int questionTestsIndex = questionTests.FindIndex(x => x.Id == questionTest.Id);


            if(questionTestsIndex < 0)
                throw new ObjectEmptyException("Question test not found");

            questionTests[questionTestsIndex] = questionTest;

            //questionTests[questionTestsIndex].Id = questionTest.Id;
            //questionTests[questionTestsIndex].Question = questionTest.Question;
            //questionTests[questionTestsIndex].Answer1 = questionTest.Answer1;
            //questionTests[questionTestsIndex].Answer2 = questionTest.Answer2;
            //questionTests[questionTestsIndex].Answer3 = questionTest.Answer3;
            //questionTests[questionTestsIndex].Answer4 = questionTest.Answer4;
            //questionTests[questionTestsIndex].CorrectAnswer = questionTest.CorrectAnswer;
            //questionTests[questionTestsIndex].IsDelete = questionTest.IsDelete;
            //questionTests[questionTestsIndex].ChoiceQuestion = questionTest.ChoiceQuestion;
            //questionTests[questionTestsIndex].TestCatalog = questionTest.TestCatalog;

            string result = questionTests[questionTestsIndex].Equals(questionTest) ? "Question test updated successfully" : "Failed to update question test";

            return Task.FromResult(result);
        }

        public async Task<string> DeleteQuestionTest(QuestionTest questionTest)
        {
            if(questionTest == null)
                throw new ObjectEmptyException("Question test is null");

            questionTest.IsDelete = true;

            List<QuestionTest> questionTests = await GetAllQuestions();

            if(questionTests == null)
                throw new ObjectEmptyException("No question tests found");

            int questionTestToDelete = questionTests.FindIndex(x => x.Id == questionTest.Id);

            if (questionTestToDelete < 0)
                throw new ObjectEmptyException("Question test not found");
            
            questionTests[questionTestToDelete].IsDelete = true;

            string result = "";

            foreach (var item in questionTests)
            {
                result = await UpdateQuestionTest(item);
            }

            if (result.Contains("successfully"))
                result = "Question test deleted successfully";
            else
                result = "Failed to delete question test";

            return await Task.FromResult(result);
        }

        //string testCatalog
        public async Task<List<QuestionTest>> GetQuiziQuestion(int id)
        {
          
            if (id <= 0)
                throw new ObjectEmptyException("testCatalog is null");

            //int testCatalogId = testCatalog1.Id;

            List<QuestionTest> question = GetAllQuestions().Result.FindAll(q=> q.TestCatalogId == id && !q.IsDelete);

            Random random = new Random();

            List<QuestionTest> questionTests = question.OrderBy(x => random.Next()).Take(5).ToList();




            //List<QuestionTest> selectedQuestionTests = new List<QuestionTest>();
            //List<int> usedIndexes = new List<int>();

            //while (selectedQuestionTests.Count < 5)
            //{
            //    int index = random.Next(0, question.Count);

            //    if (!usedIndexes.Contains(index))
            //    {
            //        usedIndexes.Add(index);
            //        selectedQuestionTests.Add(selectedQuestionTests[index]);
            //    }
            //}


            return await Task.FromResult(questionTests);
        }

        public async Task<List<AnswerTest>> GetQuestionAnswer(int id)
        {
            if (id <= 0)
                throw new ObjectEmptyException("Question is null");

            Random random = new Random();

            List<AnswerTest> answerTests = GetAllAnswer()
                                            .Result
                                            .FindAll(x => x.Id == id && !x.isDelete)
                                            .OrderBy(x => random.Next())
                                            .ToList();


            //List<AnswerTest> answerTests = answerTests.OrderBy(x => random.Next()).ToList();


            return await Task.FromResult(answerTests);
        }



        public async Task<List<StudentsTestResult>> GetStudentsTestResult()
        {
            List<StudentsTestResult> studentsTestResults = new List<StudentsTestResult>();

            string[] lines = File.ReadAllLines(testResult);

            foreach (string line in lines)
            {
                StudentsTestResult? studentTestResult = JsonSerializer.Deserialize<StudentsTestResult>(line);

                if(studentTestResult != null)
                    studentsTestResults.Add(studentTestResult);

            }


            return await Task.FromResult(studentsTestResults);
        }



        public async Task AddQuestionAnswer(int studentId, int testCatalogId, int[] questionId, int[] answerId, bool[] isCorrect)
        {

            if (studentId <= 0 || testCatalogId <=0 ||questionId.Length <=0 || answerId.Length <=0 || isCorrect.Length <=0)
                throw new ObjectEmptyException("One of the IDs is empty.");

            StudentsTestResult studentsTestResult = new StudentsTestResult();


            List<StudentsTestResult> studentResult = await GetStudentsTestResult();

            studentsTestResult.Id = studentResult.Count > 0 ? studentResult.Max(x => x.Id) + 1 : 1;
            studentsTestResult.TestCatalogId = testCatalogId;
            studentsTestResult.StudentId = studentId;

            for (int i = 0; i < questionId.Length; i++)
            {
                studentsTestResult.QuestionTestId = questionId[i];
                studentsTestResult.AnswerId = answerId[i];
                studentsTestResult.IsCorrect = isCorrect[i];

                string testResultJson = JsonSerializer.Serialize(studentsTestResult);

                if (studentResult.Count <= 0)
                    File.AppendAllText(testResult, testResultJson);
                else
                    File.AppendAllText(testResult, Environment.NewLine + testResultJson);
            }


        }
    }
}
