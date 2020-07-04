using System.Collections.Generic;
using System.Data;
using AtelierEntertainment.Providers;

namespace AtelierEntertainment.DataAccess
{
    public class OrderQueries : IOrderQueries
    {
        public Order GetOrderById(IDbConnection conn, int id)
        {
            var cmd = conn.CreateCommand();

            cmd.CommandText = $"SELECT * FROM dbo.Orders WHERE Id = {id}";

            var order = new Order();
            using (var reader = cmd.ExecuteReader())
            {
                order.Id = id;
                order.Total = reader.GetDecimal(reader.GetOrdinal(PropertyNameProvider.GetPropertyName(() => order.Total)));
            }

            return order;
        }

        public IEnumerable<Order> GetOrdersByCustomerId(IDbConnection conn, int customerId)
        {
            var cmd = conn.CreateCommand();

            cmd.CommandText = $"SELECT o.Id as id, o.Total as Total FROM dbo.Orders as o join Customer as c on a.Id = c.Id Where c.Id = {customerId}";

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var order = new Order();
                    order.Id = int.Parse(reader[PropertyNameProvider.GetPropertyName(() => order.Id)].ToString());
                    order.Total = decimal.Parse(reader[PropertyNameProvider.GetPropertyName(() => order.Total)].ToString());
                    yield return order;
                }
            }
        }

        public IEnumerable<orderItem> GetOrderItemsByOrderId(IDbConnection conn, int id)
        {
            var cmd = conn.CreateCommand();

            cmd.CommandText = $"SELECT * FROM dbo.OrderItems WHERE OrderId = {id}";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var orderItem = new orderItem();
                    orderItem.Code =
                        reader.GetString(
                            reader.GetOrdinal(PropertyNameProvider.GetPropertyName(() => orderItem.Code)));
                    orderItem.Description =
                        reader.GetString(
                            reader.GetOrdinal(PropertyNameProvider.GetPropertyName(() => orderItem.Description)));
                    orderItem.Price =
                        reader.GetFloat(reader.GetOrdinal(PropertyNameProvider.GetPropertyName(() => orderItem.Price)));

                    yield return orderItem;
                }
            }
        }
    }
}
