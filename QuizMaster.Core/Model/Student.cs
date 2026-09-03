
using QuizMaster.Core.Enum;

namespace QuizMaster.Core.Model
{
    public class Student : Person
    {
        public double Grade { get; set; }
        
        public int CompareTo(Student? other)
        {
            if (other == null) return 1;
            return this.Grade.CompareTo(other.Grade);
        }

        public override string? ToString()
        {
            return $"  Id: {Id},\n FirsName: {FirsName},\n Lastname: {Lastname},\n Email: {Email},\n PhoneNumber: {PhoneNumber},\n PersonalNumber: {PersonalNumber},\n UserName: {UserName},\n Password: {Password},\n VerificationCode: {VerificationCode},\n IsVerified: {IsVerified},\n Role: {Role},\n Gender: {Gender},\n Grade: {Grade},\n IsDelete: {IsDelete}\n";
        }
    }
}
