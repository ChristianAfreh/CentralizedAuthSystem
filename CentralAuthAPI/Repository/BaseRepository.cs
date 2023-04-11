using CentralAuthAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;

namespace CentralAuthAPI.Repository
{
	public class BaseRepository<T> : IBaseRepository<T> where T : class
	{
		 protected DbContext DbContext { get; set; }

        public BaseRepository(DbContext DbContext)
        {
            this.DbContext = DbContext;
        }

        public async Task<T> GetAsync(object id)
        {
            return await DbContext.FindAsync<T>(id);
        }

        public async Task InsertAsync(T entity)
        {
            await DbContext.Set<T>().AddAsync(entity);
        }

        public async Task InsertAsync(IEnumerable<T> entities)
        {
            await DbContext.Set<T>().AddRangeAsync(entities);
        }

        public IQueryable<T> Query()
        {
            return DbContext.Set<T>().AsQueryable();
        }

        public void Update(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            DbContext.Set<T>().Remove(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            DbContext.Set<T>().RemoveRange(entities);
        }

        public async Task<int> SaveAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbContext.Set<T>().ToListAsync();
        }

        protected DbContext _appContext
        {
            get { return DbContext; }
        }

		
	}
}
