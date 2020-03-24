namespace ShellTemperature.Repository.Interfaces
{
    public interface IRepository<T>
    {
        bool Create(T model);
    }
}