
using FluentValidation;
using QuizMaster.Core.Enum;

namespace QuizMaster.Core.Model
{
    public class Person
    {
        public int Id { get; set; }
        public bool IsDelete { get; set; } = false;


        public string? FirsName { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PersonalNumber { get; set; }

        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? VerificationCode { get; set; }
        public bool IsVerified { get; set; } = false;

        public Role Role { get; set; } = Role.Student;
        public Gender Gender { get; set; }
    }


    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(x => x.FirsName).NotEmpty().WithMessage("First name is required.");
            RuleFor(x => x.Lastname).NotEmpty().WithMessage("Last name is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required.");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("password must be at least 8 characters")
                .Matches(@"[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]")
                .WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"[0-9]")
                .WithMessage("Password must contain at least one digit letter");

            RuleFor(x => x.PersonalNumber)
                .NotEmpty()
                .WithMessage("Personal number is required.")
                .Matches(@"[0-9]")
                .WithMessage("Personal number must be only numbers");

            RuleFor(x => x.VerificationCode).NotEmpty().WithMessage("Verification code is required.");
            RuleFor(x => x.IsVerified).NotNull().WithMessage("IsVerified is required.");

            RuleFor(x => x.Role).IsInEnum().WithMessage("Role must be a valid enum value.");
            RuleFor(x => x.Gender).IsInEnum().WithMessage("Gender must be a valid enum value.");
            RuleFor(x => x.Id).InclusiveBetween(0, int.MaxValue).WithMessage("Id must be greater than 0.");
        }
    }
}
