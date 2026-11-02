using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class PatientManager
{
    // List сам росте при додаванні, тому ні ліміту, ні ручного лічильника вже не треба
    private List<Patient> _patients = new List<Patient>();

    public int Count => _patients.Count;

    // індексатор - доступ за позицією: clinic.Patients[0]
    // за межами списку повертаємо null, тому тип Patient?
    public Patient? this[int index]
    {
        get
        {
            if (index < 0 || index >= _patients.Count)
                return null;
            return _patients[index];
        }
    }

    public void Add(Patient patient)
    {
        _patients.Add(patient);
        Console.WriteLine($"Пацієнта [{patient.Id}] {patient.FullName} додано.");
    }

    // лінійний пошук, null якщо такого Id немає
    public Patient? FindById(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
                return _patients[i];
        }
        return null;
    }

    // патерн TryXxx: повертаємо чи знайшли, а самого пацієнта віддаємо через out
    public bool TryFindById(int id, out Patient patient)
    {
        Patient? found = FindById(id);
        if (found == null)
        {
            patient = null!; // out треба заповнити навіть коли не знайшли
            return false;
        }

        patient = found;
        return true;
    }

    // пошук по частині імені або прізвища без урахування регістру
    public Patient[] FindByName(string name)
    {
        string query = name.ToLower();

        // розмір наперед не відомий - збираємо в список, а масив робимо в кінці
        List<Patient> result = new List<Patient>();
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(query) ||
                _patients[i].LastName.ToLower().Contains(query))
                result.Add(_patients[i]);
        }
        return result.ToArray();
    }

    // як FindByName, але умова - точна рівність enum
    public Patient[] FindByBloodType(BloodType bloodType)
    {
        List<Patient> result = new List<Patient>();
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].BloodType == bloodType)
                result.Add(_patients[i]);
        }
        return result.ToArray();
    }

    // копія, а не сам список, як і в DoctorManager
    public Patient[] GetAll()
    {
        return _patients.ToArray();
    }

    public bool Remove(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
            {
                // RemoveAt сам зсуває всі наступні елементи
                _patients.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public void DisplayAll()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        Console.WriteLine($"=== Пацієнти ({_patients.Count}) ===");
        for (int i = 0; i < _patients.Count; i++)
        {
            Console.WriteLine(_patients[i]);
        }
        Console.WriteLine("────────────────────────────────────────────────────────────");
    }

    public void DisplayStats()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        int ageSum = 0;
        int youngest = 0; // індекси, а не самі об'єкти
        int oldest = 0;
        int adults = 0;

        for (int i = 0; i < _patients.Count; i++)
        {
            ageSum += _patients[i].Age;
            if (_patients[i].Age < _patients[youngest].Age)
                youngest = i;
            if (_patients[i].Age > _patients[oldest].Age)
                oldest = i;
            if (_patients[i].IsAdult)
                adults++;
        }

        double averageAge = (double)ageSum / _patients.Count;

        Console.WriteLine("=== Статистика пацієнтів ===");
        Console.WriteLine("Всього:       " + _patients.Count);
        Console.WriteLine($"Середній вік: {averageAge:F1} р.");
        Console.WriteLine($"Наймолодший:  {_patients[youngest].FullName} ({_patients[youngest].Age} р.)");
        Console.WriteLine($"Найстарший:   {_patients[oldest].FullName} ({_patients[oldest].Age} р.)");
        Console.WriteLine($"Дорослих:     {adults} з {_patients.Count}");
        Console.WriteLine("============================");
    }
}
