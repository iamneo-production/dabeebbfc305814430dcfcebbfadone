using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using dotnetmicroservicetwo.Controllers;
using dotnetmicroservicetwo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
namespace dotnetmicroservicetwo.Tests
{
    [TestFixture]
    public class OrderControllerTests
    {
        private OrderController _OrderController;
        private OrderDbContext _context;

        [SetUp]
        public void Setup()
        {
            // Initialize an in-memory database for testing
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new OrderDbContext(options);
            _context.Database.EnsureCreated(); // Create the database

            // Seed the database with sample data
            _context.Orders.AddRange(new List<Order>
            {
                new Order { OrderID = 1,  CustomerID="CT101",ProductID="PD101",Quantity=5,TotalAmount=2590,OrderStatus="Delivered" },
                new Order { OrderID = 2,  CustomerID="CT102",ProductID="PD102",Quantity=7,TotalAmount=7690,OrderStatus="In Transit" },
                new Order { OrderID = 3,  CustomerID="CT103",ProductID="PD103",Quantity=10,TotalAmount=2090,OrderStatus="Cancelled" }
            });
            _context.SaveChanges();

            _OrderController = new OrderController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted(); // Delete the in-memory database after each test
            _context.Dispose();
        }
        [Test]
        public void OrderClassExists()
        {
            // Arrange
            Type OrderType = typeof(Order);

            // Act & Assert
            Assert.IsNotNull(OrderType, "Order class not found.");
        }
        [Test]
        public void Order_Properties_CustomerID_ReturnExpectedDataTypes()
        {
            // Arrange
            Order order = new Order();
            PropertyInfo propertyInfo = order.GetType().GetProperty("CustomerID");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "CustomerID property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "CustomerID property type is not string.");
        }
[Test]
        public void Order_Properties_OrderStatus_ReturnExpectedDataTypes()
        {
            // Arrange
            Order order = new Order();
            PropertyInfo propertyInfo = order.GetType().GetProperty("OrderStatus");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "OrderStatus property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "OrderStatus property type is not string.");
        }

        [Test]
        public async Task GetAllOrders_ReturnsOkResult()
        {
            // Act
            var result = await _OrderController.GetAllOrders();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetAllOrders_ReturnsAllOrders()
        {
            // Act
            var result = await _OrderController.GetAllOrders();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;

            Assert.IsInstanceOf<IEnumerable<Order>>(okResult.Value);
            var orders = okResult.Value as IEnumerable<Order>;

            var OrderCount = orders.Count();
            Assert.AreEqual(3, OrderCount); // Assuming you have 3 Orders in the seeded data
        }


        [Test]
        public async Task AddOrder_ValidData_ReturnsOkResult()
        {
            // Arrange
            var newOrder = new Order
            {
 CustomerID="CT104",ProductID="PD104",Quantity=12,TotalAmount=2590,OrderStatus="Delivered"
            };

            // Act
            var result = await _OrderController.AddOrder(newOrder);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task DeleteOrder_ValidId_ReturnsNoContent()
        {
            // Arrange
              // var controller = new OrdersController(context);

                // Act
                var result = await _OrderController.DeleteOrder(1) as NoContentResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(204, result.StatusCode);
        }

        [Test]
        public async Task DeleteOrder_InvalidId_ReturnsBadRequest()
        {
                   // Act
                var result = await _OrderController.DeleteOrder(0) as BadRequestObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(400, result.StatusCode);
                Assert.AreEqual("Not a valid Order id", result.Value);
        }
    }
}
