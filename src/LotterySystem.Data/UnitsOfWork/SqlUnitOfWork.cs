using System.Threading.Tasks;
using LotterySystem.Data.Abstractions;
using LotterySystem.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LotterySystem.Data.UnitsOfWork
{
    public class SqlUnitOfWork: IUnitOfWork
    {
        private readonly DbContext _context;

        public SqlUnitOfWork(DbContext context)
        {
            _context = context;
        }

        public IRepository<T> GetRepository<T>() where T : Entity
        {
            return new SqlRepository<T>(_context);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
