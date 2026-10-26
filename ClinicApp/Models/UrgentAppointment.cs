namespace ClinicApp.Models;

public class UrgentAppointment : Appointment
{
    // причина терміновості
    public string UrgencyNote { get; }

    public UrgentAppointment(int patientId, int doctorId, DateTime scheduledAt, string urgencyNote = "", int durationMinutes = 30)
        : base(patientId, doctorId, scheduledAt, durationMinutes)
    {
        UrgencyNote = urgencyNote;
    }

    // на 50% дорожче, рахуємо від ціни батька через base
    public override decimal GetCost()
    {
        return base.GetCost() * 1.5m;
    }

    // sealed override - нащадки термінового прийому опис уже не змінять
    public sealed override string GetDescription()
    {
        // без причини - просто "Терміновий", без порожніх дужок
        if (string.IsNullOrWhiteSpace(UrgencyNote))
            return "Терміновий";
        return "Терміновий (" + UrgencyNote + ")";
    }

    // GetPriority у батька не virtual, тому override неможливий - лише приховати через new
    public new int GetPriority()
    {
        return 1;
    }
}
