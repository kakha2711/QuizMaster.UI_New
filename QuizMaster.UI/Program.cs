using QuizMaster.Infrastructure.Repositori;
using QuizMaster.Service;
using QuizMaster.UI;

Menu menu = new Menu(new StudentService(new StudentRepository()));

menu.Show();