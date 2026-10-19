using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class BillingManager
{
    // присвоюється тільки в конструкторі
    private readonly AppointmentManager _appointments;

    public BillingManager(AppointmentManager appointments)
    {
        _appointments = appointments;
    }

    public IPayable[] GetAllUnpaid()
    {
        return FilterUnpaid(_appointments.GetAll());
    }

    public IPayable[] GetUnpaidByPatient(int patientId)
    {
        return FilterUnpaid(_appointments.GetByPatient(patientId));
    }

    // далі тільки IPayable: новий платний клас тут нічого не змінить
    public decimal GetTotalDebt()
    {
        IPayable[] unpaid = GetAllUnpaid();
        decimal total = 0m;
        for (int i = 0; i < unpaid.Length; i++)
            total += unpaid[i].GetCost();
        return total;
    }

    public decimal GetPatientDebt(int patientId)
    {
        IPayable[] unpaid = GetUnpaidByPatient(patientId);
        decimal total = 0m;
        for (int i = 0; i < unpaid.Length; i++)
            total += unpaid[i].GetCost();
        return total;
    }

    // MarkPaid() мовчить, якщо не вийшло, тому стан перевіряємо тут і повертаємо bool
    public bool PayAppointment(int appointmentId)
    {
        Appointment[] all = _appointments.GetAll();
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].Id == appointmentId)
            {
                if (all[i].IsPaid || all[i].IsCancelled)
                    return false;
                all[i].MarkPaid();
                return true;
            }
        }
        return false;
    }

    // метод не знає, що лежить у масиві; деталі показуємо тільки для записів
    public void DisplayUnpaid(IPayable[] items)
    {
        if (items.Length == 0)
        {
            Console.WriteLine("Немає неоплачених записів.");
            return;
        }

        for (int i = 0; i < items.Length; i++)
        {
            string cost = "Сума: " + items[i].GetCost().ToString("F2") + " грн";
            if (items[i] is Appointment appointment)
                Console.WriteLine(appointment + " | " + cost);
            else
                Console.WriteLine((i + 1) + ". " + cost);
        }
    }

    // спільний фільтр для обох Get...Unpaid
    // тут без Appointment не обійтись - такі записи віддає AppointmentManager
    private static IPayable[] FilterUnpaid(Appointment[] appointments)
    {
        int matches = 0;
        for (int i = 0; i < appointments.Length; i++)
        {
            if (!appointments[i].IsPaid && !appointments[i].IsCancelled)
                matches++;
        }

        IPayable[] result = new IPayable[matches];
        int index = 0;
        for (int i = 0; i < appointments.Length; i++)
        {
            if (!appointments[i].IsPaid && !appointments[i].IsCancelled)
                result[index++] = appointments[i];
        }
        return result;
    }
}
