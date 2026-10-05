using System.Text.RegularExpressions;

namespace ClinicApp.Utils;

// правила, які повторюються в кількох класах, зібрані в одному місці
// static, бо власного стану немає - тільки перевірки (як ClinicFormatter)
public static class ClinicValidator
{
    // static readonly - шаблон розбирається один раз, а не при кожній перевірці
    // [0-9] а не \d: \d пропускає цифри інших алфавітів; \z а не $: $ пропускає \n у кінці
    private static readonly Regex _phoneRegex = new Regex(@"^[0-9]{10}\z");
    // спрощена перевірка: щось@щось.щось без пробілів і без другого @
    private static readonly Regex _emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+\z");

    // ім'я або прізвище: не порожнє і не довше 50 символів
    public static void ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"Поле {fieldName} не може бути порожнім.", fieldName);
        if (value.Length > 50)
            throw new ArgumentException($"Поле {fieldName} не може бути довшим за 50 символів.", fieldName);
    }

    // телефон: рівно 10 цифр (замість циклу по символах - Regex)
    public static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Телефон не може бути порожнім.", nameof(phone));
        if (!_phoneRegex.IsMatch(phone))
            throw new ArgumentException("Телефон має складатися рівно з 10 цифр, без інших символів.", nameof(phone));
    }

    public static void ValidateEmail(string email)
    {
        if (!_emailRegex.IsMatch(email))
            throw new ArgumentException("Некоректний email: " + email, nameof(email));
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
