
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
        Task<List<QuestionTest>> GetAllQuestions();
        Task<List<QuestionTest>> GetQuestionsByTestCatalogId(int testCatalogId);
        Task<string> AddQuestionTest(QuestionTest questionTest);
        Task<string> UpdateQuestionTest(QuestionTest questionTest);
        Task<string> DeleteQuestionTest(QuestionTest questionTest);
    }
}
