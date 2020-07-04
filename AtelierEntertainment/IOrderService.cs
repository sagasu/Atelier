using System.Collections.Generic;

namespace AtelierEntertainment
{
    public interface IOrderService
    {
        void CreateOrder(Order order);
        Order GetOrderById(int id);
        IEnumerable<Order> GetOrdersByCustomerId(int customerId);
    }
}
