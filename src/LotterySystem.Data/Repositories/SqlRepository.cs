using System.Threading.Tasks;
using LotterySystem.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LotterySystem.Data.Repositories
{
    public class SqlRepository : IRepository
    {
        private readonly DbContext _context;

        public SqlRepository(DbContext context)
        {
            _context = context;
        }

        public async Task AddAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetEntityCountAsync<TEntity>() where TEntity : Entity
        {
            return await _context.Set<TEntity>().CountAsync();
        }
    }
}
