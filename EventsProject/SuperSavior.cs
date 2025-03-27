using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Apocalypse
{
    // Делегат для події, що викликається, коли СуперРятівник здійснює дію
    public delegate void SuperSaviorEventHandler();

    internal class SuperSavior
    {
        private string Name {  get; set; } // Ім'я рятівника
        private int Energy { get; set; } // Поточний рівень енергії-сили

        // Подія, яка спрацьовує при дії козака
        public event SuperSaviorEventHandler SuperSaviorMadeAction;

        // Конструктор з параметрами за замовчуванням
        public SuperSavior(string name= "Сагайдачний", int energy=100)
        {
            Name = name;
            Energy = energy;
        }

        public override string ToString() {
            return $"{Name} - сила {Energy}";
        }

        // Метод, що викликається при настанні парних днів
        // Кількість зомбі зменшується
        // Група врятованих людей відповідно збільшується
        // СуперРятівник витрачає енергію (чим більше знищив, тим більше втратив)
        // Після цього викликається подія SuperSaviorMadeAction
        public void OnEvenChange(Zombies zombies, People saved_people)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Random random = new Random();
            int randomNumber = random.Next(0, 30); // Перетворює від 0 до 30 зомбі

            zombies.ChangeZombiesAmount(-randomNumber);
            saved_people.ChangeNumberOfPeople(randomNumber);

            Energy-=randomNumber*2;
            Console.WriteLine($"{Name} - Наш козак! Сила: {Energy};  Знищив зомбі, врятував людей: {randomNumber};\n");
            Console.ResetColor();
            SuperSaviorMadeAction?.Invoke();
        }

        // Метод, що викликається при не парних днях
        public void OnNotEvenChange()
        {
            Energy += 50;
            Console.WriteLine($"{Name} сила: {Energy}");
        }
    }
}
