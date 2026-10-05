using ClinicApp.Models;

namespace ClinicApp.Managers;

// той самий менеджер, але без ліміту: масив сам росте, коли заповнюється
public class GrowablePatientManager
{
    // навмисно маленький початковий розмір, щоб побачити кілька розширень
    private Patient[] _patients = new Patient[4];
    private int _count = 0;

    public int Count => _count;

    // поточна довжина внутрішнього масиву
    public int Capacity => _patients.Length;

    public void Add(Patient patient)
    {
        if (_count == _patients.Length)
            Grow();

        _patients[_count] = patient;
        _count++;
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
                return _patients[i];
        }
        return null;
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

        for (int i = position; i < _count - 1; i++)
        {
            _patients[i] = _patients[i + 1];
        }
        _patients[_count - 1] = null!;
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

        Console.WriteLine($"=== Пацієнти ({_count}, ємність {_patients.Length}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_patients[i]);
        }
    }

    // створюємо масив удвічі більший і переносимо в нього всі елементи,
    // старий масив потім прибере збирач сміття
    private void Grow()
    {
        int newSize = _patients.Length * 2;
        Patient[] bigger = new Patient[newSize];
        for (int i = 0; i < _count; i++)
        {
            bigger[i] = _patients[i];
        }
        Console.WriteLine($"  Масив заповнений! Розширення: {_patients.Length} → {newSize}");
        _patients = bigger;
    }
}
