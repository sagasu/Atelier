using System.Collections.Generic;
using AtelierEntertainment.DataAccess;
using AtelierEntertainment.Factories;

namespace AtelierEntertainment
{
    //Matt: To be honest I haven't seen so badly written class in a long time
    // 1) Accessing properties in DB by column order
    // 2) Not disposing opened streams
    // 3) DataContext lifestyle should by managed by the DI - it means that we should inject it here thru constructor and consume it.
    // 4) LoadOrder method is Static
    // 5) No transactions when inserting to multiple tables.
    // 6) I am not a fan of using pure SQL when we are using EF, if it was Dapper, maybe..., but for EF default should be LINQ.
    // 7) Connection string should be kept in a config file
    // 8) Operations on DB/streams are not async
    // 9) Two Select queries in LoadOrder could be executed in our query using Join. I don't want to spend too much time refactoring it, If I had to I would prefer to invest time introducing EF.
    // 10) We are accessing columns by it's order (that is a huge no go), in original code both Description and Price had column order 2 assigned
    //     It just shows how easy it is to make a mistake here. I don't like to use magic strings also, that's why I am here doing it with reflection to be able to maintain this code easily later on.
    // 11) `result` is a bad property name
    // 12) I think that LoadOrder method name should be named GetOrderById. LoadOrder is a bad name.
    // 13) Entire class was untestable, because I was not allowed to change the signature of the class, I am using property DI to add test harness.
    // 14) It is also worth noticing how unpleasant it is to test System.Data.SqlClient, please consider not using it.

    public class OrderDataContext : IOrderDataContext
    {
        //todo: extract this to config file.
        const string ConnectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password = myPassword;";

        private static IOrderQueries _oq;
        public static IOrderQueries OrderQueries
        {
            get => _oq ?? (_oq = new OrderQueries());
            set => _oq = value;
        }

        private IOrderCommands _oc;
        public IOrderCommands OrderCommands
        {
            get => _oc ?? (_oc = new OrderCommands());
            set => _oc = value;
        }

        private static ISqlConnectionFactory _sqlCF;
        public static ISqlConnectionFactory SqlConnectionFactory
        {
            get => _sqlCF ?? (_sqlCF = new SqlConnectionFactory());
            set => _sqlCF = value;
        }

        public void CreateOrder(Order order)
        {
            using (var conn = SqlConnectionFactory.GetSqlConnection(ConnectionString))
            {
                using (var trans = conn.BeginTransaction())
                {
                    OrderCommands.AddOrder(order, conn);
                    OrderCommands.AddOrderItems(order, conn);

                    trans.Commit();
                }
            }
        }

        public static Order LoadOrder(int id)
        {
            using (var conn = SqlConnectionFactory.GetSqlConnection(ConnectionString))
            {
                var order = OrderQueries.GetOrderById(conn, id);
                var orderItems = OrderQueries.GetOrderItemsByOrderId(conn, id);
                order.Items.AddRange(orderItems);
                return order;
            }
        }

        public Order GetOrderById(int id)
        {
            return LoadOrder(id);
        }

        public IEnumerable<Order> GetOrdersByCustomerId(int customerId)
        {
            using (var conn = SqlConnectionFactory.GetSqlConnection(ConnectionString))
            {
                var order = OrderQueries.GetOrdersByCustomerId(conn, customerId);
                return order;
            }
        }
    }
}
