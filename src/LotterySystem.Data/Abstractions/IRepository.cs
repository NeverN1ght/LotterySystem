using System.Threading.Tasks;

namespace LotterySystem.Data.Abstractions
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task AddAsync(TEntity entity);
    }
}
