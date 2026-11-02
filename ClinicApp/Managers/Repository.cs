using ClinicApp.Interfaces;

namespace ClinicApp.Managers;

// where T : IIdentifiable - гарантія, що в T є Id, без цього item.Id не скомпілюється
public class Repository<T> where T : IIdentifiable
{
    private List<T> _items = new List<T>();

    public int Count => _items.Count;

    public void Add(T item)
    {
        _items.Add(item);
    }

    public T GetById(int id)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].Id == id)
                return _items[i];
        }
        // не знайшли - default, для класів це null (як null! у TryFindById)
        return default!;
    }

    public T[] GetAll()
    {
        return _items.ToArray();
    }

    public bool Remove(int id)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].Id == id)
            {
                _items.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
}
