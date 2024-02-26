using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
	public class CategoryRepository : Repository<Category>, ICategoryRepository
	{
		private readonly AppDbContext _context;
		public CategoryRepository(AppDbContext context)
			: base(context)
        {
            _context = context;
        }

		public IEnumerable<Category> Sort()
		{
			return _context.Categories.OrderBy(x => x.DisplayOrder);
		}

		public void Update(Category category)
		{
			_context.Categories.Update(category);
		}


	}
}
