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
        private readonly bool _isUnitOfWork;

        protected IDocumentStore _store;
        protected IDocumentSession _session;

        protected AbstractRepository(IDocumentSession session)
        {
            _isUnitOfWork = true;
            _session = session;
            Initialize();
        }

        protected AbstractRepository(IDocumentStore store)
        {
            _store = store;
            Initialize();
        }

        private void Initialize()
        {
             PatchDictionary = new Dictionary<string, Func<TEntity, PatchRequest[]>>();
            InitializePatchFunctions();
        }

        protected virtual void InitializePatchFunctions() { }

        protected virtual IDictionary<string, Func<TEntity, PatchRequest[]>> PatchDictionary { get; set; }

        public virtual void Save(TEntity entity)
        {
            if(_isUnitOfWork)
           {
               Save(entity, _session);
           }
           else
           {
               using(var session = _store.OpenSession())
               {
                   Save(entity, session);
               }
           }
        }

        private void Save(TEntity entity, IDocumentSession session)
        {
            session.Store(entity);
            session.SaveChanges();
        }

        public virtual void Patch(TEntity entity, string patchKey)
        {
           if(_isUnitOfWork)
           {
               Patch(entity, patchKey, _session);
           }
           else
           {
               using(var session = _store.OpenSession())
               {
                   Patch(entity, patchKey, session);
               }
           }
        }

        private void Patch(TEntity entity, string patchKey, IDocumentSession session)
        {
            const string patchFormat = "{0}s/{1}";

            var patchString = string.Format(patchFormat, typeof(TEntity).Name.ToLower(), entity.Id);
            var patches = PatchDictionary[patchKey].Invoke(entity);

            session
               .Advanced
               .DocumentStore
               .DatabaseCommands
               .Patch(patchString, patches);

            session.SaveChanges();
        }

        public virtual void Delete(TEntity entity)
        {
           if(_isUnitOfWork)
           {
               Delete(entity, _session);
           }
           else
           {
               using(var session = _store.OpenSession())
               {
                   Delete(entity, session);
               }
           }
        }

        private void Delete(TEntity entity, IDocumentSession session)
        {
            session.Delete(entity);
            session.SaveChanges();
        }

        public virtual TEntity Get(int id)
        {
            if (_isUnitOfWork)
            {
                return Get(id, _session);
            }

            using (var session = _store.OpenSession())
            {
                return Get(id, session);
            }
        }

        private TEntity Get(int id, IDocumentSession session)
        {
            return _session.Load<TEntity>(id);
        }

        public virtual TEntity GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<TEntity> Find(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null)
        {
            if (_isUnitOfWork)
            {
                return Find(_session, predicate);
            }

            using (var session = _store.OpenSession())
            {
                return Find(session, predicate);
            }
        }

        private IQueryable<TEntity> Find(IDocumentSession session, System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null ? session.Query<TEntity>() : session.Query<TEntity>().Where(predicate);
        }

        public virtual IQueryable<TEntity> FindAll()
        {
            return Find();
        }

        public virtual List<TEntity> List(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null)
        {
            return Find(predicate).ToList();
        }

        public virtual List<TEntity> ListAll()
        {
            return Find().ToList();
        }
        
    }
}
