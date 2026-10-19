namespace ClinicApp.Interfaces;

// контракт "це можна оплатити" - підписати його може будь-який клас, не тільки Appointment
public interface IPayable
{
    decimal GetCost();
    bool IsPaid { get; }
    void MarkPaid();
}
