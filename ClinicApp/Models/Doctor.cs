using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Doctor : ISchedulable
{
    private static int _nextId = 1;

    private string _firstName = "";
    private string _lastName = "";
    private string _licenseNumber = "";
    private string _phone = "";

    public int Id { get; }

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

    public Speciality Speciality { get; set; }

    // правило для ліцензії тільки одне і тільки тут, тому лишається на місці
    public string LicenseNumber
    {
        get => _licenseNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Номер ліцензії не може бути порожнім.", nameof(LicenseNumber));
            _licenseNumber = value;
        }
    }

    public string Phone
    {
        get => _phone;
        set
        {
            ClinicValidator.ValidatePhone(value);
            _phone = value;
        }
    }

    // замість двох окремих годин - один розклад, графік можна замінити цілком
    public WorkSchedule Schedule { get; set; }

    public string FullName => FirstName + " " + LastName;

    public bool IsAvailableNow => Schedule.IsNow;

    public Doctor() : this("Невідомий", "Лікар", Speciality.General)
    {
    }

    public Doctor(string firstName, string lastName, Speciality speciality)
        : this(firstName, lastName, speciality, "LIC-000", "0000000000")
    {
    }

    public Doctor(string firstName, string lastName, Speciality speciality, string licenseNumber, string phone)
    {
        FirstName = firstName;
        LastName = lastName;
        Speciality = speciality;
        LicenseNumber = licenseNumber;
        Phone = phone;
        // за замовчуванням лікар працює з 8 до 17
        Schedule = new WorkSchedule(8, 17);
        // Id тільки коли всі перевірки пройшли
        Id = _nextId++;
    }

    public bool CanAcceptAt(int hour)
    {
        return Schedule.Contains(hour);
    }

    // правило вже є в CanAcceptAt, третя копія не потрібна
    public bool CanSchedule(DateTime at)
    {
        return CanAcceptAt(at.Hour);
    }

    // години розкладу, а не справді вільні - записів лікар не знає
    public DateTime[] GetAvailableSlots(DateTime date, int slotCount)
    {
        // перевірка першою: з від'ємним розміром масив не створиться (OverflowException)
        ClinicValidator.ValidatePositive(slotCount, nameof(slotCount));

        // не більше, ніж годин у розкладі
        int count = Math.Min(slotCount, Schedule.HoursPerDay);
        DateTime[] slots = new DateTime[count];
        for (int i = 0; i < count; i++)
            slots[i] = date.Date.AddHours(Schedule.Start + i);
        return slots;
    }

    public override string ToString()
    {
        string status = IsAvailableNow ? "доступний зараз" : "не в робочий час";
        return $"[{Id}] {FullName} | {ClinicFormatter.FormatSpeciality(Speciality)} | {LicenseNumber} | Тел: {ClinicFormatter.FormatPhone(Phone)} | {Schedule} | {status}";
    }
}
