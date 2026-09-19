
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

            foreach (var item in question)
            {
                if (item.TestCatalog.Id == testCatalogId)
                {
                    questionTest.Add(item);
                }
            }

            return await Task.FromResult(questionTest);
        }
        public Task<string> AddQuestionTest(QuestionTest questionTest)
        {
            string result;

            if(questionTest == null)
                throw new ObjectEmptyException("Question test is null");

            var questionTests = GetAllQuestions().Result;

            int oldCount = questionTests.Count;

            int questionTestsId = questionTests.Count > 0 ? questionTests.Max(x => x.Id) + 1 : 1;

            questionTest.Id = questionTestsId;

            string serializedQuestionTest = JsonSerializer.Serialize(questionTest);

            if (questionTests.Count == 0)
                File.AppendAllText(questionTestPath, serializedQuestionTest);
            else
                File.AppendAllText(questionTestPath, Environment.NewLine + serializedQuestionTest);
            
            int newCount = GetAllQuestions().Result.Count;

            result = newCount > oldCount ? "Question test added successfully" : "Failed to add question test";

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
    }
}
