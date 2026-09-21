using System.Globalization;
using System.Text;
using ClinicApp;

// без цього кирилиця в консолі виводиться і читається неправильно
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;
// дробові числа з крапкою незалежно від локалі Windows (як у Lab01)
Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

// тестові дані
PatientManager patients = new PatientManager();
patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 3, 15), "A+", "0501234567"));
patients.Add(new Patient("Олена", "Коваль", new DateTime(1992, 7, 22), "B-", "0672345678"));
patients.Add(new Patient("Максим", "Бойко", new DateTime(2010, 1, 30), "O+", "0933456789"));
patients.Add(new Patient("Марія", "Ткач"));

DoctorManager doctors = new DoctorManager();
Doctor d1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
d1.WorkEndHour = 16;
Doctor d2 = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
d2.WorkStartHour = 9;
d2.WorkEndHour = 18;
doctors.Add(d1);
doctors.Add(d2);
doctors.Add(new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789"));

// Задача 5 - записи на прийом (поки що тільки з Id, імена будуть у менеджері)
Console.WriteLine();
DateTime tomorrow = DateTime.Today.AddDays(1);
Appointment a1 = new Appointment(1, 1, tomorrow.AddHours(10));
Appointment a2 = new Appointment(2, 2, tomorrow.AddHours(11), 45);
Appointment a3 = new Appointment(3, 3, tomorrow.AddDays(1).AddHours(9), 20);
Console.WriteLine(a1);
Console.WriteLine(a2);
Console.WriteLine(a3);

Console.WriteLine();
Console.WriteLine("Після Cancel та Complete:");
a1.Cancel("Пацієнт не зміг прийти");
a2.Complete();
Console.WriteLine(a1);
Console.WriteLine(a2);
// повторний перехід має повернути false
Console.WriteLine("Скасувати [1] ще раз: " + (a1.Cancel() ? "вдалося" : "не вдалося, статус " + a1.Status));
Console.WriteLine("Завершити [2] ще раз: " + (a2.Complete() ? "вдалося" : "не вдалося, статус " + a2.Status));
Console.WriteLine("Запис [3] майбутній? " + (a3.IsUpcoming ? "так" : "ні"));

bool running = true;
while (running)
{
    Console.WriteLine();
    Console.WriteLine("=== Медична клініка ===");
    Console.WriteLine("1. Пацієнти");
    Console.WriteLine("2. Лікарі");
    Console.WriteLine("0. Вихід");
    Console.Write("Оберіть: ");
    string choice = Console.ReadLine()!;

    switch (choice)
    {
        case "1":
            PatientsMenu(patients);
            break;
        case "2":
            DoctorsMenu(doctors);
            break;
        case "0":
            running = false;
            break;
        default:
            Console.WriteLine("Невідомий пункт меню.");
            break;
    }
}

// підменю "Пацієнти" - окрема функція, щоб не роздувати головний switch
static void PatientsMenu(PatientManager patients)
{
    bool back = false;
    while (!back)
    {
        Console.WriteLine();
        Console.WriteLine("--- Пацієнти ---");
        Console.WriteLine("1. Показати всіх");
        Console.WriteLine("2. Додати пацієнта");
        Console.WriteLine("3. Знайти за ім'ям");
        Console.WriteLine("4. Видалити");
        Console.WriteLine("5. Статистика");
        Console.WriteLine("0. Назад");
        Console.Write("Оберіть: ");
        string choice = Console.ReadLine()!;

        switch (choice)
        {
            case "1":
                patients.DisplayAll();
                break;
            case "2":
                AddPatient(patients);
                break;
            case "3":
                Console.Write("Ім'я або прізвище: ");
                string query = Console.ReadLine()!;
                Patient[] found = patients.FindByName(query);
                if (found.Length == 0)
                {
                    Console.WriteLine("Нічого не знайдено.");
                }
                else
                {
                    for (int i = 0; i < found.Length; i++)
                        Console.WriteLine(found[i]);
                }
                break;
            case "4":
                int id = ReadInt("ID пацієнта: ");
                if (patients.Remove(id))
                    Console.WriteLine("Пацієнта видалено.");
                else
                    Console.WriteLine("Пацієнта з ID " + id + " не знайдено.");
                break;
            case "5":
                patients.DisplayStats();
                break;
            case "0":
                back = true;
                break;
            default:
                Console.WriteLine("Невідомий пункт меню.");
                break;
        }
    }
}

static void AddPatient(PatientManager patients)
{
    Console.Write("Ім'я: ");
    string firstName = Console.ReadLine()!;
    Console.Write("Прізвище: ");
    string lastName = Console.ReadLine()!;
    DateTime dob = ReadDate("Дата народження:");
    Console.Write("Група крові: ");
    string bloodType = Console.ReadLine()!;
    Console.Write("Телефон: ");
    string phone = Console.ReadLine()!;

    patients.Add(new Patient(firstName, lastName, dob, bloodType, phone));
}

static void DoctorsMenu(DoctorManager doctors)
{
    bool back = false;
    while (!back)
    {
        Console.WriteLine();
        Console.WriteLine("--- Лікарі ---");
        Console.WriteLine("1. Показати всіх");
        Console.WriteLine("2. Додати лікаря");
        Console.WriteLine("3. Знайти за спеціальністю");
        Console.WriteLine("4. Хто приймає о годині");
        Console.WriteLine("5. Видалити");
        Console.WriteLine("6. Статистика");
        Console.WriteLine("0. Назад");
        Console.Write("Оберіть: ");
        string choice = Console.ReadLine()!;

        switch (choice)
        {
            case "1":
                doctors.DisplayAll();
                break;
            case "2":
                AddDoctor(doctors);
                break;
            case "3":
                Console.Write("Спеціальність: ");
                string query = Console.ReadLine()!;
                Doctor[] found = doctors.FindBySpeciality(query);
                if (found.Length == 0)
                {
                    Console.WriteLine("Нічого не знайдено.");
                }
                else
                {
                    for (int i = 0; i < found.Length; i++)
                        Console.WriteLine(found[i]);
                }
                break;
            case "4":
                int hour = ReadInt("Година (0-23): ");
                Doctor[] all = doctors.GetAll();
                int available = 0;
                for (int i = 0; i < all.Length; i++)
                {
                    if (all[i].CanAcceptAt(hour))
                    {
                        Console.WriteLine("  " + all[i].FullName + " - " + all[i].WorkSchedule);
                        available++;
                    }
                }
                if (available == 0)
                    Console.WriteLine("О " + hour + ":00 ніхто не приймає.");
                break;
            case "5":
                int id = ReadInt("ID лікаря: ");
                if (doctors.Remove(id))
                    Console.WriteLine("Лікаря видалено.");
                else
                    Console.WriteLine("Лікаря з ID " + id + " не знайдено.");
                break;
            case "6":
                doctors.DisplayStats();
                break;
            case "0":
                back = true;
                break;
            default:
                Console.WriteLine("Невідомий пункт меню.");
                break;
        }
    }
}

static void AddDoctor(DoctorManager doctors)
{
    Console.Write("Ім'я: ");
    string firstName = Console.ReadLine()!;
    Console.Write("Прізвище: ");
    string lastName = Console.ReadLine()!;
    Console.Write("Спеціальність: ");
    string speciality = Console.ReadLine()!;
    Console.Write("Номер ліцензії: ");
    string license = Console.ReadLine()!;
    Console.Write("Телефон: ");
    string phone = Console.ReadLine()!;
    int start = ReadInt("Початок роботи (година): ");
    int end = ReadInt("Кінець роботи (година): ");

    Doctor doctor = new Doctor(firstName, lastName, speciality, license, phone);
    doctor.WorkStartHour = start;
    doctor.WorkEndHour = end;
    doctors.Add(doctor);
}

// читає ціле число, поки користувач не введе коректне
static int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        int value;
        if (int.TryParse(Console.ReadLine(), out value))
            return value;
        Console.WriteLine("Введіть ціле число.");
    }
}

// дату питаємо трьома числами, щоб не залежати від формату вводу
static DateTime ReadDate(string label)
{
    Console.WriteLine(label);
    while (true)
    {
        int day = ReadInt("  День: ");
        int month = ReadInt("  Місяць: ");
        int year = ReadInt("  Рік: ");
        if (year >= 1900 && month >= 1 && month <= 12 && day >= 1 && day <= DateTime.DaysInMonth(year, month))
            return new DateTime(year, month, day);
        Console.WriteLine("Такої дати не існує, спробуйте ще раз.");
    }
}
