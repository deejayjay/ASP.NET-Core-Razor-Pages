using Abby.DataAccess.Data;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.Categories
{
    public class DeleteModel : PageModel
    {
        #region Constructor Dependency Injection

        private readonly IUnitOfWork _unitOfWork;
        public DeleteModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        [BindProperty]
        public Category Category { get; set; }
        public void OnGet(int id)
        {
            Category = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
        }

        public IActionResult OnPost()
        {
            var existingCategory = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == Category.Id);
            if (existingCategory != null)
            {
                _unitOfWork.Category.Remove(existingCategory);
                _unitOfWork.Save();
                TempData["success"] = "Category deleted successfully";
                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}
