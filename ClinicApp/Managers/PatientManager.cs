using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class PatientManager
{
    private const int MaxPatients = 100;

    // масив фіксованого розміру + лічильник, скільки комірок реально зайнято
    private Patient[] _patients = new Patient[MaxPatients];
    private int _count = 0;

    public int Count => _count;

    // індексатор - доступ за позицією: clinic.Patients[0]
    // за межами списку повертаємо null, тому тип Patient?
    public Patient? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
                return null;
            return _patients[index];
        }
    }

    public void Add(Patient patient)
    {
        if (_count >= MaxPatients)
        {
            Console.WriteLine("Неможливо додати: досягнуто ліміт " + MaxPatients + " пацієнтів.");
            return;
        }

        _patients[_count] = patient;
        _count++;
        Console.WriteLine($"Пацієнта [{patient.Id}] {patient.FullName} додано.");
    }

    // лінійний пошук, null якщо такого Id немає
    public Patient? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
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

        // перший прохід - рахуємо скільки підходить
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(query) ||
                _patients[i].LastName.ToLower().Contains(query))
                matches++;
        }

        // другий прохід - заповнюємо масив потрібного розміру
        Patient[] result = new Patient[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(query) ||
                _patients[i].LastName.ToLower().Contains(query))
            {
                result[index] = _patients[i];
                index++;
            }
        }
        return result;
    }

    // як FindByName, але умова - точна рівність enum
    public Patient[] FindByBloodType(BloodType bloodType)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].BloodType == bloodType)
                matches++;
        }

        Patient[] result = new Patient[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].BloodType == bloodType)
            {
                result[index] = _patients[i];
                index++;
            }
        }
        return result;
    }

    public bool Remove(int id)
    {
        int position = -1;
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                position = i;
                break;
            }
        }

        if (position == -1)
            return false;

        // зсуваємо всі наступні елементи на одну позицію вліво
        for (int i = position; i < _count - 1; i++)
        {
            _patients[i] = _patients[i + 1];
        }
        _patients[_count - 1] = null!; // остання комірка тепер дубль - очищаємо
        _count--;
        return true;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        Console.WriteLine($"=== Пацієнти ({_count} / {MaxPatients}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_patients[i]);
        }
        Console.WriteLine("────────────────────────────────────────────────────────────");
    }

    public void DisplayStats()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        int ageSum = 0;
        int youngest = 0; // індекси, а не самі об'єкти
        int oldest = 0;
        int adults = 0;

        for (int i = 0; i < _count; i++)
        {
            ageSum += _patients[i].Age;
            if (_patients[i].Age < _patients[youngest].Age)
                youngest = i;
            if (_patients[i].Age > _patients[oldest].Age)
                oldest = i;
            if (_patients[i].IsAdult)
                adults++;
        }

        double averageAge = (double)ageSum / _count;

        Console.WriteLine("=== Статистика пацієнтів ===");
        Console.WriteLine("Всього:       " + _count);
        Console.WriteLine($"Середній вік: {averageAge:F1} р.");
        Console.WriteLine($"Наймолодший:  {_patients[youngest].FullName} ({_patients[youngest].Age} р.)");
        Console.WriteLine($"Найстарший:   {_patients[oldest].FullName} ({_patients[oldest].Age} р.)");
        Console.WriteLine($"Дорослих:     {adults} з {_count}");
        Console.WriteLine("============================");
    }
}
