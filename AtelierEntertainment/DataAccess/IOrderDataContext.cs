using System.Collections.Generic;

namespace AtelierEntertainment.DataAccess
{
    public interface IOrderDataContext
    {
        void CreateOrder(Order order);
        Order GetOrderById(int id);
        IEnumerable<Order> GetOrdersByCustomerId(int customerId);
    }
}
