
using QuizMaster.Core.Enum;

namespace QuizMaster.Core.Model
{
    public class StudentProgress
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Progres progres { get; set; }
        public int Attempt { get; set; } //რამდენჯერ ცადა ტესტის დაწერა
    }
}
