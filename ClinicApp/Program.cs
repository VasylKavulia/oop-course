using System.Globalization;
using System.Text;
using ClinicApp;

// без цього кирилиця в консолі виводиться і читається неправильно
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;
// дробові числа з крапкою незалежно від локалі Windows (як у Lab01)
Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

// вся робота йде через один об'єкт клініки
Clinic clinic = new Clinic("Медична Клініка");

// тестові дані - пацієнти створені всіма трьома конструкторами
clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 3, 15), BloodType.APositive, "0501234567"));
clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1992, 7, 22), BloodType.BNegative, "0672345678"));
clinic.Patients.Add(new Patient("Максим", "Бойко", new DateTime(2010, 1, 30), BloodType.OPositive, "0933456789"));
clinic.Patients.Add(new Patient());
clinic.Patients.Add(new Patient("Марія", "Ткач"));

Doctor d1 = new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567");
d1.Schedule = new WorkSchedule(8, 16);
Doctor d2 = new Doctor("Наталія", "Мороз", Speciality.Neurology, "LIC-002", "0442345678");
d2.Schedule = new WorkSchedule(9, 18);
clinic.Doctors.Add(d1);
clinic.Doctors.Add(d2);
clinic.Doctors.Add(new Doctor("Андрій", "Власенко", Speciality.Pediatrics, "LIC-003", "0443456789"));

DateTime tomorrow = DateTime.Today.AddDays(1);
clinic.Appointments.Book(1, 1, tomorrow.AddHours(10));
clinic.Appointments.Book(2, 2, tomorrow.AddHours(11), 45);
clinic.Appointments.Book(3, 3, tomorrow.AddDays(1).AddHours(9), 20);

