using ClinicApp.Models;

namespace ClinicApp.Comparators;

// більша виручка вище
public class DoctorStatsByRevenue : IComparer<DoctorStats>
{
    public int Compare(DoctorStats? x, DoctorStats? y)
    {
        if (x == null && y == null)
            return 0;
        if (x == null)
            return -1;
        if (y == null)
            return 1;
        // y з x - так виходить спадання
        return y.TotalRevenue.CompareTo(x.TotalRevenue);
    }
}
