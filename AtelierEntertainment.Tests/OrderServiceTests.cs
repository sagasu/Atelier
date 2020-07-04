using System.Collections.Generic;
using AtelierEntertainment.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AtelierEntertainment.Tests
{
    [TestClass]
    public class OrderServiceTests
    {
        // I am using strings, because MSTest doesn't allow different types as parameters. I need float and decimal.
        [DataTestMethod]
        [DataRow("2", "3", "5")]
        [DataRow("2.2", "3.2", "5.4")]
        [DataRow("2.2", "3.2", "5.4")]
        [DataRow("125.609375", "0", "125.609375")]
        public void CreateOrder_PriceIsProvidedNoCountry_CorrectTotalIsCalculated(string pr1, string pr2, string total)
        {
            var orderDataContext = new Mock<IOrderDataContext>();
            var orderService = new OrderService(orderDataContext.Object);
            var order = new Order
            {
                Customer = new Customer(),
                Items = new List<orderItem>
                {
                    new orderItem {Price = float.Parse(pr1)}, new orderItem {Price = float.Parse(pr2)}
                }
            };

            orderService.CreateOrder(order);

            Assert.AreEqual(decimal.Parse(total),order.Total);
        }

        [DataTestMethod]
        [DataRow("2", "3", "5")]
        [DataRow("2.2", "3.2", "5.4")]
        [DataRow("2.2", "3.2", "5.4")]
        [DataRow("125.609375", "0", "125.609375")]
        public void CreateOrder_PriceIsProvidedCountryUK_CorrectTotalIsCalculated(string pr1, string pr2, string total)
        {
            var orderDataContext = new Mock<IOrderDataContext>();
            var orderService = new OrderService(orderDataContext.Object);
            var order = new Order
            {
                Customer = new Customer {Country = Countries.UK},
                Items = new List<orderItem>
                {
                    new orderItem {Price = float.Parse(pr1)}, new orderItem {Price = float.Parse(pr2)}
                }
            };


            orderService.CreateOrder(order);

            Assert.AreEqual(decimal.Parse(total) * Countries.UKSalesTax, order.Total);
        }

        [DataTestMethod]
        [DataRow("2", "3", "5")]
        [DataRow("2.2", "3.2", "5.4")]
        [DataRow("2.2", "3.2", "5.4")]
        [DataRow("125.609375", "0", "125.609375")]
        public void CreateOrder_PriceIsProvidedCountryAU_CorrectTotalIsCalculated(string pr1, string pr2, string total)
        {
            var orderDataContext = new Mock<IOrderDataContext>();
            var orderService = new OrderService(orderDataContext.Object);
            var order = new Order
            {
                Customer = new Customer { Country = Countries.AU },
                Items = new List<orderItem>
                {
                    new orderItem {Price = float.Parse(pr1)}, new orderItem {Price = float.Parse(pr2)}
                }
            };


            orderService.CreateOrder(order);

            Assert.AreEqual(decimal.Parse(total) * Countries.AUSalesTax, order.Total);
        }
    }
}
