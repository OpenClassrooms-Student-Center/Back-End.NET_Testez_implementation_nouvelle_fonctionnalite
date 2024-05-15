using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
        private List<Product> _productTestEntities;
        private ProductService _productService;
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<ICart> _cartMock;
        private Mock<IOrderRepository> _orderRepositoryMock;
        private Mock<IStringLocalizer<ProductService>> _localizerMock;

        public ProductServiceTests()
        {
            // Creation d'une liste de produits fictives=
            _productTestEntities = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10.0, Description = "Test", Details = "Test", Quantity = 10 },
                new Product { Id = 2, Name = "Product 2", Price = 50.0, Description = "Test", Details = "Test", Quantity = 100 },
                new Product { Id = 3, Name = "Product 3", Price = 53.9, Description = "Test", Details = "Test", Quantity = 200 }
            };

            // Création des mock
            _productRepositoryMock = new Mock<IProductRepository>();
            _cartMock = new Mock<ICart>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _localizerMock = new Mock<IStringLocalizer<ProductService>>();

            // Configuration des mock
            _productRepositoryMock.Setup(x => x.GetAllProducts()).Returns(_productTestEntities);

            // Création de l'instance de ProductService
            _productService = new ProductService(_cartMock.Object, _productRepositoryMock.Object, _orderRepositoryMock.Object, _localizerMock.Object);
        }

        [Fact]
        public void GetAllProductsViewModelTest_PourUneListeDeProduits_RetourneToutLesProduits()
        {
            // Arrange
            // Les éléments necessaires sont construits une seule fois
            // dans le constructeur de la classe
                                 
            // Act
            // Appel de la méthode avec les parametres du test.
            var result = _productService.GetAllProductsViewModel();

            // Assert
            // On verifie que la méthode renvoie la meme valeur quel l'entité
            Assert.Equal(_productTestEntities.Count, result.Count);
            for (int i = 0; i < _productTestEntities.Count; i++)
            {
                Assert.Equal(_productTestEntities[i].Id, result[i].Id);
                Assert.Equal(_productTestEntities[i].Name, result[i].Name);
                Assert.Equal(_productTestEntities[i].Price.ToString(CultureInfo.InvariantCulture), result[i].Price);
                Assert.Equal(_productTestEntities[i].Description, result[i].Description);
                Assert.Equal(_productTestEntities[i].Details, result[i].Details);
                Assert.Equal(_productTestEntities[i].Quantity.ToString(), result[i].Stock);
            }
        }

        [Fact]
        public void MapToViewModelTest_ParametreEntre_SortieAttendue() { }

        [Fact]
        public void GetAllProductsTest_PourUneListeDeProduits_RetourneToutLesProduits() 
        {
            // Arrange
            // Les éléments necessaires sont construits une seule fois
            // dans le constructeur de la classe

            // Act
            // Appel de la méthode avec les parametres du test.
            var result = _productService.GetAllProducts();

            // Assert
            // Verification des résultats de la méthode
            Assert.Equal(_productTestEntities.Count, result.Count);

        }

        [Fact]
        public void GetProductByIdViewModelTest_ParametreEntre_SortieAttendue() { }

        [Fact]
        public void GetProductByIdTest_PourUnIdDeProduit_RetourneLeProduitCorrespondant() 
        {
            // Arrange
            // Creer une liste de produits fictifs, avec ID
            // Créer une instance de ProductService

            // Act
            // Apeller la méthode GetProductById en passant un ID en parametre

            // Assert
            //Assert.Equals(produit avec l'id dans ma liste, resultat de mon appel de méthode)
        }

        [Fact]
        public void GetProductTest_ParametreEntre_SortieAttendue() { }

        [Fact]
        public void GetProductTest_ByList_ParametreEntre_SortieAttendue() { }

        [Fact]
        public void UpdateProductQuantitiesTest_ParametreEntre_SortieAttendue() { }

        [Fact]
        public void SaveProduct_ParametreEntre_SortieAttendue() { }

        [Fact]
        public void MapToProductEntityTest_ParametreEntre_SortieAttendue() { }

        [Fact]
        public void DeleteProductTest_ParametreEntre_SortieAttendue() { }

        // TODO write test methods to ensure a correct coverage of all possibilities
    }
}