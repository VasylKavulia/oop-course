using System.Globalization;
using System.Text;
using ClinicApp;

Console.OutputEncoding = Encoding.UTF8;
// дробові числа з крапкою незалежно від локалі Windows (як у Lab01)
Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

// Задача 1 - пацієнти, створені трьома різними конструкторами
Patient p1 = new Patient("Іван", "Петренко", new DateTime(1985, 3, 15), "A+", "0501234567");
Patient p2 = new Patient("Олена", "Коваль", new DateTime(1992, 7, 22), "B-", "0672345678");
Patient p3 = new Patient("Максим", "Бойко", new DateTime(2010, 1, 30), "O+", "0933456789");
Patient p4 = new Patient();
Patient p5 = new Patient("Марія", "Ткач");

Console.WriteLine(p1);
Console.WriteLine(p2);
Console.WriteLine(p3);
Console.WriteLine(p4);
Console.WriteLine(p5);

// Задача 2 - лікарі та їх доступність зараз
Console.WriteLine();
Doctor d1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
d1.WorkEndHour = 16;
Doctor d2 = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
d2.WorkStartHour = 9;
d2.WorkEndHour = 18;
Doctor d3 = new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789");
Doctor d4 = new Doctor("Ірина", "Шевчук", "Терапія");

Console.WriteLine(d1);
Console.WriteLine(d2);
Console.WriteLine(d3);
Console.WriteLine(d4);

Console.WriteLine();
Console.WriteLine("Доступні зараз (" + DateTime.Now.Hour + ":00):");
Doctor[] doctors = { d1, d2, d3, d4 };
for (int i = 0; i < doctors.Length; i++)
{
    if (doctors[i].IsAvailableNow)
        Console.WriteLine("  " + doctors[i].FullName + " - " + doctors[i].WorkSchedule);
}
Console.WriteLine("Чи приймає " + d1.FullName + " о 15:00? " + (d1.CanAcceptAt(15) ? "так" : "ні"));
Console.WriteLine("Чи приймає " + d1.FullName + " о 16:00? " + (d1.CanAcceptAt(16) ? "так" : "ні"));
