namespace ClinicApp.Interfaces;

// контракт для тих, хто працює за розкладом
public interface ISchedulable
{
    bool CanSchedule(DateTime at);
    DateTime[] GetAvailableSlots(DateTime date, int slotCount);
}
