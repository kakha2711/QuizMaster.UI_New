
using FluentValidation;
using QuizMaster.Core.Enum;

namespace QuizMaster.Core.Model
{
    public class QuestionTest
    {
        public QuestionTest()
        {
            var validator = new QuestionTestValidator();
            var result = validator.Validate(this);
        }

        public int Id { get; set; }
        public string QuestionText { get; set; }
        
        public bool IsDelete { get; set; } = false;
        public ChoiceQuestion ChoiceQuestion { get; set; }
        public int TestCatalogId { get; set; }
        
        public override string? ToString()
        {
            //, VerificationCode: { VerificationCode}
            //return $"Id: {Id}, Question: {Question}, Answer1: {Answer1}, Answer2: {Answer2}, Answer3: {Answer3}, Answer4: {Answer4}, choiceQuestion: {ChoiceQuestion}";

            return $"Id: {Id}, Question: {QuestionText}, choiceQuestion: {ChoiceQuestion}";
        }

    }

    public class QuestionTestValidator : AbstractValidator<QuestionTest>
    {
        public QuestionTestValidator()
        {
            RuleFor(x => x.Id).InclusiveBetween(1, int.MaxValue).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.QuestionText).NotEmpty().WithMessage("Question is required.");
            RuleFor(x => x.IsDelete).IsInEnum().WithMessage("IsCorrectis true or false.");
            RuleFor(x => x.ChoiceQuestion).IsInEnum().WithMessage("ChoiceQuestion must be a valid enum value.");
            RuleFor(x => x.TestCatalogId).InclusiveBetween(0, int.MaxValue).WithMessage("Id must be greater than 0.");

        }
    }
}
