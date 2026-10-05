using ClinicApp.Managers;
using ClinicApp.Models;

namespace ClinicApp;

// об'єднує всі менеджери, сам майже нічого не робить - делегує їм
public class Clinic
{
    public string Name { get; set; }
    public PatientManager Patients { get; }
    public DoctorManager Doctors { get; }
    public AppointmentManager Appointments { get; }

    public Clinic(string name)
    {
        Name = name;
        // порядок важливий: менеджер записів потребує двох інших уже готових
        Patients = new PatientManager();
        Doctors = new DoctorManager();
        Appointments = new AppointmentManager(Patients, Doctors);
    }

    public void DisplaySchedule(DateTime date)
    {
        Console.WriteLine($"=== Розклад на {date:dd.MM.yyyy} ===");
        Appointments.DisplayList(Appointments.GetByDate(date));
    }

    public void GenerateReport()
    {
        Appointment[] upcoming = Appointments.GetUpcoming();
        Doctor[] doctors = Doctors.GetAll();

        Console.WriteLine("╔══════════════════════════════════════════════╗");
        Console.WriteLine("║  Звіт — " + Name);
        Console.WriteLine("╠══════════════════════════════════════════════╣");
        Console.WriteLine("║  Пацієнтів:          " + Patients.Count);
        Console.WriteLine("║  Лікарів:            " + Doctors.Count);
        Console.WriteLine("║  Майбутніх записів:  " + upcoming.Length);
        Console.WriteLine("╠══════════════════════════════════════════════╣");
        Console.WriteLine("║  Навантаження лікарів (майбутні записи):");
        for (int i = 0; i < doctors.Length; i++)
        {
            // рахуємо, скільки майбутніх записів у цього лікаря
            int load = 0;
            for (int j = 0; j < upcoming.Length; j++)
            {
                if (upcoming[j].DoctorId == doctors[i].Id)
                    load++;
            }
            Console.WriteLine($"║    {doctors[i].FullName} ({doctors[i].Speciality}): {load} записів");
        }
        Console.WriteLine("╚══════════════════════════════════════════════╝");
    }
}
