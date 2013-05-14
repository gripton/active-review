using System;
using System.Collections.Generic;
using System.Linq;

using ARR.Data.Entities;

using Raven.Abstractions.Data;
using Raven.Client;

namespace ARR.Repository
{
    public abstract class AbstractRepository<TEntity> : IRepository<TEntity> where TEntity : class, IPersistentEntity
    {
        private readonly IDocumentSession _session;

        protected AbstractRepository(IDocumentSession session)
        {
            _session = session;
            PatchDictionary = new Dictionary<string, Func<TEntity, PatchRequest[]>>();
            InitializePatchFunctions();
        }

        protected virtual void InitializePatchFunctions() { }

        protected IDictionary<string, Func<TEntity, PatchRequest[]>> PatchDictionary { get; set; }

        public void Save(TEntity entity)
        {
            _session.Store(entity);
            _session.SaveChanges();
        }

        public void Patch(TEntity entity, string patchKey)
        {
            const string patchFormat = "{0}s/{1}";

            var patchString = string.Format(patchFormat, typeof(TEntity).Name.ToLower(), entity.Id);
            var patches = PatchDictionary[patchKey].Invoke(entity);

            _session
               .Advanced
               .DocumentStore
               .DatabaseCommands
               .Patch(patchString, patches);

            _session.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
           _session.Delete<TEntity>(entity);
            _session.SaveChanges();
        }

        public TEntity Get(int id)
        {
            return _session.Load<TEntity>(id);
        }

        public virtual TEntity GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Find(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null ? _session.Query<TEntity>() : _session.Query<TEntity>().Where(predicate);
        }

        public IQueryable<TEntity> FindAll()
        {
            return Find();
        }

        public List<TEntity> List(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null)
        {
            return Find(predicate).ToList();
        }

        public List<TEntity> ListAll()
        {
            return Find().ToList();
        }
    }
}
