
using QuizMaster.Core.Interface;
using QuizMaster.Core.Model;
using QuizMaster.Service.Exceptions;
using System.Text.Json;

namespace QuizMaster.Infrastructure.Repositori
{
    internal class QuestionTestRepository : IQuestionTestRepository
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
            testCatalogs[TestCatalogId].QuctionsNumber = testCatalog.QuctionsNumber;
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







        public Task<List<QuestionTest>> GetAllQuestions(string role)
        {
            throw new NotImplementedException();
        }
        public Task<List<QuestionTest>> GetQuestionsByTestCatalogId(int testCatalogId)
        {
            throw new NotImplementedException();
        }
        public Task<QuestionTest> AddQuestionTest(QuestionTest questionTest)
        {
            throw new NotImplementedException();
        }
        public Task<QuestionTest> UpdateQuestionTest(QuestionTest questionTest)
        {
            throw new NotImplementedException();
        }
        public Task<QuestionTest> DeleteQuestionTest(QuestionTest questionTest)
        {
            throw new NotImplementedException();
        }
    }
}
