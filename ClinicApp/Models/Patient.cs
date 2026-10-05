using ClinicApp.Enums;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Patient
{
    // лічильник спільний для всіх пацієнтів, тому static
    private static int _nextId = 1;

    // приватні поля - справжнє сховище даних, ззовні доступ тільки через властивості
    private string _firstName = "";
    private string _lastName = "";
    private DateTime _dateOfBirth;
    private string _phone = "";

    public int Id { get; }

    // спочатку перевірка, потім присвоєння - некоректне значення в поле не потрапить
    public string FirstName
    {
        get => _firstName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Ім'я не може бути порожнім.", nameof(FirstName));
            if (value.Length > 50)
                throw new ArgumentException("Ім'я не може бути довшим за 50 символів.", nameof(FirstName));
            _firstName = value;
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Прізвище не може бути порожнім.", nameof(LastName));
            if (value.Length > 50)
                throw new ArgumentException("Прізвище не може бути довшим за 50 символів.", nameof(LastName));
            _lastName = value;
        }
    }

    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if (value.Date > DateTime.Today)
                throw new ArgumentOutOfRangeException(nameof(DateOfBirth), "Дата народження не може бути в майбутньому.");
            if (value.Year < 1900)
                throw new ArgumentOutOfRangeException(nameof(DateOfBirth), "Дата народження не може бути раніше 1900 року.");
            _dateOfBirth = value;
        }
    }

    public BloodType BloodType { get; set; }

    public string Phone
    {
        get => _phone;
        set
        {
            if (value.Length != 10)
                throw new ArgumentException("Телефон має складатися рівно з 10 цифр.", nameof(Phone));
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] < '0' || value[i] > '9')
                    throw new ArgumentException("Телефон може містити лише цифри.", nameof(Phone));
            }
            _phone = value;
        }
    }

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
    // поля присвоюються через властивості, тому перевірки спрацьовують і тут
    public Patient(string firstName, string lastName, DateTime dateOfBirth, BloodType bloodType, string phone)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        BloodType = bloodType;
        Phone = phone;
        Email = "";
        // Id останнім: якщо якась перевірка кинула виняток, номер не пропаде
        Id = _nextId++;
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
        return $"[{Id}] {FullName} | Вік: {ClinicFormatter.FormatAge(Age)} ({GetAgeCategory()}) | Кров: {ClinicFormatter.FormatBloodType(BloodType)} | Тел: {ClinicFormatter.FormatPhone(Phone)}";
    }
}
