using System.Threading.Tasks;

namespace LotterySystem.Data.Abstractions
{
    public interface IRepository
    {
        Task AddAsync<TEntity>(TEntity entity) where TEntity : Entity;

        Task<int> GetEntityCountAsync<TEntity>() where TEntity : Entity;
    }
}
