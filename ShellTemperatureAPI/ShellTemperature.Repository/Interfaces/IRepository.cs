using System.Threading.Tasks;

namespace ShellTemperature.Repository.Interfaces
{
    public interface IRepository<T>
    {
        Task<bool> Create(T model);
    }
}