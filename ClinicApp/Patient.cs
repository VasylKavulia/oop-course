namespace ClinicApp;

public class Patient
{
    // лічильник спільний для всіх пацієнтів, тому static
    private static int _nextId = 1;

    public int Id { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public BloodType BloodType { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    // обчислювані властивості - нічого не зберігають, рахуються з інших полів
    public string FullName => FirstName + " " + LastName;

    public int Age
    {
        get
        {
            DateTime today = DateTime.Today;
            int age = today.Year - DateOfBirth.Year;
            // якщо день народження в цьому році ще не настав - віднімаємо рік
            if (DateOfBirth.Date > today.AddYears(-age))
                age--;
            return age;
        }
    }

    public bool IsAdult => Age >= 18;

    // невідомий пацієнт - все за замовчуванням
    public Patient() : this("Невідомий", "Пацієнт")
    {
    }

    // тільки ім'я і прізвище, решта за замовчуванням
    public Patient(string firstName, string lastName)
        : this(firstName, lastName, new DateTime(2000, 1, 1), BloodType.Unknown, "0000000000")
    {
    }

    // повний конструктор - тільки тут призначається Id
    public Patient(string firstName, string lastName, DateTime dateOfBirth, BloodType bloodType, string phone)
    {
        Id = _nextId++; // беремо поточне значення і збільшуємо лічильник
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        BloodType = bloodType;
        Phone = phone;
        Email = "";
    }

    public string GetAgeCategory()
    {
        if (Age < 18)
            return "дитина";
        else if (Age < 60)
            return "дорослий";
        else
            return "літній";
    }

    public override string ToString()
    {
        return $"[{Id}] {FullName} | Вік: {Age} ({GetAgeCategory()}) | Кров: {BloodType} | Тел: {Phone}";
    }
}
