using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Appointment : IPayable, ICancellable
{
    private static int _nextId = 1;
    private const decimal PricePerMinute = 10m;

    private int _durationMinutes;
    // ззовні змінити можна тільки через MarkPaid()
    private bool _isPaid;
    // 1 - без знижки, 0.8 - VIP (-20%)
    private decimal _discountMultiplier = 1m;

    public int Id { get; }
    // зберігаємо тільки Id пацієнта і лікаря, а не самі об'єкти
    public int PatientId { get; }
    public int DoctorId { get; }
    public DateTime ScheduledAt { get; set; }

    public int DurationMinutes
    {
        get => _durationMinutes;
        set
        {
            ClinicValidator.ValidatePositive(value, nameof(DurationMinutes));
            _durationMinutes = value;
        }
    }

    // Задача 5, варіант А: множник знижки тут, у базовому класі, і його враховує базова ціна.
    // Термінові й консультації рахують від base.GetCost(), тому знижка в них уже є і їх не чіпаємо.
    // VIP-підкласи не підходять: в нащадку UrgentAppointment не можна перевизначити GetDescription
    // (sealed override, CS0239), від SpecialistAppointment успадкуватись не можна взагалі (CS0509).
    // Та й класів забагато: з VIP уже 6, а з дитячим і пенсійним тарифом 3 * 4 = 12.
    public decimal DiscountMultiplier
    {
        get => _discountMultiplier;
        set
        {
            // 0 і менше - безкоштовно або мінус, більше 1 - вже націнка, а не знижка
            if (value <= 0 || value > 1)
                throw new ArgumentOutOfRangeException(nameof(DiscountMultiplier), $"Поле {nameof(DiscountMultiplier)} має бути більшим за 0 і не більшим за 1.");
            _discountMultiplier = value;
        }
    }

    // статус міняється тільки зсередини через Cancel() / Complete()
    public AppointmentStatus Status { get; private set; }
    public string Notes { get; private set; }

    public DateTime EndsAt => ScheduledAt.AddMinutes(DurationMinutes);

    public bool IsUpcoming => ScheduledAt > DateTime.Now && Status == AppointmentStatus.Scheduled;

    public bool IsPaid => _isPaid;

    // без окремого поля, щоб не розійшлося зі Status
    public bool IsCancelled => Status == AppointmentStatus.Cancelled;
    // причину Cancel() записує в Notes
    public string CancellationReason => IsCancelled ? Notes : "";

    public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        PatientId = patientId;
        DoctorId = doctorId;
        ScheduledAt = scheduledAt;
        DurationMinutes = durationMinutes;
        Status = AppointmentStatus.Scheduled;
        Notes = "";
        // Id останнім, щоб невдала спроба не забирала номер
        Id = _nextId++;
    }

    // Scheduled -> Cancelled, з будь-якого іншого стану - не можна
    // він же реалізує Cancel з ICancellable
    public bool Cancel(string reason = "")
    {
        if (Status != AppointmentStatus.Scheduled)
            return false;

        Status = AppointmentStatus.Cancelled;
        if (reason.Length > 0)
            Notes = reason;
        return true;
    }

    // Scheduled -> Completed
    public bool Complete()
    {
        if (Status != AppointmentStatus.Scheduled)
            return false;

        Status = AppointmentStatus.Completed;
        return true;
    }

    // virtual - підклас може змінити ціну; int * decimal дає decimal, приведення не потрібне
    public virtual decimal GetCost()
    {
        return DurationMinutes * PricePerMinute * DiscountMultiplier;
    }

    public virtual string GetDescription()
    {
        return "Прийом";
    }

    // навмисно не virtual - для порівняння new і override у Задачі 3
    public int GetPriority()
    {
        return 3;
    }

    // скасований не оплачуємо; void відмову не поверне, тому просто виходимо
    public void MarkPaid()
    {
        if (IsCancelled)
            return;
        _isPaid = true;
    }

    // GetDescription і GetCost віртуальні, тому для підкласу тут будуть його опис і його ціна
    public override string ToString()
    {
        string result = $"[{Id}] {GetDescription()} | Пацієнт #{PatientId} → Лікар #{DoctorId} | {ScheduledAt:dd.MM.yyyy HH:mm}–{EndsAt:HH:mm} | {Status}";
        if (Notes.Length > 0)
            result += " | " + Notes;
        result += " | " + GetCost().ToString("F2") + " грн";
        return result;
    }
}
