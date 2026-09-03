using QuizMaster.Core.Enum;

namespace QuizMaster.Core.Model
{
    public class Lecturer : Person
    {

        public override string? ToString()
        {
            return $"  Id: {Id},\n FirsName: {FirsName},\n Lastname: {Lastname},\n Email: {Email},\n PhoneNumber: {PhoneNumber},\n PhoneNumber: {PhoneNumber},\n Password: {Password},\n VerificationCode: {VerificationCode},\n IsVerified: {IsVerified},\n Role: {Role},\n Gender: {Gender}\n";
        }
    }
}
