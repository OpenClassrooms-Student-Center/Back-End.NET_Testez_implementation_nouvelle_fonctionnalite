using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models;
using Xunit;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;

namespace P3AddNewFunctionalityDotNetCore.Tests.Unit
{
    public class ProductServiceTests
    {
        public class CheckProductModelErrors
        {
            private readonly Mock<ICart> mockCart;
            private readonly Mock<IProductRepository> mockProductRepository;
            private readonly Mock<IOrderRepository> mockOrderRepository;
            private readonly Mock<IStringLocalizer<ProductService>> mockLocalizer;

            public CheckProductModelErrors()
            {
                // Setup
                mockCart = new Mock<ICart>();
                mockProductRepository = new Mock<IProductRepository>();
                mockOrderRepository = new Mock<IOrderRepository>();
                // Map localized error messages to the localizer mock
                mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
                foreach (var property in typeof(Resources.ProductService).GetProperties())
                {
                    if (property.Name == "ResourceManager" || property.Name == "Culture" || property.Name == "ResourceCulture")
                        continue;
                    mockLocalizer.Setup(l => l[property.Name]).Returns(new LocalizedString(property.Name, property.GetValue(null).ToString()));
                }
            }

            [Fact]
            public void ShouldSucceed_WhenProductIsWellFormed()
            {
                // Arrange
                var service = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);

                var product = new ProductViewModel()
                {
                    Name = "Test",
                    Stock = "1",
                    Price = "1.5"
                };

                // Act
                List<string> modelErrors = service.CheckProductModelErrors(product);

                // Assert
                Assert.Empty(modelErrors);
            }

            [Fact]
            public void ShouldAddModelErrors_WhenProductIsEmpty()
            {
                // Arrange
                var service = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);

                var product = new ProductViewModel();

                // Act
                List<string> modelErrors = service.CheckProductModelErrors(product);

                // Assert
                Assert.Contains(Resources.ProductService.MissingName, modelErrors);
                Assert.Contains(Resources.ProductService.MissingPrice, modelErrors);
                Assert.Contains(Resources.ProductService.MissingStock, modelErrors);
            }

            [Fact]
            public void ShouldAddModelErrors_WhenPriceIsNotANumber()
            {
                // Arrange
                var service = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);

                var product = new ProductViewModel()
                {
                    Name = "Test",
                    Stock = "1",
                    Price = "NotADouble"
                };

                // Act
                List<string> modelErrors = service.CheckProductModelErrors(product);

                // Assert
                Assert.Contains(Resources.ProductService.PriceNotANumber, modelErrors);
            }

            [Fact]
            public void ShouldAddModelErrors_WhenPriceIsNotGreaterThanZero()
            {
                // Arrange
                var service = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);

                var product = new ProductViewModel()
                {
                    Name = "Test",
                    Stock = "1",
                    Price = "0"
                };

                // Act
                List<string> modelErrors = service.CheckProductModelErrors(product);

                // Assert
                Assert.Contains(Resources.ProductService.PriceNotGreaterThanZero, modelErrors);
            }

            [Fact]
            public void ShouldAddModelErrors_WhenStockIsNotAnInteger()
            {
                // Arrange
                var service = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);

                var product = new ProductViewModel()
                {
                    Name = "Test",
                    Stock = "NotAnInteger",
                    Price = "1"
                };

                // Act
                List<string> modelErrors = service.CheckProductModelErrors(product);

                // Assert
                Assert.Contains(Resources.ProductService.StockNotAnInteger, modelErrors);
            }

            [Fact]
            public void ShouldAddModelErrors_WhenStockIsNotGreaterThanZero()
            {
                // Arrange
                var service = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object, mockLocalizer.Object);

                var product = new ProductViewModel()
                {
                    Name = "Test",
                    Stock = "0",
                    Price = "1"
                };

                // Act
                List<string> modelErrors = service.CheckProductModelErrors(product);

                // Assert
                Assert.Contains(Resources.ProductService.StockNotGreaterThanZero, modelErrors);
            }
        }
    }
}