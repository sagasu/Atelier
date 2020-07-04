using System.Data;

namespace AtelierEntertainment.DataAccess
{
    // Inserting w/o providing column names is order based, this is not good. Consider refactoring.
    public class OrderCommands : IOrderCommands
    {
        public void AddOrderItems(Order order, IDbConnection conn)
        {
            foreach (var item in order.Items)
            {
                var cmd = conn.CreateCommand();

                cmd.CommandText =
                    $"INSERT INTO dbo.OrderItems VALUES {order.Id}, {item.Code}, {item.Description}, {item.Price};";

                cmd.ExecuteNonQuery();
            }
        }

        public void AddOrder(Order order, IDbConnection conn)
        {
            var cmd = conn.CreateCommand();

            cmd.CommandText = $"INSERT INTO dbo.Orders VALUES {order.Id}, {order.Customer.Id}, {order.Total}";

            cmd.ExecuteNonQuery();
        }
    }
}
