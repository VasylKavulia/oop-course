using System.Globalization;
using System.Text;
using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Utils;

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
    Console.WriteLine("8. Тест членів класу (Lab04)");
    Console.WriteLine("9. Тест валідації (Lab05)");
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
        case "8":
            TestClassMembers(clinic);
            break;
        case "9":
            TestValidation();
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
        Console.WriteLine("6. Знайти за групою крові");
        Console.WriteLine("7. Знайти за ID");
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
            case "6":
                BloodType bloodType = ReadBloodType();
                Patient[] byBloodType = clinic.Patients.FindByBloodType(bloodType);
                if (byBloodType.Length == 0)
                {
                    Console.WriteLine("Пацієнтів з групою " + ClinicFormatter.FormatBloodType(bloodType) + " немає.");
                }
                else
                {
                    for (int i = 0; i < byBloodType.Length; i++)
                        Console.WriteLine(byBloodType[i]);
                }
                break;
            case "7":
                int searchId = ReadInt("ID пацієнта: ");
                if (clinic.Patients.TryFindById(searchId, out Patient patient))
                    Console.WriteLine(patient);
                else
                    Console.WriteLine("Пацієнта з ID " + searchId + " не знайдено.");
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
    BloodType bloodType = ReadBloodType();
    Console.Write("Телефон: ");
    string phone = Console.ReadLine()!;

    // некоректні дані - Patient кине виняток, ловимо його і повертаємось у меню
    try
    {
        clinic.Patients.Add(new Patient(firstName, lastName, dob, bloodType, phone));
    }
    catch (ArgumentOutOfRangeException e) // спершу конкретніший тип
    {
        Console.WriteLine("Помилка: " + e.Message);
    }
    catch (ArgumentException e)
    {
        Console.WriteLine("Помилка: " + e.Message);
    }
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
        Console.WriteLine("3. Знайти за спеціальністю (текст)");
        Console.WriteLine("4. Хто приймає о годині");
        Console.WriteLine("5. Видалити");
        Console.WriteLine("6. Статистика");
        Console.WriteLine("7. Знайти за спеціальністю (зі списку)");
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
            case "7":
                // тут викликається інша версія FindBySpeciality - з параметром enum
                Speciality speciality = ReadSpeciality();
                Doctor[] bySpeciality = clinic.Doctors.FindBySpeciality(speciality);
                if (bySpeciality.Length == 0)
                {
                    Console.WriteLine("Лікарів цієї спеціальності немає.");
                }
                else
                {
                    for (int i = 0; i < bySpeciality.Length; i++)
                        Console.WriteLine(bySpeciality[i]);
                }
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
    Speciality speciality = ReadSpeciality();
    Console.Write("Номер ліцензії: ");
    string license = Console.ReadLine()!;
    Console.Write("Телефон: ");
    string phone = Console.ReadLine()!;
    int start = ReadInt("Початок роботи (година): ");
    int end = ReadInt("Кінець роботи (година): ");

    // WorkSchedule теж всередині try - інакше години 20 і 6 обвалять програму
    // розклад створюємо першим: якщо години неправильні, лікар не створиться і Id не пропаде
    try
    {
        WorkSchedule schedule = new WorkSchedule(start, end);
        Doctor doctor = new Doctor(firstName, lastName, speciality, license, phone);
        doctor.Schedule = schedule;
        clinic.Doctors.Add(doctor);
    }
    catch (ArgumentOutOfRangeException e)
    {
        Console.WriteLine("Помилка: " + e.Message);
    }
    catch (ArgumentException e)
    {
        Console.WriteLine("Помилка: " + e.Message);
    }
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

    // тривалість <= 0 - Appointment кине ArgumentOutOfRangeException
    try
    {
        clinic.Appointments.Book(patientId, doctorId, scheduledAt, duration);
    }
    catch (ArgumentOutOfRangeException e)
    {
        Console.WriteLine("Помилка: " + e.Message);
    }
}

