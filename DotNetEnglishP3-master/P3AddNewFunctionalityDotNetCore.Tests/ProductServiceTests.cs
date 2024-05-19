using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
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
            for (int i = 0; i < _productTestEntities.Count; i++)
            {
                Assert.Equal(_productTestEntities[i].Id, result[i].Id);
                Assert.Equal(_productTestEntities[i].Name, result[i].Name);
                Assert.Equal(_productTestEntities[i].Price, result[i].Price);
                Assert.Equal(_productTestEntities[i].Description, result[i].Description);
                Assert.Equal(_productTestEntities[i].Details, result[i].Details);
                Assert.Equal(_productTestEntities[i].Quantity, result[i].Quantity);
            }

        }

        [Fact]
        public void GetProductByIdViewModelTest_ParametreEntre_SortieAttendue()
        {
            // Arrange
            // Les éléments necessaires sont construits une seule fois
            // dans le constructeur de la classe

            // Act & Assert
            // Apeller la méthode GetProductByIdViewModel en passant un ID en parametre
            var result = _productService.GetProductByIdViewModel(1);
            Assert.NotNull(result);
            var result2 = _productService.GetProductByIdViewModel(2);
            Assert.NotNull(result2);
            var result3 = _productService.GetProductByIdViewModel(3);
            Assert.NotNull(result3);
            var falseResult = _productService.GetProductByIdViewModel(12);
            Assert.Null(falseResult);
        }

        [Fact]
        public void GetProductByIdTest_PourUnIdDeProduit_RetourneLeProduitCorrespondant() 
        {
            // Arrange
            // Les éléments necessaires sont construits une seule fois
            // dans le constructeur de la classe

            // Act & Assert
            // Apeller la méthode GetProductById en passant un ID en parametre
            var result = _productService.GetProductById(1);
            Assert.NotNull(result);
            var result2 = _productService.GetProductById(2);
            Assert.NotNull(result2);
            var result3 = _productService.GetProductById(3);
            Assert.NotNull(result3);
            var falseResult = _productService.GetProductById(12);
            Assert.Null(falseResult);
        }

        [Fact]
        public async Task GetProductTest_QuandLeProduitExiste_RetourneLeProduitAttendu() 
        {
            // Arrange
            // Initialisation du premier produit et du mock (en Asynchrone)
            var produitAttendu = _productTestEntities.First();
            _productRepositoryMock.Setup(x => x.GetProduct(produitAttendu.Id)).ReturnsAsync(produitAttendu);

            // Act
            // Appel de la méthode en Assynchrone
            var result = await _productService.GetProduct(produitAttendu.Id);

            // Assert
            Assert.Equal(produitAttendu, result);
        }

        [Fact]
        public async Task GetProductTest_ByList_SiLesProduitsExistent_RetourneLaListeDesProduits()
        {
            // Arrange
            // Configuration du Mock pour un resultat Asynchrone
            _productRepositoryMock.Setup(x => x.GetProduct()).ReturnsAsync(_productTestEntities);

            // Act
            // Appel de la méthode en Assynchrone
            var result = await _productService.GetProduct();

            // Assert
            Assert.IsType<List<Product>>(result);
            Assert.Equal(_productTestEntities, result);
        }


        [Fact]
        public void UpdateProductQuantitiesTest_PourUnPanierAvecDesProduits_MettreAJourLeStockDesProduits()
        {
            // Arrange
            
            // Act

            // Assert
        }


        [Fact]
        public void SaveProduct_ParametreEntre_SortieAttendue() 
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public void MapToProductEntityTest_ParametreEntre_SortieAttendue() { }

        [Fact]
        public void DeleteProductTest_ParametreEntre_SortieAttendue() { }

        // TODO write test methods to ensure a correct coverage of all possibilities
    }
}