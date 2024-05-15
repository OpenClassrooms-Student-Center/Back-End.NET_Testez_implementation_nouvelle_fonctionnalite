using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using System.Collections.Generic;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
        /// <summary>
        /// Take this test method as a template to write your test method.
        /// A test method must check if a definite method does its job:
        /// returns an expected value from a particular set of parameters
        /// </summary>
        [Fact]
        public void GetAllProductsViewModelTest_PourUneListeDeProduits_RetourneToutLesProduits()
        {
            // Arrange
            // Creation d'une fausse liste de produits pour le test
            var productTestEntities = new List<Product>
            {
                new Product { Id = 1, Name="Product 1", Price = 10.0, Description = "Test", Details = "Test", Quantity = 10},
                new Product { Id = 2, Name="Product 2", Price = 50.0, Description = "Test", Details = "Test", Quantity = 100},
                new Product { Id = 3, Name="Product 3", Price = 53.9, Description = "Test", Details = "Test", Quantity = 200}
            };

            // Création des bouchons pour les dépendances
            var cartMock = new Mock<ICart>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var localizerMock = new Mock<IStringLocalizer<ProductService>>();

            // Configuration du mock IProductRepository
            productRepositoryMock.Setup(x => x.GetAllProducts()).Returns(productTestEntities);
            
            // Création d'un instance ProductService pour utiliser ses méthodes
            var productServiceInstance = new ProductService(cartMock.Object, productRepositoryMock.Object, orderRepositoryMock.Object, localizerMock.Object);
            
            // Act
            // Appel de la méthode avec les parametres du test.
            var result = productServiceInstance.GetAllProductsViewModel();


            // Assert
            // On verifie que la méthode renvoie la meme valeur quel l'entité
            Assert.Equal(productTestEntities.Count, result.Count);
            // TODO vérifier que les produits sont les memes.
/*            foreach (var product in productTestEntities)
            {
                
            }*/
        }

        [Fact]
        public void MapToViewModelTest_ParametreEntre_SortieAttendue() { }

        [Fact]
        public void GetAllProductsTest_ParametreEntre_SortieAttendue() { }

        [Fact]
        public void GetProductByIdViewModelTest_ParametreEntre_SortieAttendue() { }

        [Fact]
        public void GetProductByIdTest_ParametreEntre_SortieAttendue() { }

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