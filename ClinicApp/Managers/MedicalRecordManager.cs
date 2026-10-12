using ClinicApp.Models;

namespace ClinicApp.Managers;

public class MedicalRecordManager
{
    private const int MaxRecords = 1000;

    // поліморфний масив: в одному масиві лежать і діагнози, і аналізи, і рецепти
    private MedicalRecord[] _records = new MedicalRecord[MaxRecords];
    private int _count = 0;

    public int Count => _count;

    public MedicalRecord? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
                return null;
            return _records[index];
        }
    }

    public void Add(MedicalRecord record)
    {
        if (_count >= MaxRecords)
        {
            Console.WriteLine("Неможливо додати: досягнуто ліміт " + MaxRecords + " записів.");
            return;
        }

        _records[_count] = record;
        _count++;
        Console.WriteLine($"Запис [{record.Id}] {record.GetRecordType()} додано.");
    }

    public MedicalRecord? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].Id == id)
                return _records[i];
        }
        return null;
    }

    public MedicalRecord[] GetByPatient(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId)
                matches++;
        }

        MedicalRecord[] result = new MedicalRecord[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId)
            {
                result[index] = _records[i];
                index++;
            }
        }
        return result;
    }

    public MedicalRecord[] GetByDoctor(int doctorId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId)
                matches++;
        }

        MedicalRecord[] result = new MedicalRecord[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId)
            {
                result[index] = _records[i];
                index++;
            }
        }
        return result;
    }

    // далі вибірки за типом: is перевіряє реальний тип об'єкта і одразу дає змінну потрібного типу
    public Diagnosis[] GetDiagnoses(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis)
                matches++;
        }

        Diagnosis[] result = new Diagnosis[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis diagnosis)
            {
                result[index] = diagnosis;
                index++;
            }
        }
        return result;
    }

    // тут через as: якщо запис не LabResult - отримаємо null, а не виняток
    public LabResult[] GetLabResults(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            LabResult? labResult = _records[i] as LabResult;
            if (labResult != null && labResult.PatientId == patientId)
                matches++;
        }

        LabResult[] result = new LabResult[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            LabResult? labResult = _records[i] as LabResult;
            if (labResult != null && labResult.PatientId == patientId)
            {
                result[index] = labResult;
                index++;
            }
        }
        return result;
    }

    public Prescription[] GetPrescriptions(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription)
                matches++;
        }

        Prescription[] result = new Prescription[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription prescription)
            {
                result[index] = prescription;
                index++;
            }
        }
        return result;
    }

    // змінна з is доступна праворуч від &&, тому можна одразу перевірити IsChronic
    public Diagnosis[] GetChronicDiagnoses(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis diagnosis && diagnosis.IsChronic)
                matches++;
        }

        Diagnosis[] result = new Diagnosis[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis diagnosis && diagnosis.IsChronic)
            {
                result[index] = diagnosis;
                index++;
            }
        }
        return result;
    }

    public Prescription[] GetActivePrescriptions(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription prescription && prescription.IsActive())
                matches++;
        }

        Prescription[] result = new Prescription[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription prescription && prescription.IsActive())
            {
                result[index] = prescription;
                index++;
            }
        }
        return result;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Медичних записів немає.");
            return;
        }

        Console.WriteLine($"=== Медичні записи ({_count}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_records[i]);
        }
    }

    // жодних перевірок типу: кожен запис сам знає, як себе показати (ToString нащадка)
    public void DisplayList(MedicalRecord[] records)
    {
        if (records.Length == 0)
        {
            Console.WriteLine("Записів не знайдено.");
            return;
        }

        for (int i = 0; i < records.Length; i++)
        {
            Console.WriteLine(records[i]);
        }
    }

    public void DisplayPatientSummary(int patientId)
    {
        MedicalRecord[] records = GetByPatient(patientId);
        if (records.Length == 0)
        {
            Console.WriteLine("У пацієнта #" + patientId + " немає медичних записів.");
            return;
        }

        // один прохід - три лічильники
        int diagnoses = 0;
        int labResults = 0;
        int prescriptions = 0;
        for (int i = 0; i < records.Length; i++)
        {
            if (records[i] is Diagnosis)
                diagnoses++;
            else if (records[i] is LabResult)
                labResults++;
            else if (records[i] is Prescription)
                prescriptions++;
        }

        Console.WriteLine($"=== Медична картка пацієнта #{patientId} ===");
        Console.WriteLine($"Всього записів: {records.Length} (діагнозів: {diagnoses}, аналізів: {labResults}, рецептів: {prescriptions})");

        // порожні розділи не показуємо
        Diagnosis[] chronic = GetChronicDiagnoses(patientId);
        if (chronic.Length > 0)
        {
            Console.WriteLine($"Хронічні діагнози ({chronic.Length}):");
            for (int i = 0; i < chronic.Length; i++)
                Console.WriteLine("  " + chronic[i]);
        }

        Prescription[] active = GetActivePrescriptions(patientId);
        if (active.Length > 0)
        {
            Console.WriteLine($"Активні рецепти ({active.Length}):");
            for (int i = 0; i < active.Length; i++)
                Console.WriteLine($"  {active[i]} | до {active[i].ExpiresAt:dd.MM.yyyy}");
        }
    }
}
