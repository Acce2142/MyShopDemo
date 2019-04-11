using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Products> products;
        public ProductRepository()
        {
            products = cache["products"] as List<Products>;
            if(products == null)
            {
                products = new List<Products>();
            }
        }

        public void Commit()
        {
            cache["products"] = products;
        }
        public void Insert(Products p)
        {
            products.Add(p);
        }

        public void Update(Products product)
        {
            Products productToUpdate = products.Find(p => p.Id == product.Id);
            if(productToUpdate != null)
            {
                productToUpdate = product;
            } else
            {
                throw new Exception("Product not found");
            }
        }
        
        public Products Find(string id)
        {
            Products target = products.Find(p => p.Id == id);
            if(target == null)
            {
                throw new Exception("Product not found");
            } else
            {
                return target;
            }
        }

        public IQueryable<Products> Collection()
        {
            return products.AsQueryable();
        }

        public void Delete(string id)
        {
            Products productToDelete = products.Find(p => p.Id == id);
            if (productToDelete != null)
            {
                products.Remove(productToDelete);
            }
            else
            {
                throw new Exception("Product not found");
            }
        }
    }
    
}
