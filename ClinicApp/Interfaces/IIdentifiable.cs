namespace ClinicApp.Interfaces;

// все, що має Id - на цьому тримається обмеження в Repository<T>
public interface IIdentifiable
{
    int Id { get; }
}
