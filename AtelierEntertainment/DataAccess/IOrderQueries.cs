using System.Collections.Generic;
using System.Data;

namespace AtelierEntertainment.DataAccess
{
    public interface IOrderQueries
    {
        Order GetOrderById(IDbConnection conn, int id);
        IEnumerable<Order> GetOrdersByCustomerId(IDbConnection conn, int customerId);
        IEnumerable<orderItem> GetOrderItemsByOrderId(IDbConnection conn, int id);
    }
}