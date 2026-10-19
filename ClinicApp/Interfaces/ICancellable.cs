namespace ClinicApp.Interfaces;

// контракт "це можна скасувати"
public interface ICancellable
{
    bool IsCancelled { get; }
    string CancellationReason { get; }
    // = "" і тут, а не тільки в класі: інакше c.Cancel() через змінну інтерфейсу не скомпілюється
    bool Cancel(string reason = "");
}
