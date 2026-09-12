
using QuizMaster.Core.Interface;
using QuizMaster.Core.Model;

namespace QuizMaster.Service
{
    public class QuestionTestRepositoryService
    {
        private readonly IQuestionTestRepository _questionTestRepository;

        public QuestionTestRepositoryService(IQuestionTestRepository questionTestRepository)
        {
            _questionTestRepository = questionTestRepository;
        }


        public async Task<List<TestCatalog>> GetAllTestsCatalog()
        {
            return await _questionTestRepository.GetAllTestsCatalog();
        }

        public async Task<TestCatalog> GetTestCatalogById(int id)
        {
            return await _questionTestRepository.GetTestCatalogById(id);
        }

        public async Task AddTestCatalog(TestCatalog testCatalog)
        {
            await _questionTestRepository.AddTestCatalog(testCatalog);
        }

        public async Task UpdateTestCatalog(TestCatalog testCatalog)
        {
            await _questionTestRepository.UpdateTestCatalog(testCatalog);
        }

        public async Task DeleteTestCatalog(TestCatalog testCatalog)
        {
            await _questionTestRepository.DeleteTestCatalog(testCatalog);
        }





        public async Task<List<QuestionTest>> GetAllQuestionTests()
        {
            return await _questionTestRepository.GetAllQuestions();
        }

        public void AddQuestionTest(QuestionTest questionTest)
        {
            _questionTestRepository.AddQuestionTest(questionTest);
        }

        public void UpdateQuestionTest(QuestionTest questionTest)
        {
            _questionTestRepository.UpdateQuestionTest(questionTest);
        }


    }
}
