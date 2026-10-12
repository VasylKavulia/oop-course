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
}
