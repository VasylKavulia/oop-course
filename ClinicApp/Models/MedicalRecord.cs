using ClinicApp.Utils;

namespace ClinicApp.Models;

// спільне для всіх видів медичних записів
// abstract - "запису взагалі" не буває, створюються тільки конкретні нащадки
public abstract class MedicalRecord
{
    private static int _nextId = 1;

    public int Id { get; }
    public int PatientId { get; }
    public int DoctorId { get; }
    public DateTime Date { get; }
    public string Notes { get; set; }

    // protected - викликати можуть тільки нащадки через base(...)
    protected MedicalRecord(int patientId, int doctorId, DateTime date)
    {
        ClinicValidator.ValidatePositive(patientId, nameof(patientId));
        ClinicValidator.ValidatePositive(doctorId, nameof(doctorId));

        PatientId = patientId;
        DoctorId = doctorId;
        Date = date;
        Notes = "";
        // Id останнім, як у Лабі 05
        Id = _nextId++;
    }

    // без тіла - кожен нащадок зобов'язаний описати свій зміст
    public abstract string GetSummary();

    // з тілом за замовчуванням - нащадок може замінити, а може й ні
    public virtual string GetRecordType()
    {
        return "Медичний запис";
    }

    // запис активний, якщо йому не більше 6 місяців (рівно пів року - ще активний)
    public virtual bool IsActive()
    {
        return Date >= DateTime.Today.AddMonths(-6);
    }

    // GetRecordType і GetSummary тут викличуться ті, що в реального нащадка
    public override string ToString()
    {
        string result = $"[{Id}] {GetRecordType()} | {Date:dd.MM.yyyy} | {GetSummary()}";
        if (Notes.Length > 0)
            result += " | " + Notes;
        return result;
    }
}
