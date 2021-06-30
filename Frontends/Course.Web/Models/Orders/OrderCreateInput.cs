using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Models.Orders
{
    public class OrderCreateInput
    {
        public OrderCreateInput()
        {
            OrderItems = new List<OrderItemViewModel>();
        }
        public string BuyerId { get; set; }

        public List<OrderItemViewModel> OrderItems { get; set; }

        public AddressCreateInput Address { get; set; }
    }
}
