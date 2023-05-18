using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

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
			//Projections in EF Core
			IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString()
			});

			//pass data with viewBag
			//ViewBag.CategoryList = CategoryList;

			//pass data with ViewData
			//ViewData["CategoryList"] = CategoryList;

			//Using View Model to pass data
			ProductVM productVM = new()
			{
				CategoryList = CategoryList,
				Product = new Product()
			};


			return View(productVM);
		}

		[HttpPost]
		public IActionResult Create(ProductVM obj) 
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Product.Add(obj.Product);
				_unitOfWork.Save();
				TempData["success"] = "Product created successfully";

				return RedirectToAction("Index");
			}
			else
			{
				//Projections in EF Core
				obj.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				});
				return View(obj);
			}
			
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
