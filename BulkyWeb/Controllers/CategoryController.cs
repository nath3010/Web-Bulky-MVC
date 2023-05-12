using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAcess.Data;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _CategoryRepo;
        public CategoryController(ICategoryRepository db)
        {
			_CategoryRepo = db;    
        }

        public IActionResult Index()
        {
            //get all categorys
            List<Category> objCategoryList = _CategoryRepo.GetAll().ToList();
              
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
		public IActionResult Create(Category obj)
		{
            if (obj.Name == obj.DisplayOrder.ToString()) 
            {
                ModelState.AddModelError("name", "The display order cannot exactly match the name.");

			}

			if (ModelState.IsValid)
            {
				_CategoryRepo.Add(obj);
				_CategoryRepo.Save();
				TempData["success"] = "Category created successfully";

				return RedirectToAction("Index");
			}

            return View();

		}

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) 
            {
                return NotFound();
            }
            Category? categoryDb = _CategoryRepo.Get(u=>u.Id==id);
   
            if(categoryDb == null)
            {
                return NotFound();
            }

			return View(categoryDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
			if (ModelState.IsValid)
            {
				_CategoryRepo.Update(obj);
				_CategoryRepo.Save();

				TempData["success"] = "Category edit successfully";

				return RedirectToAction("Index");
			}
				return View();
		}


       public IActionResult Delete(int? id) 
        {
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Category? categoryDb = _CategoryRepo.Get(u => u.Id == id);

			if (categoryDb == null)
			{
				return NotFound();
			}

			return View(categoryDb);
		}

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id) 
        {
            Category? obj  = _CategoryRepo.Get(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }
			_CategoryRepo.Delete(obj);
            _CategoryRepo.Save();

			TempData["success"] = "Category deleted successfully";

			return RedirectToAction("Index");

		}

	}
}
