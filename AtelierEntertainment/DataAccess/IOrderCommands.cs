using System.Data;

namespace AtelierEntertainment.DataAccess
{
    public interface IOrderCommands
    {
        void AddOrderItems(Order order, IDbConnection conn);
        void AddOrder(Order order, IDbConnection conn);
    }
}