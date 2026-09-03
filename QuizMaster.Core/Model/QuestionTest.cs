
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
        public string Question { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public string CorrectAnswer { get; set; }
        public bool IsDelete { get; set; } = false;
        public ChoiceQuestion ChoiceQuestion { get; set; }
        public TestCatalog TestCatalog { get; set; }

        public override string? ToString()
        {
            //, VerificationCode: { VerificationCode}
            return $"Id: {Id}, Question: {Question}, Answer1: {Answer1}, Answer2: {Answer2}, Answer3: {Answer3}, Answer4: {Answer4}, choiceQuestion: {ChoiceQuestion}";
        }

    }

    public class QuestionTestValidator : AbstractValidator<QuestionTest>
    {
        public QuestionTestValidator()
        {
            RuleFor(x => x.Id).InclusiveBetween(1, int.MaxValue).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.Question).NotEmpty().WithMessage("Question is required.");
            RuleFor(x => x.Answer1).NotEmpty().WithMessage("Question is required.");
            RuleFor(x => x.Answer2).NotEmpty().WithMessage("Question is required.");
            RuleFor(x => x.Answer3).NotEmpty().WithMessage("Question is required.");
            RuleFor(x => x.Answer4).NotEmpty().WithMessage("Question is required.");
            RuleFor(x => x.CorrectAnswer).NotEmpty().WithMessage("Question is required.");
            RuleFor(x => x.ChoiceQuestion).IsInEnum().WithMessage("ChoiceQuestion must be a valid enum value.");
            RuleFor(x => x.TestCatalog).IsInEnum().WithMessage("ChoiceQuestion must be a valid enum value.");

        }
    }
}
