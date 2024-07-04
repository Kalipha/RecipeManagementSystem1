using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeManagementSystem.Context;
using RecipeManagementSystem.Data;
using RecipeManagementSystem.Models.Recipe;
using System.Security.Claims;


namespace RecipeManagement_System.Controllers
{
    public class RecipeController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly RMSDbContext _rmsDbContext;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public RecipeController(
            INotyfService notyf,
            RMSDbContext rmsDbContext)
        //IHttpContextAccessor httpContextAccessor)
        {
            _notyfService = notyf;
            _rmsDbContext = rmsDbContext;
            //_httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var recipe = _rmsDbContext.Recipes.ToList();
            return View();
        }

        [Authorize(Roles = "Creator")]
        public IActionResult Create()
        {
            ViewBag.Categories = _rmsDbContext.Categories.ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> Create(RecipeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                string imagePath = null;
                if (model.Image != null)
                {
                    string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    imagePath = Path.Combine(uploadFolder, Guid.NewGuid().ToString() + "_" + model.Image.FileName);
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(fileStream);
                    }
                    imagePath = "/images/" + Path.GetFileName(imagePath);
                }
                var newRecipe = new Recipe
                {
                    RecipeName = model.RecipeName,
                    Ingredient = model.Ingredients,
                    Description = model.Description,
                    Procedure = model.Procedure,
                    CategoryId = model.CategoryId,
                    OwnerId = userId,
                    ImagePath = imagePath
                };

                _rmsDbContext.Recipes.Add(newRecipe);
                await _rmsDbContext.SaveChangesAsync();

                _notyfService.Success("Recipe created successfully!");

                return RedirectToAction("Index");
            }
            ViewBag.Categories = _rmsDbContext.Categories.ToList();
            return View(model);
        }
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var recipe = await _rmsDbContext.Recipes.FirstOrDefaultAsync(m => m.Id == id && m.OwnerId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (recipe == null)
            {
                return Unauthorized();
            }
            var viewModel = new RecipeViewModel
            {
                Id = recipe.Id,
                RecipeName = recipe.RecipeName,
                Description = recipe.Description,
                Ingredients = recipe.Ingredient,
                CategoryId = recipe.CategoryId,
                Procedure = recipe.Procedure
            };

            return View(viewModel);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _rmsDbContext.Recipes.FirstOrDefaultAsync(m => m.Id == id && m.OwnerId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (recipe == null)
            {
                _rmsDbContext.Recipes.Remove(recipe);
                await _rmsDbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> Edit(int? id, RecipeViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var recipe = await _rmsDbContext.Recipes.FindAsync(id);
                if (recipe == null || recipe.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    return Unauthorized();
                }

                string imagepath = recipe.ImagePath;
                if (model.Image != null)
                {
                    if (!string.IsNullOrEmpty(recipe.ImagePath))
                    {
                        var OldImagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", recipe.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(OldImagepath))
                        {
                            System.IO.File.Delete(OldImagepath);
                        }
                    }

                    string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", recipe.ImagePath.TrimStart('/'));
                    imagepath = Path.Combine(uploadFolder, Guid.NewGuid().ToString() + "_" + model.Image.FileName);
                    using (var fileStream = new FileStream(imagepath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(fileStream);
                    }
                    imagepath = "/images/" + Path.GetFileName(imagepath);
                }

                recipe.RecipeName = model.RecipeName;
                recipe.Description = model.Description;
                recipe.Ingredient = model.Ingredients;
                recipe.Procedure = model.Procedure;
                recipe.CategoryId = model.CategoryId;
                recipe.ImagePath = imagepath;

                _rmsDbContext.Update(recipe);
                await _rmsDbContext.SaveChangesAsync();

                _notyfService.Success("Recipe Updated Successfully!");
                return RedirectToAction("Index");

            }
            ViewBag.Categories = _rmsDbContext.Categories.ToList();
            return View(model);
        }

    }

}

