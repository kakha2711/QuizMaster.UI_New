
using QuizMaster.Core.Model;

namespace QuizMaster.Core.Interface
{
    public interface IPersonRepository
    {
        Task<List<Person>> GetAllStudent(string role);
        public Person GetPersonByPersonalNumber(string personalNumber, string role);
        public Task<Person> GetPersonByUserName(string username, string role);
        public  Task<string> AddStudent(Person person, int param = 0);
        public Task<string> UpdateStudent(Person student);
        public Task<string> DeleteStudent(string personalNumber);
        //public  Task<Person> GetPersonByUserName<Person>(string username, string role);
    }
}

