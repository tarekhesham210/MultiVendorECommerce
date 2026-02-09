using Microsoft.AspNetCore.Mvc;
using MultiVendorECommerce.Areas.Admin.ViewModels;
using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.Areas.Admin.Services;
using MultiVendorECommerce.Shared.Services.Interfaces;

namespace MultiVendorECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;
        private readonly ICategoryQueryService _categoryQueryService;

        public CategoryController(CategoryService categoryService, ICategoryQueryService categoryQueryService)
        {
            _categoryService = categoryService;
            _categoryQueryService = categoryQueryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categoryVm = await _categoryService.CreateCategory();
            return View(categoryVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryWithAttributeVM categoeryVM)
        {
            if (!ModelState.IsValid)
            {
                return View(categoeryVM);
            }
            await _categoryService.AddCategoryAsync(categoeryVM);
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Edit(int id)
        {
            if(id<=0)
                return NotFound();           
            var categoryVm = await _categoryService.GetCategoryByIdForEditAsync(id);
            return View(categoryVm);
              
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCategoryVM categoryVM)
        {
            if (!ModelState.IsValid)
            {
                categoryVM.ParentCategories = await _categoryQueryService.GetAllCategoriesforSelection();
                return View(categoryVM);
            }
            try
            {
                await _categoryService.UpdateCategoryAsync(categoryVM.Id, categoryVM);
                return RedirectToAction(nameof(Index));
            }

            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                categoryVM.ParentCategories = await _categoryQueryService.GetAllCategoriesforSelection();

                return View(categoryVM);
            }
            catch (BusinessRuleException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                categoryVM.ParentCategories = await _categoryQueryService.GetAllCategoriesforSelection();
                return View(categoryVM);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                TempData["SuccessMessage"] = "Category deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }
    }
}
