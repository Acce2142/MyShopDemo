using System.Collections.Generic;
using System.Web;
using MyShop.Core.ViewModels;

namespace MyShop.Servers
{
    public interface IBasketService
    {
        void AddToBasket(HttpContextBase httpcontext, string productId);
        List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext);
        BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext);
        void RemoveFromBasket(HttpContextBase httpcontext, string itemId);
    }
}