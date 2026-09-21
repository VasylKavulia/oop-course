namespace ClinicApp;

public class DoctorManager
{
    private const int MaxDoctors = 50;

    private Doctor[] _doctors = new Doctor[MaxDoctors];
    private int _count = 0;

    public int Count => _count;

    public void Add(Doctor doctor)
    {
        if (_count >= MaxDoctors)
        {
            Console.WriteLine("Неможливо додати: досягнуто ліміт " + MaxDoctors + " лікарів.");
            return;
        }

        _doctors[_count] = doctor;
        _count++;
        Console.WriteLine($"Лікаря [{doctor.Id}] {doctor.FullName} додано.");
    }

    public Doctor? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
                return _doctors[i];
        }
        return null;
    }

    // той самий двопрохідний пошук, що і FindByName у пацієнтів
    public Doctor[] FindBySpeciality(string speciality)
    {
        string query = speciality.ToLower();

        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality.ToLower().Contains(query))
                matches++;
        }

        Doctor[] result = new Doctor[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality.ToLower().Contains(query))
            {
                result[index] = _doctors[i];
                index++;
            }
        }
        return result;
    }

    // копія масиву, щоб ззовні не можна було зіпсувати внутрішній
    public Doctor[] GetAll()
    {
        Doctor[] result = new Doctor[_count];
        Array.Copy(_doctors, result, _count);
        return result;
    }

    public bool Remove(int id)
    {
        int position = -1;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
            {
                position = i;
                break;
            }
        }

        if (position == -1)
            return false;

        for (int i = position; i < _count - 1; i++)
        {
            _doctors[i] = _doctors[i + 1];
        }
        _doctors[_count - 1] = null!;
        _count--;
        return true;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список лікарів порожній.");
            return;
        }

        Console.WriteLine($"=== Лікарі ({_count} / {MaxDoctors}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_doctors[i]);
        }
        Console.WriteLine("────────────────────────────────────────────────────────────");
    }

    public void DisplayStats()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список лікарів порожній.");
            return;
        }

        int available = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].IsAvailableNow)
                available++;
        }

        Console.WriteLine("=== Статистика лікарів ===");
        Console.WriteLine("Всього:         " + _count);
        Console.WriteLine("Доступні зараз: " + available);
        Console.WriteLine("По спеціальностях:");

        // унікальні спеціальності без Dictionary: спеціальність нова,
        // якщо не траплялась у жодного з попередніх лікарів
        for (int i = 0; i < _count; i++)
        {
            bool alreadyShown = false;
            for (int j = 0; j < i; j++)
            {
                if (_doctors[j].Speciality == _doctors[i].Speciality)
                {
                    alreadyShown = true;
                    break;
                }
            }
            if (alreadyShown)
                continue;

            int withSpeciality = 0;
            for (int k = 0; k < _count; k++)
            {
                if (_doctors[k].Speciality == _doctors[i].Speciality)
                    withSpeciality++;
            }
            Console.WriteLine($"  {_doctors[i].Speciality}: {withSpeciality}");
        }
        Console.WriteLine("==========================");
    }
}
