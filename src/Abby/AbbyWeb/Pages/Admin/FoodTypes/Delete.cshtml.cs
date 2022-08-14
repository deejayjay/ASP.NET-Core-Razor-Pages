using Abby.DataAccess.Data;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.FoodTypes
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
        public FoodType FoodType { get; set; }

        public void OnGet(int id)
        {
            FoodType = _unitOfWork.FoodType.GetFirstOrDefault(ft => ft.Id == id);
        }

        public IActionResult OnPost()
        {
            var existingFoodType = _unitOfWork.FoodType.GetFirstOrDefault(ft => ft.Id == FoodType.Id);
            if (existingFoodType != null)
            {
                _unitOfWork.FoodType.Remove(existingFoodType);
                _unitOfWork.Save();
                TempData["success"] = "Food Type deleted successfully";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
