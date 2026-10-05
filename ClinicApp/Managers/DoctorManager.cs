using ClinicApp.Enums;
using ClinicApp.Models;
using ClinicApp.Utils;

namespace ClinicApp.Managers;

public class DoctorManager
{
    private const int MaxDoctors = 50;

    private Doctor[] _doctors = new Doctor[MaxDoctors];
    private int _count = 0;

    public int Count => _count;

    // індексатор, як у PatientManager
    public Doctor? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
                return null;
            return _doctors[index];
        }
    }

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

    public bool TryFindById(int id, out Doctor doctor)
    {
        Doctor? found = FindById(id);
        if (found == null)
        {
            doctor = null!;
            return false;
        }

        doctor = found;
        return true;
    }

    // той самий двопрохідний пошук, що і FindByName у пацієнтів
    // шукаємо по українській назві спеціальності, щоб працювало "кардіо"
    public Doctor[] FindBySpeciality(string query)
    {
        string lowerQuery = query.ToLower();

        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (ClinicFormatter.FormatSpeciality(_doctors[i].Speciality).ToLower().Contains(lowerQuery))
                matches++;
        }

        Doctor[] result = new Doctor[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (ClinicFormatter.FormatSpeciality(_doctors[i].Speciality).ToLower().Contains(lowerQuery))
            {
                result[index] = _doctors[i];
                index++;
            }
        }
        return result;
    }

    // перевантаження: те саме ім'я, але параметр enum - точне співпадіння
    public Doctor[] FindBySpeciality(Speciality speciality)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality == speciality)
                matches++;
        }

        Doctor[] result = new Doctor[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality == speciality)
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
            Console.WriteLine($"  {ClinicFormatter.FormatSpeciality(_doctors[i].Speciality)}: {withSpeciality}");
        }
        Console.WriteLine("==========================");
    }
}
