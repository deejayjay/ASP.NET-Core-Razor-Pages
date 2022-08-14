using Abby.DataAccess.Data;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abby.DataAccess.Repository
{
    public class FoodTypeRepository : Repository<FoodType>, IFoodTypeRepository
    {
        #region Constructor Dependency Injection

        private readonly ApplicationDbContext _db;
        public FoodTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        #endregion

        public void Update(FoodType obj)
        {
            var objFromDb = _db.FoodTypes.FirstOrDefault(item => item.Id == obj.Id);
            objFromDb.Name = obj.Name;
        }
    }
}
