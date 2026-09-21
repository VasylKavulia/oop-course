namespace ClinicApp;

public class Doctor
{
    private static int _nextId = 1;

    public int Id { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Speciality { get; set; }
    public string LicenseNumber { get; set; }
    public string Phone { get; set; }
    // години роботи 0-23, графік можна міняти після створення
    public int WorkStartHour { get; set; }
    public int WorkEndHour { get; set; }

    public string FullName => FirstName + " " + LastName;

    public int WorkingHoursPerDay => WorkEndHour - WorkStartHour;

    // D2 - щоб було 08:00, а не 8:00
    public string WorkSchedule => $"{WorkStartHour:D2}:00–{WorkEndHour:D2}:00";

    public bool IsAvailableNow => CanAcceptAt(DateTime.Now.Hour);

    public Doctor() : this("Невідомий", "Лікар", "Терапія")
    {
    }

    public Doctor(string firstName, string lastName, string speciality)
        : this(firstName, lastName, speciality, "LIC-000", "0000000000")
    {
    }

    public Doctor(string firstName, string lastName, string speciality, string licenseNumber, string phone)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        Speciality = speciality;
        LicenseNumber = licenseNumber;
        Phone = phone;
        // за замовчуванням лікар працює з 8 до 17
        WorkStartHour = 8;
        WorkEndHour = 17;
    }

    // година в межах [початок, кінець) - кінець не включається
    public bool CanAcceptAt(int hour)
    {
        return hour >= WorkStartHour && hour < WorkEndHour;
    }

    public override string ToString()
    {
        string status = IsAvailableNow ? "доступний зараз" : "не в робочий час";
        return $"[{Id}] {FullName} | {Speciality} | {LicenseNumber} | Тел: {Phone} | {WorkSchedule} ({WorkingHoursPerDay} год) | {status}";
    }
}
