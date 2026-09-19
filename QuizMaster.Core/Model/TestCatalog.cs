
namespace QuizMaster.Core.Model
{
    public class TestCatalog
    {
        public int Id { get; set; }
        public string? TestTitle { get; set; }
        public string? Topic { get; set; }
        public double QuestionsNumber { get; set; }
        public double MaximumScore { get; set; }
        public byte DateTime { get; set; }
        public double PassingPercentage { get; set; }
        public bool IsDelete { get; set; } = false;


        public override bool Equals(object? obj)
        {
            return obj is TestCatalog catalog &&
                   Id == catalog.Id &&
                   TestTitle == catalog.TestTitle &&
                   Topic == catalog.Topic &&
                   QuestionsNumber == catalog.QuestionsNumber &&
                   MaximumScore == catalog.MaximumScore &&
                   DateTime == catalog.DateTime &&
                   PassingPercentage == catalog.PassingPercentage &&
                   IsDelete == catalog.IsDelete;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, TestTitle, Topic, QuestionsNumber, MaximumScore, DateTime, PassingPercentage, IsDelete);
        }

        public override string? ToString()
        {
            return $" Id: {Id},\n TestTitle: {TestTitle},\n Topic: {Topic},\n QuestionsNumber: {QuestionsNumber},\n MaximumScore: {MaximumScore},\n DateTime: {DateTime},\n PassingPercentage: {PassingPercentage}\n";

        }
    }
}
