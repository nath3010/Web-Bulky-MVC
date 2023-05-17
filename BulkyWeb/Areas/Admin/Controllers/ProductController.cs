using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
		{
			List<Product> objproductsList = _unitOfWork.Product.GetAll().ToList();

			return View(objproductsList);
		}

		public IActionResult Create() 
		{ 
			return View();
		}

		[HttpPost]
		public IActionResult Create(Product obj) 
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Product.Add(obj);
				_unitOfWork.Save();
				TempData["success"] = "Product created successfully";

				return RedirectToAction("Index");
			}
			return View();
		}
		public IActionResult Edit(int? id)
		{
			if(id == null || id == 0) 
			{
				return NotFound();
			}

			Product? productDb = _unitOfWork.Product.Get(u => u.Id == id);

			if (productDb == null)
			{
				return NotFound();
			}
			return View(productDb);
		}

		[HttpPost]
		public IActionResult Edit(Product obj) 
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Product.Update(obj);
				_unitOfWork.Save();

				TempData["success"] = "Product edit successfully";

				return RedirectToAction("Index");
			}
			return View();
		}

		public IActionResult Delete(int id) 
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Product? productDb = _unitOfWork.Product.Get(u => u.Id == id);

			if (productDb == null)
			{
				return NotFound();
			}
			return View(productDb);
		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePOST(int? id)
		{
			Product? obj = _unitOfWork.Product.Get(u =>u.Id == id);

			if(obj == null)
			{
				return NotFound();
			}

			_unitOfWork.Product.Delete(obj);
			_unitOfWork.Save();

			TempData["success"] = "Product deleted successfully";

			return RedirectToAction("Index");
		}


	}
}
