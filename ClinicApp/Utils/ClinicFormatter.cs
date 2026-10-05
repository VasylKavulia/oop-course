using ClinicApp.Enums;

namespace ClinicApp.Utils;

// static клас - об'єкт створити не можна, тільки викликати методи через ім'я класу
public static class ClinicFormatter
{
    public static string FormatBloodType(BloodType bt)
    {
        return bt switch
        {
            BloodType.Unknown => "Невідомо",
            BloodType.APositive => "A+",
            BloodType.ANegative => "A-",
            BloodType.BPositive => "B+",
            BloodType.BNegative => "B-",
            BloodType.ABPositive => "AB+",
            BloodType.ABNegative => "AB-",
            BloodType.OPositive => "O+",
            BloodType.ONegative => "O-",
            _ => "Невідомо"
        };
    }

    public static string FormatSpeciality(Speciality s)
    {
        return s switch
        {
            Speciality.General => "Терапія",
            Speciality.Cardiology => "Кардіологія",
            Speciality.Neurology => "Неврологія",
            Speciality.Pediatrics => "Педіатрія",
            Speciality.Surgery => "Хірургія",
            Speciality.Orthopedics => "Ортопедія",
            Speciality.Dermatology => "Дерматологія",
            Speciality.Emergency => "Невідкладна допомога",
            _ => "Невідомо"
        };
    }

    // 1 рік, 3 роки, 11 років, 21 рік
    public static string FormatAge(int age)
    {
        int lastTwoDigits = age % 100;
        int lastDigit = age % 10;

        // 11-19 - виняток, там завжди "років"
        if (lastTwoDigits >= 11 && lastTwoDigits <= 19)
            return age + " років";
        if (lastDigit == 1)
            return age + " рік";
        if (lastDigit >= 2 && lastDigit <= 4)
            return age + " роки";
        return age + " років";
    }

    // 0501234567 -> (050) 123-4567, якщо формат інший - повертаємо як є
    public static string FormatPhone(string phone)
    {
        if (phone.Length != 10)
            return phone;

        for (int i = 0; i < phone.Length; i++)
        {
            if (!char.IsDigit(phone[i]))
                return phone;
        }

        return "(" + phone.Substring(0, 3) + ") " + phone.Substring(3, 3) + "-" + phone.Substring(6);
    }
}
