using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Contracts;
using MyShop.Core.Models;

namespace MyShop.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        internal DataContext context;
        internal DbSet<T> dbset;

        public SQLRepository(DataContext context)
        {
            this.context = context;
            dbset = context.Set<T>();
        }

        public IQueryable<T> Collection()
        {
            return dbset;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(string Id)
        {
            var obj = Find(Id);
            if(context.Entry(obj).State == EntityState.Detached)
            {
                dbset.Attach(obj);
            }
            dbset.Remove(obj);
        }

        public T Find(string Id)
        {
            return dbset.Find(Id);
        }

        public void Insert(T item)
        {
            dbset.Add(item);
        }

        public void Update(T item)
        {
            dbset.Attach(item);
            context.Entry(item).State = EntityState.Modified;
        }
    }
}
