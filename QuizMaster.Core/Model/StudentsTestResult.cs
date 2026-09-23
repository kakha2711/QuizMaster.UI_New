namespace QuizMaster.Core.Model
{
    public class StudentsTestResult
    {

        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TestCatalogId { get; set; }
        public int QuestionTestId { get; set; }
        public int AnswerId { get; set; }
        public bool IsCorrect { get; set; }

        public bool IsDelete { get; set; } = false;

    }
}
