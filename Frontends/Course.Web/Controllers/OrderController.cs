using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.Web.Models.Orders;
using Course.Web.Services.Interfaces;

namespace Course.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }

        public async Task<IActionResult> CheckOut()
        {
            var basket = await _basketService.Get();
            ViewBag.basket = basket;
            return View(new CheckoutInfoInput());
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(CheckoutInfoInput checkoutInfoInput)
        {
            var orderStatus = await _orderService.CreateOrder(checkoutInfoInput);
            if (!orderStatus.IsSuccessful)
            {
                var basket = await _basketService.Get();
                ViewBag.basket = basket;
                ViewBag.error = orderStatus.Error;
                return View();
            }

            return RedirectToAction(nameof(SuccessfulCheckout),new {orderId=orderStatus.OrderId});


        }

        public IActionResult SuccessfulCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }
    }
}
