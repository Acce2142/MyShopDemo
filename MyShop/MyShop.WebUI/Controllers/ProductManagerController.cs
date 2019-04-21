using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        ProductRepository context;
        public ProductManagerController()
        {
            context = new ProductRepository();
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            Product product = new Product();
            return View(product);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                context.Insert(product);
                context.Commit();
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

        public ActionResult Edit(string id)
        {
            Product target = context.Find(id);
            if (target == null)
            {
                return HttpNotFound("Product not found");
            }
            else
            {
                return View(target);
            }
        }

        [HttpPost]
        public ActionResult Edit(Product target, string id)
        {
            Product targetToEdit = context.Find(id);
            if (target == null)
            {
                return HttpNotFound("Product not found");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    targetToEdit.Image = target.Image;
                    targetToEdit.Name = target.Name;
                    targetToEdit.price = target.price;
                    targetToEdit.Description = target.Description;
                    targetToEdit.Category = target.Category;
                    context.Commit();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(targetToEdit);
                }
            }
        }

        public ActionResult Delete (string id)
        {
            Product target = context.Find(id);
            if (target == null)
            {
                return HttpNotFound("Product not found");
            }
            else
            {
                return View(target);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(Product target, string id)
        {
            Product targetToDelete = context.Find(id);
            if (target == null)
            {
                return HttpNotFound("Product not found");
            }
            else
            {
                context.Delete(id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}