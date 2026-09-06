
using QuizMaster.Core.Model;

namespace QuizMaster.Core.Interface
{
    public interface IQuestionTestRepository
    {
        //for student
        Task<List<TestCatalog>> GetAllTestsCatalog();
        Task<TestCatalog> GetTestCatalogById(int id);
        Task<string> AddTestCatalog(TestCatalog testCatalog);
        Task<string> UpdateTestCatalog(TestCatalog testCatalog);
        Task<string> DeleteTestCatalog(TestCatalog testCatalog);



        //for Lecturer
        Task<List<QuestionTest>> GetAllQuestions(string role);
        Task<QuestionTest> AddQuestionTest(QuestionTest questionTest);
        Task<List<QuestionTest>> GetQuestionsByTestCatalogId(int testCatalogId);
        Task<QuestionTest> UpdateQuestionTest(QuestionTest questionTest);
        Task<QuestionTest> DeleteQuestionTest(QuestionTest questionTest);
    }
}
