using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
	
namespace MVC5Course.Models
{   
	public  class ProductRepository : EFRepository<Product>, IProductRepository
    {
        public override IQueryable<Product> All()
        {
            return base.All().Where(p => p.IsDel == false);
        }

        public IQueryable<Product> All(bool ShowAll)
        {
            if (ShowAll)
            {
                return base.All();
            }
            else
            {
                return this.All();
            }
        }

        //index
        public IQueryable<Product> GetAllRecords(bool Active)
        {
            return this.All().Where(p => p.Active.HasValue && p.Active.Value == Active).OrderByDescending(p => p.ProductId).Take(10);
        }
        //detail
        public Product GetRecordByProductId(int id)
        {
            return this.All().FirstOrDefault(p => p.ProductId == id);
        }

        public void Update(Product product)
        {
            this.UnitOfWork.Context.Entry(product).State = EntityState.Modified;
        }
	}

	public  interface IProductRepository : IRepository<Product>
	{

	}
}