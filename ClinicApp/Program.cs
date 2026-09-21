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
