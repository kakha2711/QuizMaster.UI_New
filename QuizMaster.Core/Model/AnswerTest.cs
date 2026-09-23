
using FluentValidation;
using QuizMaster.Core.Enum;

namespace QuizMaster.Core.Model
{
    public class AnswerTest
    {
        public AnswerTest()
        {
            var validator = new AnswerTestValidator();
            var result = validator.Validate(this);
        }

        public int Id { get; set; }
        public string? Answer { get; set; }
        public bool IsCorrect { get; set; }
        public bool isDelete { get; set; }
        public int QuestionTestId { get; set; }


        public override string? ToString()
        {
            //, VerificationCode: { VerificationCode}
            //return $"Id: {Id}, Question: {Question}, Answer1: {Answer1}, Answer2: {Answer2}, Answer3: {Answer3}, Answer4: {Answer4}, choiceQuestion: {ChoiceQuestion}";

            return $"Id: {Id}, Question: {Answer}";
        }
    }

    public class AnswerTestValidator : AbstractValidator<AnswerTest>
    {
        public AnswerTestValidator()
        {
            RuleFor(x => x.Id).InclusiveBetween(0, int.MaxValue).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.Answer).NotEmpty().WithMessage("Answer is required.");
            RuleFor(x => x.IsCorrect).IsInEnum().WithMessage("IsCorrectis true or false.");
            RuleFor(x => x.isDelete).IsInEnum().WithMessage("IsCorrectis true or false.");
            RuleFor(x => x.QuestionTestId).InclusiveBetween(0, int.MaxValue).WithMessage("Id must be greater than 0.");

        }
    }
}
