using ClinicApp.Utils;

namespace ClinicApp.Models;

// діагноз є різновидом медичного запису - тому успадковуємо MedicalRecord
public class Diagnosis : MedicalRecord
{
    private string _diagnosisCode = "";
    private string _description = "";

    public string DiagnosisCode
    {
        get => _diagnosisCode;
        set
        {
            // ValidateName перевіряє "не порожньо і не довше 50 символів" - для коду теж підходить
            ClinicValidator.ValidateName(value, nameof(DiagnosisCode));
            _diagnosisCode = value;
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            ClinicValidator.ValidateName(value, nameof(Description));
            _description = value;
        }
    }

    public bool IsChronic { get; set; }

    // спочатку повністю відпрацьовує конструктор MedicalRecord, потім тіло цього
    public Diagnosis(int patientId, int doctorId, DateTime date, string diagnosisCode, string description, bool isChronic = false)
        : base(patientId, doctorId, date)
    {
        DiagnosisCode = diagnosisCode;
        Description = description;
        IsChronic = isChronic;
    }

    public override string GetSummary()
    {
        string result = DiagnosisCode + ": " + Description;
        if (IsChronic)
            result += " [хронічне]";
        return result;
    }

    public override string GetRecordType()
    {
        return "Діагноз";
    }
}
