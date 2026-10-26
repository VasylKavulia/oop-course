namespace ClinicApp.Models;

// sealed class - від консультації успадковуватись уже не можна
public sealed class SpecialistAppointment : Appointment
{
    public string ConsultationTopic { get; }

    public SpecialistAppointment(int patientId, int doctorId, DateTime scheduledAt, string topic = "", int durationMinutes = 45)
        : base(patientId, doctorId, scheduledAt, durationMinutes)
    {
        ConsultationTopic = topic;
    }

    // на 30% дорожче за базову ціну
    public override decimal GetCost()
    {
        return base.GetCost() * 1.3m;
    }

    public override string GetDescription()
    {
        if (string.IsNullOrWhiteSpace(ConsultationTopic))
            return "Консультація спеціаліста";
        return "Консультація спеціаліста: " + ConsultationTopic;
    }
}
