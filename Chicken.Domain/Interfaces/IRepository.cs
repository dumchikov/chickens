using System.Linq;
using System.Threading.Tasks;
using Chicken.Domain.Models;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;

namespace Chicken.Domain.Interfaces
{
    public interface IRepository<T> where T : Entity
    {
        IQueryable<T> Query();

        T GetById(int id);

        void Add(T entity);

        void Delete(T entity);

        void Edit(T entity);

        void Save();

        Task SaveAsync();
    }
}
