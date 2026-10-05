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
    private string _email = "";

    public int Id { get; }

    // спочатку перевірка, потім присвоєння - некоректне значення в поле не потрапить
    public string FirstName
    {
        get => _firstName;
        set
        {
            ClinicValidator.ValidateName(value, nameof(FirstName));
            _firstName = value;
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            ClinicValidator.ValidateName(value, nameof(LastName));
            _lastName = value;
        }
    }

    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            ClinicValidator.ValidateDate(value, nameof(DateOfBirth));
            _dateOfBirth = value;
        }
    }

    public BloodType BloodType { get; set; }

    public string Phone
    {
        get => _phone;
        set
        {
            ClinicValidator.ValidatePhone(value);
            _phone = value;
        }
    }

    // порожній рядок - email невідомий, це дозволено; непорожній має пройти перевірку
    public string Email
    {
        get => _email;
        set
        {
            if (value.Length > 0)
                ClinicValidator.ValidateEmail(value);
            _email = value;
        }
    }

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
