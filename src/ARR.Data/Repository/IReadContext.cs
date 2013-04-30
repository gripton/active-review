using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ARR.Data.Repository
{
    public interface IReadContext<TEntity>
    {
        TEntity Get(int id);
        TEntity GetByName(string name);
        
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate = null);
        IQueryable<TEntity> FindAll();
        
        List<TEntity> List(Expression<Func<TEntity, bool>> predicate = null);
        List<TEntity> ListAll();
    }
}
