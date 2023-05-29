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
		private readonly ApplicationDbContext _db;
		public ProductRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Product product)
		{
			_db.Products.Update(product);
			
			//EXPLICIT ABOUT WHAT NEED BE UPDATE
			//var objFromDb = _db.Products.FirstOrDefault(product => product.Id == product.Id);
			//if (objFromDb != null)
			//{
			//	objFromDb.Title = product.Title;
			//	objFromDb.Description = product.Description;
			//	objFromDb.ISBN = product.ISBN;
			//	objFromDb.Author = product.Author;
			//	objFromDb.CategoryId = product.CategoryId;
			//	objFromDb.Price = product.Price;
			//	objFromDb.Price100 = product.Price100;
			//	objFromDb.Price50 = product.Price50;
			//	objFromDb.ListPrice = product.ListPrice;
			//	if(product.ImgUrl!=null)
			//	{
			//		objFromDb.ImgUrl = product.ImgUrl;
			//	}
			//}
		}
	}
}
