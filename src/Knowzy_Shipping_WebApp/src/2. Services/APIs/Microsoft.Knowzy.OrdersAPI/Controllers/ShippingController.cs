using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Knowzy.OrdersAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Knowzy.OrdersAPI.Controllers
{
    [Route("api/[controller]")] //that will be /api/Shipping
    public class ShippingController : Controller
    {
        private IOrdersStore _ordersStore;

        public ShippingController(IOrdersStore ordersStore)
        {
            _ordersStore = ordersStore;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Domain.Shipping> Get()
        {
            return _ordersStore.GetShippings();
        }

    }
}