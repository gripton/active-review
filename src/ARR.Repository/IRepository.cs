using ARR.Data.Repository;
using Raven.Abstractions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARR.Repository
{
    public interface IRepository<TEntity> : IReadContext<TEntity>, IPatcher<TEntity>
    {
        void Save(TEntity entity);        
        void Delete(TEntity entity);
    }
}
