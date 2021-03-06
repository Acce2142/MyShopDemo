﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MyShop.Core;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;

namespace MyShop.Servers
{
    public class BasketService : IBasketService
    {
        IRepository<Product> ProductContext;
        IRepository<Basket> BasketContext;
        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepository<Product> ProductContext, IRepository<Basket> BasketContext)
        {
            this.BasketContext = BasketContext;
            this.ProductContext = ProductContext;
        }

        private Basket GetBasket(HttpContextBase httpContext, bool IfNull)
        {
            HttpCookie cookies = httpContext.Request.Cookies.Get(BasketSessionName);
            Basket basket = new Basket();
            if(cookies != null)
            {
                string basketID = cookies.Value;
                if (!string.IsNullOrEmpty(basketID))
                {
                    basket = BasketContext.Find(basketID);
                } else
                {
                    if (IfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else
            {
                if (IfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();
            BasketContext.Insert(basket);
            BasketContext.Commit();
            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(7);
            httpContext.Response.Cookies.Add(cookie);
            return basket;
        }

        public void AddToBasket(HttpContextBase httpcontext, string productId)
        {
            Basket basket = GetBasket(httpcontext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductID == productId);
            if(item == null)
            {
                item = new BasketItem();
                item.ProductID = productId;
                item.Quanity = 1;
                item.BasketID = basket.Id;
                basket.BasketItems.Add(item);
            }

            else
            {
                item.Quanity = item.Quanity + 1;
            }
            BasketContext.Commit();
        }

        public void RemoveFromBasket(HttpContextBase httpcontext, string itemId)
        {
            Basket basket = GetBasket(httpcontext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                basket.BasketItems.Remove(item);
                BasketContext.Commit();
            }
        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            if (basket != null)
            {
                var result = (from b in basket.BasketItems
                              join p in ProductContext.Collection() on b.ProductID equals p.Id
                              select new BasketItemViewModel()
                              {
                                  id = b.Id,
                                  Quanity = b.Quanity,
                                  ProductName = p.Name,
                                  Image = p.Image,
                                  Price = p.Price

                              }
                             ).ToList();
                return result;
               
            }
            else
            {
                return new List<BasketItemViewModel>();
            }
        }

        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            BasketSummaryViewModel model = new BasketSummaryViewModel(0, 0);
            if (basket != null)
            {
                int? BasketCount = (from item in basket.BasketItems
                                    select item.Quanity).Sum();
                decimal? BasketTotal = (from item in basket.BasketItems
                                        join p in ProductContext.Collection() on item.ProductID equals p.Id
                                        select item.Quanity * p.Price).Sum();
                model.BasketCount = BasketCount ?? 0;
                model.BasketTotal = BasketTotal ?? decimal.Zero;
                return model;
            }
            else
            {
                return model;
            }
        }
    }

}
