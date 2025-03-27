using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Apocalypse
{
    public class Zombies
    {
        private int Zombies_amount {  get; set; }  // Кількість зомбі

        public Zombies(int zombies_amount) {
            Zombies_amount = zombies_amount;
        }
        public override string ToString()
        {
            return $"Кількість зомбі: {Zombies_amount}";
        }

        // Метод для зміни кількості зомбі на вказану величину
        public void ChangeZombiesAmount(int numberToChange)
        {
            Zombies_amount += numberToChange;
        }

        public int GetZombiesAmount() { return Zombies_amount; }

        // Метод, що викликається при настанні ночі
        // Кількість людей у групах зменшується (від 0 до 5 осіб у кожній групі)
        // Усі заражені люди стають зомбі, і їхня кількість додається до загального числа зомбі
        public void OnNightHasCome(object sendler, List<Civilians> groups)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine( $"{Zombies_amount} ЗОМБІ НА ПОЛЮВАННІ!!!");

            Random random = new Random();
            int transformedToZombies = 0;
            foreach (Civilians group in groups)
            {
                int randomNumber = random.Next(0, 6); // Випадкове число від 0 до 5
                group.ChangeNumberOfPeople(-randomNumber);
                transformedToZombies += randomNumber;
            }

            this.ChangeZombiesAmount(transformedToZombies);  // Додаємо заражених до зомбі
            

            Console.ResetColor();
        }

        // Метод, що викликається вдень - зомбі неактивні та ховаються
        public void OnDayHasCome()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Зомбі ховаються!!!");
            Console.ResetColor();
        }
        // Метод, що викликається, коли СуперРятівник здійснює боротьбу
        // Відображається випадкова фраза від зомбі
        public void OnKossacMadeAction()
        {
            List<string> zobies_frazes = new List<string>
            {
                "Проміняв жінку на тютюн та люльку",
                "Вєткі калін пакланіліся, зачім же ми зомбі маліліся",
                "Піду втоплюся у річці глибокій",
                "А вже осінь прийшла у мій сад, Сагайдачний прошу йди назад",
                "Ні обіцянок, ні пробачень, це все зробив нам Сагайдачний",
                "На могилі моїй посадіть молоду Яворину", 
                "І я на небі, мила моя на небі, зомбі моя на небі, відколи Петя мене знайшов",
                "А чому Сагайдачний не в костюмі?"
            };
            Random random = new Random();
            string randomPhrase = zobies_frazes[random.Next(zobies_frazes.Count)]; // Випадкова фраза

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($" - Зомбі: {randomPhrase}");
            Console.WriteLine();
            Console.ResetColor();

        }

    }
}
