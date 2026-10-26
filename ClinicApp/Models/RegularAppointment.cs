namespace ClinicApp.Models;

// звичайний прийом: ціна як у батька, свій тільки опис
public class RegularAppointment : Appointment
{
    // все потрібне вже є в конструкторі Appointment, тут лише передаємо параметри
    public RegularAppointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
        : base(patientId, doctorId, scheduledAt, durationMinutes)
    {
    }

    public override string GetDescription()
    {
        return "Звичайний прийом";
    }
}
