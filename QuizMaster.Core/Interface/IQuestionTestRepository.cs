
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
        Task<string> AddQuestionTest(QuestionTest questionTest, AnswerTest[] answerTests);
        Task<string> UpdateQuestionTest(QuestionTest questionTest);
        Task<string> DeleteQuestionTest(QuestionTest questionTest);


        Task<List<QuestionTest>> GetQuiziQuestion(int id);
        Task<List<AnswerTest>> GetQuestionAnswer(int id);
        Task<List<StudentsTestResult>> GetStudentsTestResult();
        Task AddQuestionAnswer(int studentId, int testCatalogId, int[] questionId, int[] answerId, bool[] isCorrect);

    }
}
