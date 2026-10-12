using ClinicApp.Utils;

namespace ClinicApp.Models;

public class LabResult : MedicalRecord
{
    private string _testName = "";
    private string _unit = "";
    private string _referenceRange = "";

    public string TestName
    {
        get => _testName;
        set
        {
            ClinicValidator.ValidateName(value, nameof(TestName));
            _testName = value;
        }
    }

    // просто число, перевіряти нічого
    public double Value { get; set; }

    public string Unit
    {
        get => _unit;
        set
        {
            ClinicValidator.ValidateName(value, nameof(Unit));
            _unit = value;
        }
    }

    // норма текстом ("120–160", "< 5.2"), тому порівняти з Value автоматично не можна
    public string ReferenceRange
    {
        get => _referenceRange;
        set
        {
            ClinicValidator.ValidateName(value, nameof(ReferenceRange));
            _referenceRange = value;
        }
    }

    // в нормі чи ні - вказує той, хто вносить результат
    public bool IsNormal { get; set; }

    public LabResult(int patientId, int doctorId, DateTime date, string testName, double value, string unit, string referenceRange, bool isNormal)
        : base(patientId, doctorId, date)
    {
        TestName = testName;
        Value = value;
        Unit = unit;
        ReferenceRange = referenceRange;
        IsNormal = isNormal;
    }

    public override string GetSummary()
    {
        string result = $"{TestName}: {Value} {Unit} (норма: {ReferenceRange})";
        if (!IsNormal)
            result += " ⚠ поза нормою";
        return result;
    }

    public override string GetRecordType()
    {
        return "Аналіз";
    }
}
