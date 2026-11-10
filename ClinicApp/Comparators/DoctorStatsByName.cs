using ClinicApp.Models;

namespace ClinicApp.Comparators;

// за ім'ям від А до Я
public class DoctorStatsByName : IComparer<DoctorStats>
{
    public int Compare(DoctorStats? x, DoctorStats? y)
    {
        if (x == null && y == null)
            return 0;
        if (x == null)
            return -1;
        if (y == null)
            return 1;
        // CurrentCulture, а не Ordinal - за кодами символів Є, І, Ї були б перед А
        return string.Compare(x.FullName, y.FullName, StringComparison.CurrentCulture);
    }
}
