namespace ClinicApp.Models;

// struct - при присвоєнні копіюється саме значення, а не посилання
public struct WorkSchedule
{
    // тільки get - після створення розклад змінити не можна
    public int Start { get; }
    public int End { get; }

    public int HoursPerDay => End - Start;

    // D2 - щоб було 08:00, а не 8:00
    public string Display => $"{Start:D2}:00–{End:D2}:00";

    public bool IsNow => Contains(DateTime.Now.Hour);

    // Start і End тільки для читання, тому всі перевірки - тут, до присвоєння
    public WorkSchedule(int start, int end)
    {
        if (start < 0 || start > 23)
            throw new ArgumentOutOfRangeException(nameof(start), "Початок роботи має бути від 0 до 23.");
        if (end < 1 || end > 24)
            throw new ArgumentOutOfRangeException(nameof(end), "Кінець роботи має бути від 1 до 24.");
        if (start >= end)
            throw new ArgumentException("Початок роботи має бути раніше за кінець.");

        Start = start;
        End = end;
    }

    // година в межах [Start, End) - кінець не включається
    public bool Contains(int hour)
    {
        return hour >= Start && hour < End;
    }

    public override string ToString()
    {
        return Display + " (" + HoursPerDay + " год)";
    }
}