bool running = true;
while (running)
{
    Console.WriteLine();
    Console.WriteLine("=== " + clinic.Name + " ===");
    Console.WriteLine("1. Пацієнти");
    Console.WriteLine("2. Лікарі");
    Console.WriteLine("3. Записи");
    Console.WriteLine("4. Розклад на дату");
    Console.WriteLine("5. Звіт");
    Console.WriteLine("6. Тест зростаючого масиву");
    Console.WriteLine("7. Тест WorkSchedule (struct)");
    Console.WriteLine("0. Вихід");
    Console.Write("Оберіть: ");
    string choice = Console.ReadLine()!;

    switch (choice)
    {
        case "1":
            PatientsMenu(clinic);
            break;
        case "2":
            DoctorsMenu(clinic);
            break;
        case "3":
            AppointmentsMenu(clinic);
            break;
        case "4":
            DateTime date = ReadDate("Дата:");
            clinic.DisplaySchedule(date);
            break;
        case "5":
            clinic.GenerateReport();
            break;
        case "6":
            TestGrowableManager();
            break;
        case "7":
            TestWorkSchedule();
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
static void PatientsMenu(Clinic clinic)
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
                clinic.Patients.DisplayAll();
                break;
            case "2":
                AddPatient(clinic);
                break;
            case "3":
                Console.Write("Ім'я або прізвище: ");
                string query = Console.ReadLine()!;
                Patient[] found = clinic.Patients.FindByName(query);
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
                if (clinic.Patients.Remove(id))
                    Console.WriteLine("Пацієнта видалено.");
                else
                    Console.WriteLine("Пацієнта з ID " + id + " не знайдено.");
                break;
            case "5":
                clinic.Patients.DisplayStats();
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

static void AddPatient(Clinic clinic)
{
    Console.Write("Ім'я: ");
    string firstName = Console.ReadLine()!;
    Console.Write("Прізвище: ");
    string lastName = Console.ReadLine()!;
    DateTime dob = ReadDate("Дата народження:");

    // користувач вводить номер, а ми приводимо його до enum
    Console.WriteLine("Група крові:");
    for (int i = 0; i <= 8; i++)
        Console.WriteLine("  " + i + ". " + (BloodType)i);
    BloodType bloodType = (BloodType)ReadInt("Номер: ");

    Console.Write("Телефон: ");
    string phone = Console.ReadLine()!;

    clinic.Patients.Add(new Patient(firstName, lastName, dob, bloodType, phone));
}

static void DoctorsMenu(Clinic clinic)
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
                clinic.Doctors.DisplayAll();
                break;
            case "2":
                AddDoctor(clinic);
                break;
            case "3":
                Console.Write("Спеціальність: ");
                string query = Console.ReadLine()!;
                Doctor[] found = clinic.Doctors.FindBySpeciality(query);
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
                Doctor[] all = clinic.Doctors.GetAll();
                int available = 0;
                for (int i = 0; i < all.Length; i++)
                {
                    if (all[i].CanAcceptAt(hour))
                    {
                        Console.WriteLine("  " + all[i].FullName + " - " + all[i].Schedule.Display);
                        available++;
                    }
                }
                if (available == 0)
                    Console.WriteLine("О " + hour + ":00 ніхто не приймає.");
                break;
            case "5":
                int id = ReadInt("ID лікаря: ");
                if (clinic.Doctors.Remove(id))
                    Console.WriteLine("Лікаря видалено.");
                else
                    Console.WriteLine("Лікаря з ID " + id + " не знайдено.");
                break;
            case "6":
                clinic.Doctors.DisplayStats();
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

static void AddDoctor(Clinic clinic)
{
    Console.Write("Ім'я: ");
    string firstName = Console.ReadLine()!;
    Console.Write("Прізвище: ");
    string lastName = Console.ReadLine()!;
    Console.WriteLine("Спеціальність:");
    for (int i = 0; i <= 7; i++)
        Console.WriteLine("  " + i + ". " + (Speciality)i);
    Speciality speciality = (Speciality)ReadInt("Номер: ");
    Console.Write("Номер ліцензії: ");
    string license = Console.ReadLine()!;
    Console.Write("Телефон: ");
    string phone = Console.ReadLine()!;
    int start = ReadInt("Початок роботи (година): ");
    int end = ReadInt("Кінець роботи (година): ");

    Doctor doctor = new Doctor(firstName, lastName, speciality, license, phone);
    doctor.Schedule = new WorkSchedule(start, end);
    clinic.Doctors.Add(doctor);
}

static void AppointmentsMenu(Clinic clinic)
{
    bool back = false;
    while (!back)
    {
        Console.WriteLine();
        Console.WriteLine("--- Записи ---");
        Console.WriteLine("1. Майбутні записи");
        Console.WriteLine("2. Створити запис");
        Console.WriteLine("3. Скасувати запис");
        Console.WriteLine("4. Завершити запис");
        Console.WriteLine("5. Записи пацієнта");
        Console.WriteLine("6. Записи лікаря");
        Console.WriteLine("7. Записи на дату");
        Console.WriteLine("0. Назад");
        Console.Write("Оберіть: ");
        string choice = Console.ReadLine()!;

        switch (choice)
        {
            case "1":
                Console.WriteLine("Майбутні записи:");
                clinic.Appointments.DisplayList(clinic.Appointments.GetUpcoming());
                break;
            case "2":
                BookAppointment(clinic);
                break;
            case "3":
                int cancelId = ReadInt("ID запису: ");
                Console.Write("Причина (можна пропустити): ");
                string reason = Console.ReadLine()!;
                if (clinic.Appointments.Cancel(cancelId, reason))
                    Console.WriteLine("Запис [" + cancelId + "] скасовано.");
                else
                    Console.WriteLine("Не вдалося скасувати: запису немає або він уже не запланований.");
                break;
            case "4":
                int completeId = ReadInt("ID запису: ");
                if (clinic.Appointments.Complete(completeId))
                    Console.WriteLine("Запис [" + completeId + "] завершено.");
                else
                    Console.WriteLine("Не вдалося завершити: запису немає або він уже не запланований.");
                break;
            case "5":
                int patientId = ReadInt("ID пацієнта: ");
                Console.WriteLine("Записи пацієнта #" + patientId + ":");
                clinic.Appointments.DisplayList(clinic.Appointments.GetByPatient(patientId));
                break;
            case "6":
                int doctorId = ReadInt("ID лікаря: ");
                Console.WriteLine("Записи лікаря #" + doctorId + ":");
                clinic.Appointments.DisplayList(clinic.Appointments.GetByDoctor(doctorId));
                break;
            case "7":
                DateTime date = ReadDate("Дата:");
                clinic.Appointments.DisplayList(clinic.Appointments.GetByDate(date));
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

static void BookAppointment(Clinic clinic)
{
    // спочатку показуємо списки, щоб було видно доступні Id
    clinic.Patients.DisplayAll();
    clinic.Doctors.DisplayAll();

    int patientId = ReadInt("ID пацієнта: ");
    int doctorId = ReadInt("ID лікаря: ");
    DateTime date = ReadDate("Дата прийому:");
    int hour = ReadInt("Година (0-23): ");
    int minute = ReadInt("Хвилини: ");
    DateTime scheduledAt = date.AddHours(hour).AddMinutes(minute);
    int duration = ReadInt("Тривалість (хв): ");

    clinic.Appointments.Book(patientId, doctorId, scheduledAt, duration);
}

// Задача 8 - перевіряємо, як масив росте сам
static void TestGrowableManager()
{
    Console.WriteLine("=== Тест GrowablePatientManager ===");
    GrowablePatientManager manager = new GrowablePatientManager();

    Console.WriteLine("Додаємо пацієнтів одного за одним...");
    for (int i = 1; i <= 20; i++)
    {
        Patient patient = new Patient("Тест", "Пацієнт" + i);
        manager.Add(patient);
        Console.WriteLine($"  Додано [{patient.Id}]. Розмір: {manager.Count} / {manager.Capacity}");
    }

    Console.WriteLine();
    Console.WriteLine("Тест пошуку:");
    Patient? found = manager.FindById(10);
    if (found != null)
        Console.WriteLine("  FindById(10) → " + found.FullName);
    else
        Console.WriteLine("  FindById(10) → не знайдено");

    Patient? missing = manager.FindById(99);
    if (missing != null)
        Console.WriteLine("  FindById(99) → " + missing.FullName);
    else
        Console.WriteLine("  FindById(99) → не знайдено");

    Console.WriteLine();
    Console.WriteLine("Порівняння:");
    Console.WriteLine("  PatientManager:         100 місць (фіксовано)");
    Console.WriteLine($"  GrowablePatientManager:  {manager.Capacity} місця (зросте при потребі)");
}

// Задача 2 - struct копіюється за значенням, а клас - за посиланням
static void TestWorkSchedule()
{
    Console.WriteLine("=== Тест WorkSchedule ===");
    WorkSchedule morning = new WorkSchedule(8, 16);
    WorkSchedule evening = new WorkSchedule(14, 22);

    Console.WriteLine("Ранкова зміна: " + morning);
    Console.WriteLine("Вечірня зміна: " + evening);
    Console.WriteLine("Ранкова працює зараз: " + morning.IsNow);
    Console.WriteLine("Вечірня працює о 15:00: " + evening.Contains(15));
    Console.WriteLine("Вечірня працює о 22:00: " + evening.Contains(22));

    // copy отримує власну копію значення morning
    // copy.Start = 10; - не скомпілюється, бо Start тільки для читання
    WorkSchedule copy = morning;
    copy = new WorkSchedule(10, 18);
    Console.WriteLine();
    Console.WriteLine("Після copy = morning, а потім copy = new WorkSchedule(10, 18):");
    Console.WriteLine("  morning: " + morning);
    Console.WriteLine("  copy:    " + copy);

    // для порівняння клас: обидві змінні вказують на один і той самий об'єкт
    Patient original = new Patient("Тест", "Оригінал");
    Patient sameObject = original;
    sameObject.FirstName = "Змінений";
    Console.WriteLine();
    Console.WriteLine("Клас Patient: sameObject = original, потім sameObject.FirstName = \"Змінений\":");
    Console.WriteLine("  original.FullName: " + original.FullName);
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
