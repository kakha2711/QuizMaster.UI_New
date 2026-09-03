
namespace QuizMaster.Core.Model
{
    public class TestCatalog
    {
        public int Id { get; set; }
        public string? TestTitle { get; set; }
        public string? Topic { get; set; }
        public double QuctionsNumber { get; set; }
        public double MaximumScore { get; set; }
        public byte DateTime { get; set; }
        public double PassingPercentage { get; set; }
        public bool IsDelete { get; set; } = false;


        public override string? ToString()
        {
            return $"Id: {Id}, TestTitle: {TestTitle}, Topic: {Topic}, QuctionsNumber: {QuctionsNumber}, MaximumScore: {MaximumScore}, DateTime: {DateTime}, PassingPercentage: {PassingPercentage}";

        }
    }
}
