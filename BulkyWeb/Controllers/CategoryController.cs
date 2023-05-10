using Bulky.DataAcess.Data;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;    
        }

        public IActionResult Index()
        {
            //get all categorys
            List<Category> objCategoryList = _db.Categories.ToList();
             
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
				_db.Categories.Add(obj);
				_db.SaveChanges();
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
            Category? categoryDb = _db.Categories.Find(id);
            //Category? categoryDb = _db.Categories.FirstOrDefault(u=>u.Id == id);
            //Category? categoryDb = _db.Categories.Where(u => u.Id == id).FirstOrDefault();    
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
                _db.Categories.Update(obj);
                _db.SaveChanges();

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
			Category? categoryDb = _db.Categories.Find(id);

			if (categoryDb == null)
			{
				return NotFound();
			}

			return View(categoryDb);
		}

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id) 
        {
            Category? obj  = _db.Categories.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            _db.SaveChanges();

			TempData["success"] = "Category deleted successfully";

			return RedirectToAction("Index");

		}

	}
}
