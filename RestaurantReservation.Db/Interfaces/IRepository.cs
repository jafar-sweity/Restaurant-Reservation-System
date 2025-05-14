using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public Task<TEntity> CreatAsync(TEntity entity);
        public Task UpdateAsync(TEntity entity);
        public Task DeleteAsync(TEntity entity);
        public Task<TEntity> GetByIdAsync(int id);
    }
}
