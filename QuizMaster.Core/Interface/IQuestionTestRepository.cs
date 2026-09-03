
using QuizMaster.Core.Model;

namespace QuizMaster.Core.Interface
{
    public interface IQuestionTestRepository
    {
        //for student
        Task<List<Person>> GetAllTestsCatalog(string role);
        //for Lecturer
        Task<List<Person>> GetAllQuestions(string role);

        //for student
        Task<TestCatalog> GetTestCatalogById(int id);
        //for student
        Task<List<QuestionTest>> GetQuestionsByTestCatalogId(int testCatalogId);

        //for lecturer
        Task<TestCatalog> AddTestCatalog(TestCatalog testCatalog);
        Task<QuestionTest> AddQuestionTest(QuestionTest questionTest);

        Task<TestCatalog> UpdateTestCatalog(TestCatalog testCatalog);
        Task<QuestionTest> UpdateQuestionTest(QuestionTest questionTest);

        Task<TestCatalog> DeleteTestCatalog(TestCatalog testCatalog);
        Task<QuestionTest> DeleteQuestionTest(QuestionTest questionTest);
    }
}
