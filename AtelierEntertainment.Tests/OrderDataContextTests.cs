using System.Collections.Generic;
using System.Data;
using AtelierEntertainment.DataAccess;
using AtelierEntertainment.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AtelierEntertainment.Tests
{
    [TestClass]
    public class OrderDataContextTests
    {
        [TestMethod]
        public void CreateOrder_TwoInsertMethods_InsertMethodAreBeingExecuted()
        {
            var orderDataContext = new OrderDataContext();
            var orderCommands = new Mock<IOrderCommands>();
            var sqlConnectionFactory = new Mock<ISqlConnectionFactory>();
            var sqlConnection = new Mock<IDbConnection>();
            var dbTransaction = new Mock<IDbTransaction>();
            sqlConnection.Setup(e => e.BeginTransaction()).Returns(dbTransaction.Object);
            sqlConnectionFactory.Setup(e => e.GetSqlConnection(It.IsAny<string>())).Returns(sqlConnection.Object);
            OrderDataContext.SqlConnectionFactory = sqlConnectionFactory.Object;
            orderDataContext.OrderCommands = orderCommands.Object;
            var order = new Order();


            orderDataContext.CreateOrder(order);


            orderCommands.Verify(e => e.AddOrder(It.IsAny<Order>(), It.IsAny<IDbConnection>()), Times.Exactly(1));
            orderCommands.Verify(e => e.AddOrderItems(It.IsAny<Order>(), It.IsAny<IDbConnection>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GetOrdersByCustomerId_CustomerIdProvided_OrderQueryIsBeingExecuted()
        {

            var orderDataContext = new OrderDataContext();
            var orderQueries = new Mock<IOrderQueries>();
            var sqlConnectionFactory = new Mock<ISqlConnectionFactory>();
            var sqlConnection = new Mock<IDbConnection>();
            var dbTransaction = new Mock<IDbTransaction>();
            sqlConnection.Setup(e => e.BeginTransaction()).Returns(dbTransaction.Object);
            sqlConnectionFactory.Setup(e => e.GetSqlConnection(It.IsAny<string>())).Returns(sqlConnection.Object);
            OrderDataContext.SqlConnectionFactory = sqlConnectionFactory.Object;
            OrderDataContext.OrderQueries = orderQueries.Object;
            var anyId = 1;

            orderDataContext.GetOrdersByCustomerId(anyId);


            orderQueries.Verify(e => e.GetOrdersByCustomerId(It.IsAny<IDbConnection>(), anyId), Times.Exactly(1));
            
        }

        [TestMethod]
        public void LoadOrder_OrderIdProvided_GetOrdersAndOrderItemsAreBeingCalled()
        {
            var anyId = 1;
            var order = new Order{Items = new List<orderItem>()};
            var orderQueries = new Mock<IOrderQueries>();
            var sqlConnectionFactory = new Mock<ISqlConnectionFactory>();
            var sqlConnection = new Mock<IDbConnection>();
            var dbTransaction = new Mock<IDbTransaction>();
            sqlConnection.Setup(e => e.BeginTransaction()).Returns(dbTransaction.Object);
            sqlConnectionFactory.Setup(e => e.GetSqlConnection(It.IsAny<string>())).Returns(sqlConnection.Object);
            orderQueries.Setup(e => e.GetOrderById(It.IsAny<IDbConnection>(), anyId)).Returns(order);
            OrderDataContext.SqlConnectionFactory = sqlConnectionFactory.Object;
            OrderDataContext.OrderQueries = orderQueries.Object;
            

            OrderDataContext.LoadOrder(anyId);


            orderQueries.Verify(e => e.GetOrderById(It.IsAny<IDbConnection>(), anyId), Times.Exactly(1));
            orderQueries.Verify(e => e.GetOrderItemsByOrderId(It.IsAny<IDbConnection>(), anyId), Times.Exactly(1));

        }
    }
}
