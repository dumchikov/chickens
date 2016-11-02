using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Chicken.Domain.Interfaces;
using Chicken.Domain.Models;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.Linq.Expressions;
using System;

namespace Chicken.DAL
{
    public class EFRepository<T> : IRepository<T> where T : Entity
    {
        protected readonly ChickenDbContext Context;

        public EFRepository(ChickenDbContext context)
        {
            Context = context;
            //Context.Configuration.AutoDetectChangesEnabled = true;
            //Context.Configuration.ValidateOnSaveEnabled =  false;
        }

        public virtual IQueryable<T> Query()
        {
            var query = Context.Set<T>();
            return query;
        }

        public virtual T GetById(int id)
        {
            var query = Context.Set<T>().SingleOrDefault(x => x.Id == id);
            return query;
        }

        public virtual void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            Context.Set<T>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
