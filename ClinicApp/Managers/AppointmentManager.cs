using ClinicApp.Models;

namespace ClinicApp.Managers;

public class AppointmentManager
{
    private const int MaxAppointments = 500;

    private Appointment[] _appointments = new Appointment[MaxAppointments];
    private int _count = 0;

    // посилання на ті самі менеджери, з якими працює вся програма
    private PatientManager _patients;
    private DoctorManager _doctors;

    public int Count => _count;

    // індексатор, як у PatientManager і DoctorManager
    public Appointment? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
                return null;
            return _appointments[index];
        }
    }

    public AppointmentManager(PatientManager patients, DoctorManager doctors)
    {
        _patients = patients;
        _doctors = doctors;
    }

    public bool Book(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        Patient? patient = _patients.FindById(patientId);
        if (patient == null)
        {
            Console.WriteLine("Помилка: пацієнта з ID " + patientId + " не знайдено.");
            return false;
        }

        Doctor? doctor = _doctors.FindById(doctorId);
        if (doctor == null)
        {
            Console.WriteLine("Помилка: лікаря з ID " + doctorId + " не знайдено.");
            return false;
        }

        if (_count >= MaxAppointments)
        {
            Console.WriteLine("Помилка: досягнуто ліміт записів.");
            return false;
        }

        Appointment appointment = new Appointment(patientId, doctorId, scheduledAt, durationMinutes);
        _appointments[_count] = appointment;
        _count++;
        Console.WriteLine($"Запис [{appointment.Id}] створено: {patient.FullName} → {doctor.FullName} о {scheduledAt:dd.MM.yyyy HH:mm}");
        return true;
    }

    public bool Cancel(int id, string reason = "")
    {
        Appointment? appointment = FindById(id);
        if (appointment == null)
            return false;
        return appointment.Cancel(reason);
    }

    public bool Complete(int id)
    {
        Appointment? appointment = FindById(id);
        if (appointment == null)
            return false;
        return appointment.Complete();
    }

    public Appointment[] GetByPatient(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId)
                matches++;
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId)
                result[index++] = _appointments[i];
        }
        return result;
    }

    public Appointment[] GetByDoctor(int doctorId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId)
                matches++;
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId)
                result[index++] = _appointments[i];
        }
        return result;
    }

    // порівнюємо тільки дату, час не важливий
    public Appointment[] GetByDate(DateTime date)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date)
                matches++;
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date)
                result[index++] = _appointments[i];
        }
        return result;
    }

    // перевантаження: будуємо DateTime і викликаємо версію вище, логіку не дублюємо
    public Appointment[] GetByDate(int year, int month, int day)
    {
        return GetByDate(new DateTime(year, month, day));
    }

    public Appointment[] GetUpcoming()
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming)
                matches++;
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming)
                result[index++] = _appointments[i];
        }
        return result;
    }

    // замість Id показуємо імена, якщо пацієнта/лікаря вже видалили - лишаємо #Id
    public void DisplayAppointment(Appointment appointment)
    {
        Patient? patient = _patients.FindById(appointment.PatientId);
        Doctor? doctor = _doctors.FindById(appointment.DoctorId);

        string patientName = patient?.FullName ?? "Пацієнт #" + appointment.PatientId;
        string doctorName = doctor?.FullName ?? "Лікар #" + appointment.DoctorId;

        string line = $"[{appointment.Id}] {patientName} → {doctorName} | {appointment.ScheduledAt:dd.MM.yyyy HH:mm}–{appointment.EndsAt:HH:mm} | {appointment.Status}";
        if (appointment.Notes.Length > 0)
            line += " | " + appointment.Notes;
        Console.WriteLine(line);
    }

    public void DisplayList(Appointment[] appointments)
    {
        if (appointments.Length == 0)
        {
            Console.WriteLine("Записів не знайдено.");
            return;
        }

        for (int i = 0; i < appointments.Length; i++)
        {
            DisplayAppointment(appointments[i]);
        }
    }

    private Appointment? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].Id == id)
                return _appointments[i];
        }
        return null;
    }
}
