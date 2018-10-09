using System.Threading.Tasks;

namespace LotterySystem.Data.Abstractions
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : Entity;

        Task SaveAsync();
    }
}
