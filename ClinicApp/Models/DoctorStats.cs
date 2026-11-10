namespace ClinicApp.Models;

// статистика по лікарю, сам клас нічого не рахує, тільки зберігає
public class DoctorStats : IComparable<DoctorStats>
{
    public int DoctorId { get; }
    public string FullName { get; }
    public int AppointmentCount { get; }
    public decimal TotalRevenue { get; }
    // DateTime.MinValue - прийомів ще не було
    public DateTime LastAppointmentDate { get; }

    public DoctorStats(int doctorId, string fullName, int appointmentCount, decimal totalRevenue, DateTime lastAppointmentDate)
    {
        DoctorId = doctorId;
        FullName = fullName;
        AppointmentCount = appointmentCount;
        TotalRevenue = totalRevenue;
        LastAppointmentDate = lastAppointmentDate;
    }

    // other з this, а не навпаки - щоб після Sort() більше прийомів було першим
    public int CompareTo(DoctorStats? other)
    {
        if (other == null)
            return 1;
        return other.AppointmentCount.CompareTo(AppointmentCount);
    }

    public override string ToString()
    {
        // без прийомів замість 01.01.0001 буде прочерк
        string last = LastAppointmentDate == DateTime.MinValue ? "—" : LastAppointmentDate.ToString("dd.MM.yyyy");
        return $"[{DoctorId}] {FullName} | Прийомів: {AppointmentCount} | Виручка: {TotalRevenue:F2} грн | Останній: {last}";
    }
}
