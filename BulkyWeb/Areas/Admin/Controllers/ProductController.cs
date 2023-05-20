﻿using Bulky.DataAccess.Repository.IRepository;
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
		private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
			
        }
        public IActionResult Index()
		{
			List<Product> objproductsList = _unitOfWork.Product.GetAll().ToList();

			return View(objproductsList);
		}

		public IActionResult Upsert(int? id) //Update and Insert 
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
			if(id ==  null ||  id == 0)
			{
				//create
				return View(productVM);
			}
			else
			{
				//update
				productVM.Product = _unitOfWork.Product.Get(u=>u.Id == id);
				return View(productVM);
			}

			
		}

		[HttpPost]
		public IActionResult Upsert(ProductVM productVM, IFormFile? file) 
		{
			if (ModelState.IsValid)
			{
				string wwwRootPath = _webHostEnvironment.WebRootPath;
				if (file != null)
				{
					string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
					string productPath = Path.Combine(wwwRootPath, @"images\product");

					using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
					{
						file.CopyTo(fileStream);
					}

					productVM.Product.ImgUrl = @"\images\product\" + fileName;
				}
				_unitOfWork.Product.Add(productVM.Product);
				_unitOfWork.Save();
				TempData["success"] = "Product created successfully";

				return RedirectToAction("Index");
			}
			else
			{
				//Projections in EF Core
				productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				});
				return View(productVM);
			}
			
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
