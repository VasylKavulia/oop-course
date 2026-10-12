using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Prescription : MedicalRecord
{
    private string _medicationName = "";
    private string _dosage = "";
    private int _durationDays;

    public string MedicationName
    {
        get => _medicationName;
        set
        {
            ClinicValidator.ValidateName(value, nameof(MedicationName));
            _medicationName = value;
        }
    }

    public string Dosage
    {
        get => _dosage;
        set
        {
            ClinicValidator.ValidateName(value, nameof(Dosage));
            _dosage = value;
        }
    }

    public int DurationDays
    {
        get => _durationDays;
        set
        {
            ClinicValidator.ValidatePositive(value, nameof(DurationDays));
            _durationDays = value;
        }
    }

    // необов'язкове, тому без перевірок
    public string Instructions { get; set; }

    // обчислюється щоразу, окремого поля немає
    public DateTime ExpiresAt => Date.AddDays(DurationDays);

    public Prescription(int patientId, int doctorId, DateTime date, string medicationName, string dosage, int durationDays, string instructions = "")
        : base(patientId, doctorId, date)
    {
        MedicationName = medicationName;
        Dosage = dosage;
        DurationDays = durationDays;
        Instructions = instructions;
    }

    public override string GetSummary()
    {
        string result = $"{MedicationName} {Dosage} × {DurationDays} днів";
        if (Instructions.Length > 0)
            result += " (" + Instructions + ")";
        return result;
    }

    public override string GetRecordType()
    {
        return "Рецепт";
    }

    // у рецепта своє правило: діє, поки не закінчився курс (останній день ще діє)
    // правило "6 місяців" з MedicalRecord тут не підходить
    public override bool IsActive()
    {
        return ExpiresAt >= DateTime.Today;
    }
}
