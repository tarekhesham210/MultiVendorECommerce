using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PermissionBasedAuz.Areas.Admin.Services;
using PermissionBasedAuz.Areas.Admin.ViewModels;
using PermissionBasedAuz.Constants;
using PermissionBasedAuz.Models;
using System.Threading.Tasks;

namespace PermissionBasedAuz.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<IActionResult> Index()
        {
            var roles =await _roleService.GetAllRoles();

            return View(roles);
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            var rolesType = _roleService.GetRoleTypes();
            return View(rolesType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(NewRoleVM role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }
             await _roleService.CreateRoleAsync(role);
            return RedirectToAction("Index");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                ModelState.AddModelError("", "Invalid role id");
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");

            }
            await _roleService.DeleteRole(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> ManageRolePermissions(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                ModelState.AddModelError("", "Invalid role id");
            }
            if (!ModelState.IsValid) 
            {
                return RedirectToAction("Index");
            }
          var model=await _roleService.ManageRolePermission(id);

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRolePermissions(RolePermissionsVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            HttpContext.Items["CurrentModel"] = model;
            await _roleService.UpdateRolePermissions(model);
            return RedirectToAction("Index");
        }

    }
}
