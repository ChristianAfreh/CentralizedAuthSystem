using System.Linq.Expressions;

namespace CentralAuthAPI.Repository.IRepository
{
	public interface IBaseRepository<T>
	{
		Task<T> GetAsync(object id);

		Task<IEnumerable<T>> GetAllAsync();

		IQueryable<T> Query();

		Task InsertAsync(T entity);

		Task InsertAsync(IEnumerable<T> entities);

		void Delete(T entity);

		void Delete(IEnumerable<T> entities);

		void Update(T entity);

		Task<int> SaveAsync();
	}
}
