
using QuizMaster.Core;
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
            try
            {
                string result = await _questionTestRepository.AddTestCatalog(testCatalog);

                if (result.Contains("success"))
                    ColloringConsole.Success(result);
                else
                    ColloringConsole.Error(result);
            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("success"))
                    ColloringConsole.Success(ex.Message);
                else
                    ColloringConsole.Error(ex.Message);

            }
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

        public void AddQuestionTest(QuestionTest questionTest, AnswerTest[] answerTests)
        {
            _questionTestRepository.AddQuestionTest(questionTest, answerTests);
        }

        public void UpdateQuestionTest(QuestionTest questionTest)
        {
            _questionTestRepository.UpdateQuestionTest(questionTest);
        }





        public async Task<List<QuestionTest>> GetQuestionQuizi(int id)
        {
            List<QuestionTest> questionTest = await _questionTestRepository.GetQuiziQuestion(id);

            return await Task.FromResult(questionTest);
        }

        public async Task<List<AnswerTest>> GetAnswerQuizi(int id)
        {
            List<AnswerTest> answerTest = await _questionTestRepository.GetQuestionAnswer(id);
            return await Task.FromResult(answerTest);
        }


        public async Task<List<StudentsTestResult>> GetStudentsTestResult()
        {
            List<StudentsTestResult> studentsTestResults = await _questionTestRepository.GetStudentsTestResult();
            return await Task.FromResult(studentsTestResults);
        }

        public async Task AddQuestionAnswer(int studentId, int testCatalogId, int questionId, int[] answerId, int[] isCorrect)
        {
            await _questionTestRepository.AddQuestionAnswer(studentId, testCatalogId, questionId, answerId, isCorrect);
        }

        public async Task<List<StudentsTestResult>> GetLiderBoard()
        {
            //List<StudentsTestResult> studentProgress = await _questionTestRepository.GetLiderBoard();
            return await _questionTestRepository.GetLiderBoard();
        }
    }
}
