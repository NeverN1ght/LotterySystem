using System.Threading.Tasks;
using LotterySystem.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LotterySystem.Data.Repositories
{
    public class SqlRepository<TEntity> : IRepository<TEntity> where TEntity: Entity
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public SqlRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }
    }
}
