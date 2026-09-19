
namespace QuizMaster.Service.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
        {
        }

        public ValidationException(string? message) : base(message)
        {
            //ColloringConsole.Error(message);
        }
    }
}
