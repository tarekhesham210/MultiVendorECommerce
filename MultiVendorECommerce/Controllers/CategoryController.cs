using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
//using MultiVendorECommerce.Repository;
using System.Threading.Tasks;
using MultiVendorECommerce.Services;
using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.Areas.Admin.ViewModels;

namespace MultiVendorECommerce.Controllers
{
    //public class CategoryController : Controller
    //{
    //    private readonly CategoryService _categoryService;

    //    public CategoryController(CategoryService categoryService)
    //    {
    //        _categoryService = categoryService;
    //    }

    //    public async Task<IActionResult> Index()
    //    {
    //        var categories = await _categoryService.GetParentCategoriesAsync();
    //        return View(categories);
    //    }

    //    [HttpGet]
    //    public async Task<IActionResult> Create()
    //    {
    //        var categoryVm = await _categoryService.CreateCategory();
    //        return View(categoryVm);
    //    }
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create(CategoryVM categoeryVM)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return View(categoeryVM);
    //        }
    //        try
    //        {
    //            await _categoryService.AddCategoryAsync(categoeryVM);
    //            return RedirectToAction(nameof(Index));

    //        }
    //        catch (DomainException ex)
    //        {
    //            ModelState.AddModelError(string.Empty, ex.Message);
    //            return View(categoeryVM);
    //        }
          

    //    }
    //    public async Task<IActionResult> Edit(int id)
    //    {
    //        try
    //        {
    //            var categoryVm = await _categoryService.GetCategoryByIdAsync(id);
    //            return View(categoryVm);
    //        }
    //        catch (NotFoundException ex)
    //        {
    //            return NotFound(ex.Message);
    //        }
    //    }
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit( CategoryVM categoryVM)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            categoryVM.ParentCategories = await _categoryService.GetAllCategoriesforSelection();
    //            return View(categoryVM);
    //        }
    //        try
    //        {
    //            await _categoryService.UpdateCategoryAsync(categoryVM.Id, categoryVM);
    //            return RedirectToAction(nameof(Index));
    //        }

    //        catch (NotFoundException ex)
    //        {
    //            ModelState.AddModelError(string.Empty, ex.Message);
    //            categoryVM.ParentCategories = await _categoryService.GetAllCategoriesforSelection();

    //            return View(categoryVM);
    //        }
    //        catch (BusinessRuleException ex)
    //        {
    //            ModelState.AddModelError(string.Empty, ex.Message);
    //           categoryVM.ParentCategories=await _categoryService.GetAllCategoriesforSelection();
    //            return View(categoryVM);
    //        }
    //    }
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Delete(int id)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return RedirectToAction(nameof(Index));
    //        }
    //        try
    //        {
    //            await _categoryService.DeleteCategoryAsync(id);
    //            TempData["SuccessMessage"] = "Category deleted successfully.";
    //            return RedirectToAction(nameof(Index));
    //        }
    //        catch (NotFoundException ex)
    //        {
    //            return NotFound(ex.Message);
    //        }
            
    //    }
    //}
}
