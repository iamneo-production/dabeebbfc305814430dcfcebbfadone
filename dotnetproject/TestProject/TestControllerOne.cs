using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using dotnetmicroserviceone.Controllers;
using dotnetmicroserviceone.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
namespace dotnetmicroserviceone.Tests
{
    [TestFixture]
    public class ProductControllerTests
    {
        private ProductController _ProductController;
        private ProductDbContext _context;

        [SetUp]
        public void Setup()
        {
            // Initialize an in-memory database for testing
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ProductDbContext(options);
            _context.Database.EnsureCreated(); // Create the database

            // Seed the database with sample data
            _context.Products.AddRange(new List<Product>
            {
                new Product { ProductID = 1, ProductName="One Product", Description="One Description",Price=250,Availability="In Stock" },
                new Product { ProductID = 2, ProductName="Two Product", Description="Two Description",Price=150,Availability="out Of Stok" },
                new Product { ProductID = 3, ProductName="Three Product", Description="Three Description",Price=650,Availability="In Stock" },
            });
            _context.SaveChanges();

            _ProductController = new ProductController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted(); // Delete the in-memory database after each test
            _context.Dispose();
        }
        [Test]
        public void ProductClassExists()
        {
            // Arrange
            Type ProductType = typeof(Product);

            // Act & Assert
            Assert.IsNotNull(ProductType, "Product class not found.");
        }
        [Test]
        public void Product_Properties_ProductName_ReturnExpectedDataTypes()
        {
            // Arrange
            Product product = new Product();
            PropertyInfo propertyInfo = product.GetType().GetProperty("ProductName");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "ProductName property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "ProductName property type is not string.");
        }
[Test]
        public void Product_Properties_Description_ReturnExpectedDataTypes()
        {
            // Arrange
            Product product = new Product();
            PropertyInfo propertyInfo = product.GetType().GetProperty("Description");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "Description property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "Description property type is not string.");
        }
        [Test]
        public void Product_Properties_Availability_ReturnExpectedDataTypes()
        {
            // Arrange
            Product product = new Product();
            PropertyInfo propertyInfo = product.GetType().GetProperty("Availability");
            // Act & Assert
            Assert.IsNotNull(propertyInfo, "Availability property not found.");
            Assert.AreEqual(typeof(string), propertyInfo.PropertyType, "Availability property type is not string.");
        }

        [Test]
        public async Task GetAllProducts_ReturnsOkResult()
        {
            // Act
            var result = await _ProductController.GetAllProducts();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetAllProducts_ReturnsAllProducts()
        {
            // Act
            var result = await _ProductController.GetAllProducts();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;

            Assert.IsInstanceOf<IEnumerable<Product>>(okResult.Value);
            var products = okResult.Value as IEnumerable<Product>;

            var ProductCount = products.Count();
            Assert.AreEqual(3, ProductCount); // Assuming you have 3 Products in the seeded data
        }

        [Test]
        public async Task GetProductById_ExistingId_ReturnsOkResult()
        {
            // Arrange
            var existingId = 1;

            // Act
            var result = await _ProductController.GetProductById(existingId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetProductById_ExistingId_ReturnsProduct()
        {
            // Arrange
            var existingId = 1;

            // Act
            var result = await _ProductController.GetProductById(existingId);

            // Assert
            Assert.IsNotNull(result);

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;

            var product = okResult.Value as Product;
            Assert.IsNotNull(product);
            Assert.AreEqual(existingId, product.ProductID);
        }

        [Test]
        public async Task GetProductById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingId = 99; // Assuming this ID does not exist in the seeded data

            // Act
            var result = await _ProductController.GetProductById(nonExistingId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task AddProduct_ValidData_ReturnsOkResult()
        {
            // Arrange
            var newProduct = new Product
            {
ProductName="New Product", Description="new Description",Price=250,Availability="In Stock"
            };

            // Act
            var result = await _ProductController.AddProduct(newProduct);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task DeleteProduct_ValidId_ReturnsNoContent()
        {
            // Arrange
              // var controller = new ProductsController(context);

                // Act
                var result = await _ProductController.DeleteProduct(1) as NoContentResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(204, result.StatusCode);
        }

        [Test]
        public async Task DeleteProduct_InvalidId_ReturnsBadRequest()
        {
                   // Act
                var result = await _ProductController.DeleteProduct(0) as BadRequestObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(400, result.StatusCode);
                Assert.AreEqual("Not a valid Product id", result.Value);
        }
    }
}
