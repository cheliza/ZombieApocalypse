using System;

namespace Apocalypse
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ствворюєсо об'єкти класів Sun, Dayguard, Civilians, SuperSavior
            Sun sun = new Sun();

            Zombies zombies = new Zombies(250);

            Dayguard pmi_24 = new Dayguard(18, "ПМІ-24");
            Dayguard pmi_25 = new Dayguard(16, "ПМІ-25");
            Dayguard pmi_26 = new Dayguard(21, "ПМІ-26");

            Civilians workers = new Civilians(200, "роботяги", "працюють на роботі");
            Civilians students = new Civilians(100, "студентики", "гризуть граніт науки");
            Civilians school_students = new Civilians(150, "школярі", "йдуть до школи");
            Civilians pensioners = new Civilians(50, "пенсіонери", "їздять в автобусі і сваряться з студентами");
            Civilians saved_people = new Civilians(0, "врятовані", "лікуються");

            SuperSavior superSavior = new SuperSavior();

            // Підписуємо обробники подій об'єктів для ночі 
            sun.NightHasComeForCivilians += zombies.OnNightHasCome;
            sun.NightHasCome += pmi_24.OnNightHasCome;
            sun.NightHasCome += pmi_25.OnNightHasCome;
            sun.NightHasCome += pmi_26.OnNightHasCome;
            sun.NightHasCome += workers.OnNightHasCome;
            sun.NightHasCome += students.OnNightHasCome;
            sun.NightHasCome += school_students.OnNightHasCome;
            sun.NightHasCome += pensioners.OnNightHasCome;
            sun.NightHasCome += saved_people.OnNightHasCome;

            // Підписуємо обробники подій об'єктів для дня
            sun.DayHasCome += zombies.OnDayHasCome;
            sun.DayHasCome += pmi_24.OnDayHasCome;
            sun.DayHasCome += pmi_25.OnDayHasCome;
            sun.DayHasCome += pmi_24.OnDayHasCome;
            sun.DayHasCome += workers.OnDayHasCome;
            sun.DayHasCome += students.OnDayHasCome;
            sun.DayHasCome += school_students.OnDayHasCome;
            sun.DayHasCome += pensioners.OnDayHasCome;
            sun.DayHasCome += saved_people.OnDayHasCome;

            // Підписуємо обробники подій об'єктів для парного та непарного днів  
            sun.IsEvenDay += superSavior.OnEvenChange;
            sun.IsNotEvenDay += superSavior.OnNotEvenChange;

            // Підписуємо обробники подій об'єктів після здійснення дій СуперРятівником(Сагайдачним) 
            superSavior.SuperSaviorMadeAction += workers.OnSeviorMadeAction;
            superSavior.SuperSaviorMadeAction += students.OnSeviorMadeAction;
            superSavior.SuperSaviorMadeAction += school_students.OnSeviorMadeAction;
            superSavior.SuperSaviorMadeAction += pensioners.OnSeviorMadeAction;
            superSavior.SuperSaviorMadeAction += zombies.OnKossacMadeAction;

            // Підписуємо обробники подій об'єктів для суботи
            sun.SaturdayHasCome += saved_people.OnSaturdayHasCome;

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ЗОМБІ-АПОКАЛІПСИС");
            Console.ResetColor();

            // Запускаємо цикл для симуляції
            bool running = true;
            while (running)
            {
                Console.WriteLine(sun);

                // Створюємо списки цивільних та гвардії, з якими будуть працювати зомбі, рятівник
                List<Civilians> civilianGroups = new List<Civilians> { workers, students, school_students, pensioners };
                List<Dayguard> dayguard = new List<Dayguard> { pmi_24, pmi_25, pmi_26 };

                Console.WriteLine("Введіть кількість годин (більше 0) або 'exit' для виходу: ");
                string input = Console.ReadLine();

                if (input.ToLower() == "exit")
                {
                    running = false; // Завершуємо цикл, якщо введено "exit"
                    break; 
                }

                int hours;
                if (int.TryParse(input, out hours) && hours >= 0)
                {
                    int days = hours / 24;
                    int remainingHours = hours % 24;

                    // Викликаємо метод AddTime, щоб додати цей час і симулювати відповідні події
                    sun.AddTime(new TimeSpan(days, remainingHours, 0, 0), civilianGroups, zombies, saved_people, dayguard);
                }
                else
                {
                    Console.WriteLine("Неправильний формат часу або введено від'ємне число.");
                }
            }


        }
    }
}

