using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IO.Compression;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.Role_Admin)]
    public class ProductController: Controller
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
            var products = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            //ViewBag.CategoryList = _unitOfWork.Category.GetAll()
            //        .Select(x => new SelectListItem
            //        {
            //            Text = x.Name,
            //            Value = x.Id.ToString()
            //        });
            //foreach(Product product in products)
            //{
            //    product.Category = _unitOfWork.Category.Get(x => x.Id == product.CategoryId);
            //}
            
            return View(products);
        }

        //Update and Insert
        public IActionResult Upsert(int? id)
        {

            //ViewBag.CategoryList = categoryList;

            //ViewData[nameof(categoryList)] = categoryList;
            var productViewModel = new ProductViewModel
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll()
                         .OrderBy(x => x.DisplayOrder)
                         .Select(x => new SelectListItem
                         {
                             Text = x.Name,
                             Value = x.Id.ToString()
                         }),
            };
            if (id == null || id == 0)
            {
                //Create (Insert)
                return View(productViewModel);
            }
            else
            {
                //Update
                productViewModel.Product = _unitOfWork.Product.Get(x => x.Id == id);
                if(productViewModel.Product == null)
                {
                    return NotFound();
                }
                return View(productViewModel);
            }
			
        }
        [HttpPost]
        public IActionResult Upsert(ProductViewModel productViewModel, IFormFile? file)
        {
            if(ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString().Split("-")[0] + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\products");

                    if(!string.IsNullOrEmpty(productViewModel.Product.ImageUrl))
                    {
                        //delete old img
                        var oldImgPath = 
                            Path.Combine(wwwRootPath, productViewModel.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);
                        }
					}

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    //File.WriterAllText()
                    //productViewModel.Product.ImageUrl = Path.Combine(productPath, fileName);
                    productViewModel.Product.ImageUrl = @"\images\products\" + fileName;

				}
                else
                {
                    productViewModel.Product.ImageUrl = "";
                }

                if(productViewModel.Product.Id == 0)
                {
					_unitOfWork.Product.Add(productViewModel.Product);
					_unitOfWork.Save();
					TempData["success"] = "Product created successfully";
				}
                else
                {
					_unitOfWork.Product.Update(productViewModel.Product);
					_unitOfWork.Save();
					TempData["success"] = "Product Updated successfully";
				}
				
				return RedirectToAction("Index");
			}
            else
            {

                productViewModel.CategoryList = _unitOfWork.Category.GetAll()
                        .Select(x => new SelectListItem
                        {
                            Text = x.Name,
                            Value = x.Id.ToString()
                        });
				return View(productViewModel);
			}
           
        }

		//public IActionResult Delete(int id)
		//{
		//	if (id == null)
		//	{
		//		return NotFound();
		//	}
		//	var product = _unitOfWork.Product.Get(x => x.Id == id);
  //          if(product == null)
  //          {
  //              return NotFound();
  //          }
		//	return View(product);
		//}
		//[HttpPost]
		//public IActionResult Delete(Product product)
		//{
		//	var _product = _unitOfWork.Product.Get(x => x.Id == product.Id);
  //          if(_product == null) 
  //          {
  //              return NotFound();
  //          }

		//	_unitOfWork.Product.Remove(_product);
		//	_unitOfWork.Save();
  //          TempData["success"] = "Product deleted successfully";
  //          return RedirectToAction("Index");
		//}

        #region APIs Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data =  products });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var product = _unitOfWork.Product.Get(x => x.Id == id);
            if (product == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var wwwRootPath = _webHostEnvironment.WebRootPath;

            var oldImgPath = Path
                .Combine(wwwRootPath, product.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImgPath))
            {
                System.IO.File.Delete(oldImgPath);
            }

            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Deleted successfully" });
        }


        #endregion
    }
}
