using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using MyShop.Core.Models;
using MyShop.Core.Contracts;

namespace MyShop.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> items;
        string className;

        public InMemoryRepository()
        {
            className = typeof(T).Name;
            items = cache[className] as List<T>;
            if (items == null)
                items = new List<T>();
        }

        public void Commit()
        {
            cache[className] = items;
        }
        public void Insert(T item)
        {
            items.Add(item);
        }

        public void Update(T item)
        {
            T itemToUpdate = items.Find(p => p.Id == item.Id);

            if (itemToUpdate != null)
            {
                itemToUpdate = item;
            }
            else
            {
                throw new Exception("Product Category no found");
            }
        }

        public T Find(string Id)
        {
           T itemToFind = items.Find(p => p.Id == Id);

            if (itemToFind != null)
            {
                return itemToFind;
            }
            else
            {
                throw new Exception("Product Category no found");
            }
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete(string Id)
        {
            T itemToDelete = items.Find(p => p.Id == Id);

            if (itemToDelete != null)
            {
                items.Remove(itemToDelete);
            }
            else
            {
                throw new Exception("Product no found");
            }
        }
    }

}
