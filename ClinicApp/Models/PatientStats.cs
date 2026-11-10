namespace ClinicApp.Models;

// те саме, що DoctorStats, тільки для пацієнта
public class PatientStats : IComparable<PatientStats>
{
    public int PatientId { get; }
    public string FullName { get; }
    public int VisitCount { get; }
    public decimal TotalSpent { get; }
    // DateTime.MinValue - візитів не було
    public DateTime LastVisitDate { get; }

    public PatientStats(int patientId, string fullName, int visitCount, decimal totalSpent, DateTime lastVisitDate)
    {
        PatientId = patientId;
        FullName = fullName;
        VisitCount = visitCount;
        TotalSpent = totalSpent;
        LastVisitDate = lastVisitDate;
    }

    // найактивніший пацієнт першим, тому знову other з this
    public int CompareTo(PatientStats? other)
    {
        if (other == null)
            return 1;
        return other.VisitCount.CompareTo(VisitCount);
    }

    public override string ToString()
    {
        string lastVisit = LastVisitDate == DateTime.MinValue ? "—" : LastVisitDate.ToString("dd.MM.yyyy");
        return $"[{PatientId}] {FullName} | Візитів: {VisitCount} | Витрачено: {TotalSpent:F2} грн | Останній візит: {lastVisit}";
    }
}
