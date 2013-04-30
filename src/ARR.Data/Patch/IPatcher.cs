using Raven.Abstractions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARR.Data.Repository
{
    public interface IPatcher<TEntity>
    {
        void Patch(TEntity entity, PatchRequest[] patches);
    }
}
