using System;
using System.Collections.Generic;
using System.Linq;
using AtelierEntertainment.DataAccess;

namespace AtelierEntertainment
{
    public class OrderService : IOrderService
    {
        private readonly IOrderDataContext _orderDataContext;

        public OrderService(IOrderDataContext orderDataContext)
        {
            _orderDataContext = orderDataContext;
        }

        //Countries should be extracted to a Class or better use EF to keep a contract between valid values in DB and code.
        //if we think that there will be more country specific rules, we can introduce a new data structure that will keep country name and tax value
        //so it is easy to extend it w/o a need of modifying this class.
        public void CreateOrder(Order order)
        {
            // 1) potentially float can overflow, there will be a precision lost
            // float can overflow when we sum it and when we multiply it (in this code)
            // I would recommend changing it to Decimal
            // or if we really have to keep float, casting float to decimal first and then performing sum and multiplication.
            // I changed the way how total is calculated to mitigate it:
            // old code: order.Total = Convert.ToDecimal(order.Items.Sum(_ => _.Price) * 1.2);
            // new code: order.Total = order.Items.Select(x => CastToDecimal(x.Price)).Sum() * 1.2M;
            // 2)Converting float to decimal directly can loose a precision,  125.609375f cast to decimal becomes 125.6094M
            // old school way to mitigate the risk of precision lost is to first
            // cast float to string as a "round trip" formatter and then create decimal from that string.
            // 3) Also... we don't know who is using this method already, and because I am changing the way how total is calculated
            // calculated total can be different, and it may be problematic for people that used this code in past.
            // Again there are ways to mitigate it... I can elaborate on it more if asked.
            if (order.Customer.Country == Countries.AU)
                order.Total = order.Items.Select(x => CastToDecimal(x.Price)).Sum() * Countries.AUSalesTax;
            else if (order.Customer.Country == Countries.UK)
                order.Total = order.Items.Select(x => CastToDecimal(x.Price)).Sum() * Countries.UKSalesTax;
            else //specification mentioned that Total should be generated for all orders, it means that also for order that are not in named above countries.
                order.Total = order.Items.Select(x => CastToDecimal(x.Price)).Sum();

            //Total is being calculated when we save an order to a DB, but there is no guarantee that some process
            //will add extra orderItems for a given order directly into DB, and due to that when we get order from DB, total will be incorrect.
            //I can elaborate on it more if asked.
            _orderDataContext.CreateOrder(order);
        }

        private decimal CastToDecimal(float price)
        {
            var dec = decimal.Parse(price.ToString("R"));
            return dec;
        }

        //Matt: I am not implementing this method, I can only guess that maybe it was GetOrderById, but if this is true, it should take an id of an order as a parameter
        // without an order id the best I can do is to return a random order or just any order (First?), I don't think that a random order return method is a useful method :)
        // Please look at GetOrderById.
        // It can be also implemented as "select top 1", but again I don't see any value in it.
        public Order ViewOrder()
        {
            throw new NotImplementedException();
        }

        public Order GetOrderById(int id)
        {
            return _orderDataContext.GetOrderById(id);
        }

        public IEnumerable<Order> GetOrdersByCustomerId(int customerId)
        {
            return _orderDataContext.GetOrdersByCustomerId(customerId);
        }
    }
}
