using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AbbyWeb.Pages.Admin.MenuItems
{
    [BindProperties]
    public class UpsertModel : PageModel
    {
        #region Constructor Dependency Injection

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UpsertModel(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            MenuItem = new MenuItem();
        }

        #endregion

        public MenuItem MenuItem { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> FoodTypeList { get; set; }

        public void OnGet(int? id)
        {
            if (id != null)
            {
                // Edit Request
                MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(u => u.Id == id);
            }

            // Populate Category dropdown list
            CategoryList = _unitOfWork.Category.GetAll()
                .Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });

            // Populate FoodType dropdown list
            FoodTypeList = _unitOfWork.FoodType.GetAll()
                .Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
        }

        public IActionResult OnPost()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (MenuItem.Id == 0)
            {
                // Create Request
                string newFileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, @"images\menuItems");
                var extension = Path.GetExtension(files[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(uploads, newFileName + extension), FileMode.Create))
                {
                    files[0].CopyToAsync(fileStream);
                }
                MenuItem.Image = $@"\images\menuItems\{newFileName}{extension}";
                _unitOfWork.MenuItem.Add(MenuItem);
            }
            else
            {
                // Edit Request
                var objFromDb = _unitOfWork.MenuItem.GetFirstOrDefault(u => u.Id == MenuItem.Id);
                if (files.Count > 0)
                {
                    // Delete the old image
                    var oldImagePath = Path.Combine(webRootPath, objFromDb.Image.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                    // New upload
                    string newFileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\menuItems");
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(uploads, newFileName + extension), FileMode.Create))
                    {
                        files[0].CopyToAsync(fileStream);
                    }
                    MenuItem.Image = $@"\images\menuItems\{newFileName}{extension}";
                }
                else // If no image file is selected, set the Image path to the old path (just to be on the safe side)
                {
                    MenuItem.Image = objFromDb.Image;
                }
                _unitOfWork.MenuItem.Update(MenuItem);
            }

            _unitOfWork.Save(); // Save the changes to the DB

            return RedirectToPage("Index");
        }
    }
}
