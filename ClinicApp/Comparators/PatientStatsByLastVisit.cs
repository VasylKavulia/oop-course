using ClinicApp.Models;

namespace ClinicApp.Comparators;

// найновіший візит вище, пацієнти без візитів (MinValue) опиняються в кінці
public class PatientStatsByLastVisit : IComparer<PatientStats>
{
    public int Compare(PatientStats? x, PatientStats? y)
    {
        if (x == null && y == null)
            return 0;
        if (x == null)
            return -1;
        if (y == null)
            return 1;
        // DateTime теж IComparable, порівнюється як число
        return y.LastVisitDate.CompareTo(x.LastVisitDate);
    }
}
