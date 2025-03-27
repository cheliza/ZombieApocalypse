using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Apocalypse
{
    // Базовий клас для груп людей
    public class People
    {
        protected int Number_of_people {  get; set; } // Кількість людей у групі
        protected string People_role {  get; set; } // Роль групи

        public People(int people_amount,  string role)
        {
            Number_of_people = people_amount;
            People_role = role;
        }
        public int GetPeopleAmount() { return Number_of_people; }
        public override string ToString()
        {
            return $"{People_role} - кількість груп {Number_of_people}";
        }

        // Метод для зміни кількості людей у групі
        public void ChangeNumberOfPeople(int number_to_change)
        {
            if ((Number_of_people + number_to_change)>=0) { 
                Number_of_people += number_to_change;
            }
        }

    }
    // Клас "Денна гвардія" (успадковується від People)
    // На одне місто може бути кілька гвардій
    public class Dayguard: People
    {
        private string Guard_name {  get; set; }  // Назва гвардії
        public Dayguard(int people_amount, string guard_name) : base(people_amount, "Денна гвардія")
        {
            Guard_name = guard_name;
        }
        public override string ToString() {
            return $"{base.ToString()}, Гвардія: {Guard_name}";
        }

        // Денна робота гвардії
        public void OnDayHasCome()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Гвардія:{Number_of_people} {Guard_name} ліквідують наслідки атаки");
            Console.ResetColor();

        }
        // Нічний захист
        public void OnNightHasCome()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Гвардія:{Number_of_people} {Guard_name} захищають людей від атаки");
            Console.ResetColor();
        }
       
    }
    // Клас "Цивільні" (успадковується від People)
    public class Civilians : People
    {
        private string Civilians_type { get; set; } // Роль цивільних
        private string Activity {  get; set; }  // Їх діяльність
        public Civilians(int people_amount, string type, string activity) : base(people_amount, "Цивільні")
        {
            Civilians_type = type;
            Activity = activity;
        }
        public override string ToString()
        {
            return $"{base.ToString()}, Група: {Civilians_type}";
        }

        // Дії цивільних вдень
        public void OnDayHasCome()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Цивільні: {Number_of_people} {Civilians_type} {Activity}");
            Console.ResetColor();
        }
        // Дії цивільних вночі
        public void OnNightHasCome()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Цивільні:{Number_of_people} {Civilians_type} ховаються вдома");
            Console.ResetColor();
        }
        // Щосуботи частина цивільних-врятованих стає гвардією
        public void OnSaturdayHasCome(object sender, List<Dayguard> dayguards, int percentage)
        {
            int civiliansToTransfer = (int)(this.Number_of_people * (percentage / 100.0)); 
            foreach(Dayguard dayguard in dayguards)
            {
                dayguard.ChangeNumberOfPeople(civiliansToTransfer);
                this.ChangeNumberOfPeople(-civiliansToTransfer);
            }
            Console.WriteLine($"{dayguards.Count*civiliansToTransfer} люди завершили лікарняний\n");
        }

        // Реакція цивільних на появу козака-рятівника
        public void OnSeviorMadeAction()
        {
            List<string> zobies_frazes = new List<string>
            {
                "Наш козак!!!!!",
                "Зомбі тіло ляже в грунт, допоможе козаку",
                "Козака на максимум, зомбі буде так собі",
                "Добрий день, еврібаді, сьогодні Сагайдачний запрошує на паті",
                "Сагайдачний справжній лис, зомбі гриз як барбарис",
                "Зомбі-бомбі, що таке, приготуй собі пакет",
                "Я і друзів розумів, і зомбі не умів прощати, бо хрещений батько мій - Сагайдачний, Сагайдачний",
                "Зомбі з возу, Сагайдачному легше"
            };
            Console.ForegroundColor = ConsoleColor.Blue;
            Random random = new Random();
            string randomPhrase = zobies_frazes[random.Next(zobies_frazes.Count)];
            Console.WriteLine($" - {Civilians_type}: {randomPhrase}");
            Console.ResetColor();
        }
    }
     
}

