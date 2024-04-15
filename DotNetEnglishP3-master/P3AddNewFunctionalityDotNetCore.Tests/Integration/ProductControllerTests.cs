using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.ComponentModel;
using System.IO;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.Integration
{
    public class ProductControllerTests
    {
        private readonly IConfiguration _configuration;

        public ProductControllerTests()
        {
            // Build configuration
            var basePath = Path.Combine(Directory.GetCurrentDirectory());

            _configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
        }

        [Fact]
        [Description("It should write the new product to the database")]
        public void AddProduct()
        {
            //Arrange
            var cart = new Cart();

            // Retreive connection string and connexions options
            var connectionString = _configuration.GetConnectionString("P3Referential");
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseSqlServer(connectionString)
                .Options;

            // Instenciate and enter a DB context
            using var context = new P3Referential(options, _configuration);

            // Instanciate a Product Service and Controller
            var productService = new ProductService(cart, new ProductRepository(context), new OrderRepository(context), null);
            var productController = new ProductController(productService, null);

            //Act

            // Create a product via the controller
            var createActionResult = productController.Create(new ProductViewModel
            {
                Name = "Test",
                Stock = "1",
                Price = "1.5"
            });

            //Assert

            Assert.NotNull(createActionResult);
            // Assert a redirection to the admin page
            Assert.IsType<Microsoft.AspNetCore.Mvc.RedirectToActionResult>(createActionResult);

            // Assert that the product was created...
            var products = productService.GetAllProducts();
            Assert.Contains(products, p => p.Name == "Test");

            // ...with the right properties
            var product = productService.GetProductById(products.Find(p => p.Name == "Test").Id);
            Assert.Equal("Test", product.Name);
            Assert.Equal(1, product.Quantity);
            Assert.Equal(1.5, product.Price);

            // Teardown
            productService.DeleteProduct(product.Id);
        }

        [Fact]
        [Description("It should update the product's stock in the database")]
        public void UpdateProductStock()
        {
            //Arrange
            var cart = new Cart();

            // Retreive connection string and connexions options
            var connectionString = _configuration.GetConnectionString("P3Referential");
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseSqlServer(connectionString)
                .Options;

            // Instenciate and enter a DB context
            using var context = new P3Referential(options, _configuration);

            // Instanciate a Product Service and Controller
            var productService = new ProductService(cart, new ProductRepository(context), new OrderRepository(context), null);
            var productController = new ProductController(productService, null);

            // Create a test product
            var createActionResult = productController.Create(new ProductViewModel
            {
                Name = "Test",
                Stock = "1",
                Price = "1.5"
            });

            // Ensure the product was created with the right quantity
            var products = productService.GetAllProducts();
            var product = productService.GetProductById(products.Find(p => p.Name == "Test").Id);
            Assert.Equal(1, product.Quantity);

            //Act
            cart.AddItem(product, 1);
            productService.UpdateProductQuantities();

            //Assert
            product = productService.GetProductById(products.Find(p => p.Name == "Test").Id);
            Assert.Equal(0, product.Quantity);

            //Teardown
            productService.DeleteProduct(product.Id);
        }

        [Fact]
        [Description("It should remove the product from the database")]
        public void DeleteProduct()
        {
            //Arrange
            var cart = new Cart();

            // Retreive connection string and connexions options
            var connectionString = _configuration.GetConnectionString("P3Referential");
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseSqlServer(connectionString)
                .Options;

            // Instenciate and enter a DB context
            using var context = new P3Referential(options, _configuration);

            // Instanciate a Product Service and Controller
            var productService = new ProductService(cart, new ProductRepository(context), new OrderRepository(context), null);
            var productController = new ProductController(productService, null);

            // Create a test product
            var createActionResult = productController.Create(new ProductViewModel
            {
                Name = "Test",
                Stock = "1",
                Price = "1.5"
            });

            // Ensure the product was created
            var products = productService.GetAllProducts();
            var product = productService.GetProductById(products.Find(p => p.Name == "Test").Id);
            Assert.NotNull(product);

            // Act
            productController.DeleteProduct(product.Id);

            try
            {
                // Assert
                Assert.Null(productService.GetProductById(product.Id));
            }
            catch
            {
                // Teardown
                productService.DeleteProduct(product.Id);
            }
        }
    }
}