using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
            :base(context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetTopPrice(int count)
        {
            return _context.Products
                        .OrderByDescending(x => x.Price)
                        .Take(count)
                        .ToList();
        }

        public void Update(Product product)
        {
            var objFromDB = Get(x => x.Id == product.Id);
            if(objFromDB != null) 
            {
                objFromDB.ISBN = product.ISBN;
                objFromDB.Title = product.Title;
                objFromDB.Description = product.Description;
                objFromDB.Price = product.Price;
                objFromDB.ListPrice = product.ListPrice;
                objFromDB.Price50 = product.Price50;
                objFromDB.Price100 = product.Price100;
                objFromDB.Author = product.Author;
                objFromDB.CategoryId = product.CategoryId;

                if(product.ImageUrl is not null) 
                {
                    objFromDB.ImageUrl = product.ImageUrl;
                }
            }

        }
    }
}
