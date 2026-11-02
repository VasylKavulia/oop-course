namespace ClinicApp.Models;

// черга очікування: хто перший прийшов, того першим і приймають (FIFO)
// на T обмежень немає, тому підійде і WaitingQueue<Patient>, і WaitingQueue<string>
public class WaitingQueue<T>
{
    private Queue<T> _queue = new Queue<T>();

    public int Count => _queue.Count;

    public bool IsEmpty => _queue.Count == 0;

    public void Enqueue(T item)
    {
        _queue.Enqueue(item);
    }

    // Queue і сама кидає InvalidOperationException, але своє повідомлення зрозуміліше
    public T Dequeue()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Черга порожня, приймати нікого.");
        return _queue.Dequeue();
    }

    public T Peek()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Черга порожня.");
        return _queue.Peek();
    }

    // для виводу, порядок той самий, що й у черзі
    public T[] ToArray()
    {
        return _queue.ToArray();
    }
}
