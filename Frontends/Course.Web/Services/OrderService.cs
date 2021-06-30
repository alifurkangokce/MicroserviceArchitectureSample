using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Course.Shared.Dtos;
using Course.Shared.Services;
using Course.Web.Models.FakePayments;
using Course.Web.Models.Orders;
using Course.Web.Services.Interfaces;

namespace Course.Web.Services
{
    public class OrderService:IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly IPaymentService _paymentService;
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _identityService;
        public OrderService(HttpClient httpClient, IPaymentService paymentService, IBasketService basketService, ISharedIdentityService identityService)
        {
            _httpClient = httpClient;
            _paymentService = paymentService;
            _basketService = basketService;
            _identityService = identityService;
        }

        public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput)
        {
            var basket = await _basketService.Get();
            var paymentInfoInput = new PaymentInfoInput()
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                Expiration = checkoutInfoInput.Expiration,
                CVV = checkoutInfoInput.CVV,
                TotalPrice = basket.TotalPrice
            };
            var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);
            if (responsePayment)
            {
                return new OrderCreatedViewModel() {Error = "Ödeme Alınamadı", IsSuccessful = false};
            }

            var orderCreateInput = new OrderCreateInput()
            {
                BuyerId = _identityService.GetUserId,
                Address = new AddressCreateInput
                {
                    Province = checkoutInfoInput.Province, Distinct = checkoutInfoInput.Distinct,
                    Line = checkoutInfoInput.Line, Street = checkoutInfoInput.Street,
                    ZipCode = checkoutInfoInput.ZipCode
                }
            };
            basket.BasketItems.ForEach(x =>
            {
                var orderItem=new OrderItemViewModel()
                {
                    Price = x.Price,ProductId = x.CourseId,PictureUrl = "",
                    ProductName = x.CourseName
                };
                orderCreateInput.OrderItems.Add(orderItem);
            });
            var response = await _httpClient.PostAsJsonAsync<OrderCreateInput>("orders",orderCreateInput);
            if (!response.IsSuccessStatusCode)
            {
                return new OrderCreatedViewModel() { Error = "Sipariş Oluşturulamadı", IsSuccessful = false };
            }
            var orderCreatedViewModel = await response.Content.ReadFromJsonAsync<OrderCreatedViewModel>();
            return orderCreatedViewModel;
        }

        public Task SuspendOrder(CheckoutInfoInput checkoutInfoInput)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OrderViewModel>> GetOrder()
        {
            var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");
            return response.Data;
        }
    }
}
