using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Course.Web.Models.Orders;
using Course.Web.Services.Interfaces;

namespace Course.Web.Services
{
    public class OrderService:IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput)
        {
            throw new NotImplementedException();
        }

        public Task SuspendOrder(CheckoutInfoInput checkoutInfoInput)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderViewModel>> GetOrder()
        {
            throw new NotImplementedException();
        }
    }
}