// Задачі 3-4 - некоректні дані не проходять, а Id не "з'їдається"
static void TestValidation()
{
    Console.WriteLine("=== Тест валідації ===");
    DateTime dob = new DateTime(1990, 5, 15);

    try
    {
        Patient bad = new Patient("", "Петренко", dob, BloodType.APositive, "0501234567");
    }
    catch (ArgumentException e)
    {
        Console.WriteLine("Порожнє ім'я -> " + e.GetType().Name + ": " + e.Message);
    }

    try
    {
        Patient bad = new Patient("Іван", "Петренко", DateTime.Today.AddDays(1), BloodType.APositive, "0501234567");
    }
    catch (ArgumentOutOfRangeException e)
    {
        Console.WriteLine("Народження завтра -> " + e.GetType().Name + ": " + e.Message);
    }

    try
    {
        Patient bad = new Patient("Іван", "Петренко", dob, BloodType.APositive, "050abc4567");
    }
    catch (ArgumentException e)
    {
        Console.WriteLine("Телефон з літерами -> " + e.GetType().Name + ": " + e.Message);
    }

    try
    {
        WorkSchedule bad = new WorkSchedule(20, 6);
    }
    catch (ArgumentException e)
    {
        Console.WriteLine("WorkSchedule(20, 6) -> " + e.GetType().Name + ": " + e.Message);
    }

    // дві невдалі спроби між двома вдалими - номери мають іти підряд
    Console.WriteLine();
    Patient first = new Patient("Тест", "Перший");
    for (int i = 0; i < 2; i++)
    {
        try
        {
            Patient bad = new Patient("", "Невдалий");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine("Невдала спроба " + (i + 1) + ": " + e.Message);
        }
    }
    Patient second = new Patient("Тест", "Другий");
    Console.WriteLine($"Id першого: {first.Id}, Id наступного успішного: {second.Id}");
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

// Задачі 3-4 - індексатори, форматер, перевантаження, out, ?. та ??
static void TestClassMembers(Clinic clinic)
{
    Console.WriteLine("=== Тест членів класу ===");

    Console.WriteLine("Індексатори:");
    Console.WriteLine("  clinic.Patients[0]:   " + clinic.Patients[0]);
    Console.WriteLine("  clinic.Doctors[1]:    " + clinic.Doctors[1]);
    // 999 - за межами списку, індексатор поверне null
    Console.WriteLine("  clinic.Patients[999]: " + (clinic.Patients[999]?.FullName ?? "null"));

    Console.WriteLine();
    Console.WriteLine("ClinicFormatter:");
    Console.WriteLine("  FormatBloodType(APositive) → " + ClinicFormatter.FormatBloodType(BloodType.APositive));
    Console.WriteLine("  FormatSpeciality(Cardiology) → " + ClinicFormatter.FormatSpeciality(Speciality.Cardiology));
    int[] ages = { 1, 3, 11, 21, 111 };
    for (int i = 0; i < ages.Length; i++)
        Console.WriteLine("  FormatAge(" + ages[i] + ") → " + ClinicFormatter.FormatAge(ages[i]));
    Console.WriteLine("  FormatPhone(0501234567) → " + ClinicFormatter.FormatPhone("0501234567"));

    Console.WriteLine();
    Console.WriteLine("Перевантаження:");
    Doctor[] cardiologists = clinic.Doctors.FindBySpeciality(Speciality.Cardiology);
    Console.WriteLine("  FindBySpeciality(Speciality.Cardiology) → знайдено: " + cardiologists.Length);
    Doctor[] found = clinic.Doctors.FindBySpeciality("кардіо");
    Console.WriteLine("  FindBySpeciality(\"кардіо\") → знайдено: " + found.Length);
    DateTime tomorrow = DateTime.Today.AddDays(1);
    Appointment[] byDate = clinic.Appointments.GetByDate(tomorrow.Year, tomorrow.Month, tomorrow.Day);
    Console.WriteLine($"  GetByDate({tomorrow.Year}, {tomorrow.Month}, {tomorrow.Day}) → записів: {byDate.Length}");

    Console.WriteLine();
    Console.WriteLine("TryFindById (out):");
    if (clinic.Patients.TryFindById(3, out Patient patient))
        Console.WriteLine("  Patients.TryFindById(3) → знайдено: " + patient.FullName);
    else
        Console.WriteLine("  Patients.TryFindById(3) → не знайдено");

    if (clinic.Patients.TryFindById(99, out Patient missing))
        Console.WriteLine("  Patients.TryFindById(99) → знайдено: " + missing.FullName);
    else
        Console.WriteLine("  Patients.TryFindById(99) → не знайдено");

    if (clinic.Doctors.TryFindById(2, out Doctor doctor))
        Console.WriteLine("  Doctors.TryFindById(2) → знайдено: " + doctor.FullName);
    else
        Console.WriteLine("  Doctors.TryFindById(2) → не знайдено");

    Console.WriteLine();
    Console.WriteLine("?. та ??:");
    // FindById(99) дає null, ?. не дає впасти, а ?? підставляє запасний текст
    string name = clinic.Patients.FindById(99)?.FullName ?? "не знайдено";
    Console.WriteLine("  Patients.FindById(99)?.FullName ?? \"не знайдено\" → " + name);
    string doctorName = clinic.Doctors.FindById(1)?.FullName ?? "не знайдено";
    Console.WriteLine("  Doctors.FindById(1)?.FullName ?? \"не знайдено\" → " + doctorName);
}

// номер зі списку приводимо до enum (перевірка, чи такий номер існує, - в Lab05)
static BloodType ReadBloodType()
{
    Console.WriteLine("Група крові:");
    for (int i = 0; i <= 8; i++)
        Console.WriteLine("  " + i + ". " + ClinicFormatter.FormatBloodType((BloodType)i));
    return (BloodType)ReadInt("Номер: ");
}

static Speciality ReadSpeciality()
{
    Console.WriteLine("Спеціальність:");
    for (int i = 0; i <= 7; i++)
        Console.WriteLine("  " + i + ". " + ClinicFormatter.FormatSpeciality((Speciality)i));
    return (Speciality)ReadInt("Номер: ");
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
