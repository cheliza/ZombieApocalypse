using System;
using System.Text.RegularExpressions;

namespace Apocalypse
{
    // Делегати для різних подій
    public delegate void NightDayEventHandler();// Викликається при настанні дня або ночі
    public delegate void NightEventHandlerCivilians(object sender, List<Civilians> civilians = null); // Викликається вночі для цивільних
    public delegate void EvenDaysHandler(Zombies zombies, People saved_people); // Викликається у парні дні
    public delegate void SaturdayEventHandler(object sender, List<Dayguard> dayguard = null, int proc = 10); // Викликається у суботу

    internal class Sun
    {
        private DateTime currentTime; // Поточний час
        private int lastDay;  // Попередній до поточного день
        bool hasDayHappened = false; // Позначає, чи був день
        bool hasNightHappened = false; // Позначає, чи була ніч

        // Події, що спрацьовують при зміні часу
        public event NightDayEventHandler NightHasCome; // Настала ніч
        public event NightEventHandlerCivilians NightHasComeForCivilians; // Настала ніч для цивільних
        public event NightDayEventHandler DayHasCome; // Настав день
        public event EvenDaysHandler IsEvenDay; // Настав парний день
        public event NightDayEventHandler IsNotEvenDay; // Настав непарний день
        public event SaturdayEventHandler SaturdayHasCome;  // Настала субота

        public Sun()
        {
            currentTime = DateTime.Now;
            lastDay = currentTime.Day-1; 

        }
        public override string ToString()
        {
            return $"Поточна година: {currentTime}";

        }
        // Метод, який додає час і викликає відповідні події (день, ніч, парні/непарні дні, субота)
        // Симулює рух часу та спрацьовує для різних подій, які залежать від часу доби
        public void AddTime(TimeSpan timeToAdd, List<Civilians> civilians, Zombies zombies, People saved_people, List<Dayguard> dayguard)
        {
            // Обчислюємо кількість годин, яку потрібно додати (враховуємо і дні)
            int hoursToAdd = timeToAdd.Hours+ timeToAdd.Days*24;
           
            for (int i = 0; i < hoursToAdd; i++) // Проходимо по кожній годині 
            {
                currentTime = currentTime.Add(TimeSpan.FromHours(1)); //Змінюємо час
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Поточна година: {currentTime}"); // Виводимо поточний час в консоль
                Console.ResetColor();
                if (currentTime.Hour >= 7 && currentTime.Hour < 21) // Перевіряємо, чи поточний час знаходиться в межах з 7 до 21 - день
                {
                    if (!hasDayHappened)  // Якщо до цього ще не виконувались події для дня, то викликаємо їх
                    {
                        DayHasCome?.Invoke();
                        hasDayHappened = true;
                        hasNightHappened = false;
                    }
                    else
                    {
                        Console.WriteLine("Нічого не змінилось з попереднього часу.");
                    }
                }
                else
                {
                    if (!hasNightHappened)   // Якщо до цього ще не виконувались події для ночі, то викликаємо їх
                    {
                        NightHasComeForCivilians?.Invoke(this, civilians);
                        NightHasCome?.Invoke();
                        hasNightHappened = true;
                        hasDayHappened = false;
                    }
                    else
                    {
                        Console.WriteLine("Нічого не змінилось з попереднього часу.");
                    }
                }
                // Виводимо інформацію про кількість зомбі та цивільних
                Console.WriteLine($"Кількість зомбі: {zombies.GetZombiesAmount()}");
                int civilians_amount = 0;
                foreach (Civilians civilian in civilians) {
                    civilians_amount += civilian.GetPeopleAmount();
                }
                Console.WriteLine($"Кількість всіх цивільних: {civilians_amount}");
                Console.WriteLine($"Кількість врятованих: {saved_people.GetPeopleAmount()}");

                // Перевіряємо, чи змінився день
                if (currentTime.Day != lastDay)
                {

                    lastDay = currentTime.Day;

                    // Якщо день парний, викликаємо подію для парних днів
                    if (currentTime.Day % 2 == 0)  
                    {
                        IsEvenDay?.Invoke(zombies, saved_people);
                    }
                    // Якщо день непарний, викликаємо подію для непарних днів
                    else
                    {
                        IsNotEvenDay?.Invoke();
                    }

                    // Якщо сьогодні субота, викликаємо подію для суботи
                    if (currentTime.DayOfWeek == DayOfWeek.Saturday)
                    {
                        SaturdayHasCome?.Invoke(this, dayguard, 10);
                    }
                }
                // Якщо день не змінився, виводимо додаткову інформацію для парного/непарного дня
                else
                {
                    if (currentTime.Day % 2 == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Сагайдачний дає прочухана зомбі\n");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Сьогодні Cагайдачний відпочиває\n");
                        Console.ResetColor();
                    }

                }
                
            }
        }
        public string GetCurrentTime()
        {
            return currentTime.ToString("HH:mm");
        }

        // Повідомляє чи зараз день чи ніч
        public string GetSunPosition()
        {
            // Час початку дня о 7:00 та час початку ночі о 21:00
            DateTime sunriseTime = currentTime.Date.AddHours(7); 
            DateTime sunsetTime = currentTime.Date.AddHours(21);  

            if (currentTime >= sunriseTime && currentTime < sunsetTime)
            {
                return "День";
            }
            else
            {
                return "Ніч";
            }
        }
    }
}
