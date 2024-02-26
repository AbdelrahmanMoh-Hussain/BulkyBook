using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
	{
		private readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
			_context = context;
        }
        public void Add(T entity)
		{
			//_context.Add<T>(entity);
			//dbSet.Add(entity);
			_context.Set<T>().Add(entity);
		}

		public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
		{
			IQueryable<T> quary = _context.Set<T>();
			if (!string.IsNullOrEmpty(includeProperties))
			{
				var props = includeProperties
					.Split(',', StringSplitOptions.RemoveEmptyEntries);
				foreach (var prop in props)
				{
					quary = quary.Include(prop);
				}
			}
			return quary.FirstOrDefault(filter);
		}

		public IEnumerable<T> GetAll(string? includeProperties = null)
		{
			IQueryable<T> quary = _context.Set<T>();
			if(!string.IsNullOrEmpty(includeProperties))
			{
				var props = includeProperties
					.Split(',', StringSplitOptions.RemoveEmptyEntries);
				foreach (var prop in props)
				{
					quary = quary.Include(prop);
				}
			}
			return quary;

		}

        public IEnumerable<T> GetAllWhere(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
			return _context.Set<T>().Where(filter);
        }

        public void Remove(T entity)
		{
			_context.Set<T>().Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			_context.Set<T>().RemoveRange(entities);
		}
	}
}
