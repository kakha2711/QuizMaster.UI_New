using QuizMaster.Infrastructure.Repositori;
using QuizMaster.Service;
using QuizMaster.UI;

Menu menu = new Menu(new StudentService(new StudentRepository()), new QuestionTestRepositoryService( new QuestionTestRepository()));

menu.Show();