namespace ClinicApp.Utils;

// правила, які повторюються в кількох класах, зібрані в одному місці
// static, бо власного стану немає - тільки перевірки (як ClinicFormatter)
public static class ClinicValidator
{
    // ім'я або прізвище: не порожнє і не довше 50 символів
    public static void ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"Поле {fieldName} не може бути порожнім.", fieldName);
        if (value.Length > 50)
            throw new ArgumentException($"Поле {fieldName} не може бути довшим за 50 символів.", fieldName);
    }

    // телефон: рівно 10 символів і всі - цифри
    public static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Телефон не може бути порожнім.", nameof(phone));
        if (phone.Length != 10)
            throw new ArgumentException("Телефон має складатися рівно з 10 цифр.", nameof(phone));

        for (int i = 0; i < phone.Length; i++)
        {
            if (phone[i] < '0' || phone[i] > '9')
                throw new ArgumentException("Телефон може містити лише цифри.", nameof(phone));
        }
    }

    // дата: не в майбутньому і не раніше 1900 року
    public static void ValidateDate(DateTime value, string fieldName)
    {
        if (value.Date > DateTime.Today)
            throw new ArgumentOutOfRangeException(fieldName, $"Поле {fieldName} не може бути в майбутньому.");
        if (value.Year < 1900)
            throw new ArgumentOutOfRangeException(fieldName, $"Поле {fieldName} не може бути раніше 1900 року.");
    }

    public static void ValidatePositive(int value, string fieldName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(fieldName, $"Поле {fieldName} має бути більшим за 0.");
    }
}
