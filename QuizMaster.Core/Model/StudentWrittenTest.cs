namespace QuizMaster.Core.Model
{
    public class StudentWrittenTest
    {

        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TestCatalogId { get; set; }
        public int QuestionTestId { get; set; }
        public string Answer { get; set; }

        public bool IsDelete { get; set; } = false;

    }
}
