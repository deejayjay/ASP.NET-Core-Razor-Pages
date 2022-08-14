using Abby.DataAccess.Data;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;

namespace Abby.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        #region Constructor Dependency Injection
        
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        } 

        #endregion

        public void Update(Category obj)
        {
            var objFromDb = _db.Categories.FirstOrDefault(item => item.Id == obj.Id);
            objFromDb.Name = obj.Name;
            objFromDb.DisplayOrder = obj.DisplayOrder;
        }
    }
}
