using ClinicApp.Models;

namespace ClinicApp.Managers;

public class AnalyticsManager
{
    // ті самі менеджери, що й у Clinic, тому статистика завжди по актуальних даних
    private readonly AppointmentManager _appointments;
    private readonly DoctorManager _doctors;
    private readonly PatientManager _patients;

    public AnalyticsManager(AppointmentManager appointments, DoctorManager doctors, PatientManager patients)
    {
        _appointments = appointments;
        _doctors = doctors;
        _patients = patients;
    }

    // через yield return наступного лікаря рахуємо тільки тоді, коли foreach його попросить
    // лікар без прийомів теж буде: 0, 0 і DateTime.MinValue
    public IEnumerable<DoctorStats> ComputeDoctorStats()
    {
        Doctor[] doctors = _doctors.GetAll();
        Appointment[] appointments = _appointments.GetAll();

        foreach (Doctor doctor in doctors)
        {
            int count = 0;
            decimal revenue = 0m;
            DateTime last = DateTime.MinValue;

            foreach (Appointment appointment in appointments)
            {
                if (appointment.DoctorId == doctor.Id)
                {
                    count++;
                    revenue += appointment.GetCost();
                    if (appointment.ScheduledAt > last)
                        last = appointment.ScheduledAt;
                }
            }

            yield return new DoctorStats(doctor.Id, doctor.FullName, count, revenue, last);
        }
    }

    // те саме для пацієнтів
    public IEnumerable<PatientStats> ComputePatientStats()
    {
        Patient[] patients = _patients.GetAll();
        Appointment[] appointments = _appointments.GetAll();

        foreach (Patient patient in patients)
        {
            int count = 0;
            decimal spent = 0m;
            DateTime last = DateTime.MinValue;

            foreach (Appointment appointment in appointments)
            {
                if (appointment.PatientId == patient.Id)
                {
                    count++;
                    spent += appointment.GetCost();
                    if (appointment.ScheduledAt > last)
                        last = appointment.ScheduledAt;
                }
            }

            yield return new PatientStats(patient.Id, patient.FullName, count, spent, last);
        }
    }
}
