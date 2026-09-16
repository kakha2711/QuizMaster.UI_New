
using QuizMaster.Core;

namespace QuizMaster.Service.Exceptions
{
    public class DuplicatePersonalNumberException : Exception
    {
        public DuplicatePersonalNumberException()
        {
        }

        public DuplicatePersonalNumberException(string? message) : base(message)
        {
            //ColloringConsole.Error(message);
        }
    }
}
