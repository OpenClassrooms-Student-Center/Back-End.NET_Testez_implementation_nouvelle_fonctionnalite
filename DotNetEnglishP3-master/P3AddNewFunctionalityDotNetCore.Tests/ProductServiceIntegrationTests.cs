using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceIntegrationTests
    {
        private readonly DbContextOptions<P3Referential> _options;
        private readonly P3Referential _context;
        private readonly ProductService _productService;
        private Cart _cart;

        public ProductServiceIntegrationTests()
        {
            // Création des éléments de connexion à la base de donnée pour gérer les tests d'intégrations
            // Recupération des infos de la bdd via le Configuration Builder et les éléments présents dans appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("P3Referential");

            // Nouvelle instance du DbContextOptionsBuilder
            var optionsBuilder = new DbContextOptionsBuilder<P3Referential>();

            // Configuration de la connexion à la bdd
            optionsBuilder.UseSqlServer(connectionString);

            // On enregistre les options pour pouvoir les utiliser plus tard dans la classe
            _options = optionsBuilder.Options;

            // On configure le _context avec les options données
            _context = new P3Referential(_options, configuration);

            // Creation des éléments d'instance de ProductService pour les tests
            // Nouveau panier
            _cart = new Cart();

            // Nouveau repository de Product basé sur le contexte de connexion à la bdd
            var productRepository = new ProductRepository(_context);

            // Nouveau repository de Order basé sur le contexte de connexion à la bdd
            var orderRepository = new OrderRepository(_context);

            // Nouveau Localizer
            var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();

            // Nouvelle instance de productService
            _productService = new ProductService(_cart, productRepository, orderRepository, mockLocalizer.Object);
        }

        [Fact]
        public void Test_Ajouter_Produit_BDD()
        {
            // Arrange
            // Création d'un produit à ajouter à la BDD
            var productViewModelTest = new ProductViewModel
            {
                Name = "Test product",
                Price = "19.99",
                Stock = "10"
            };

            // Act
            // Ajout du produit à la BDD via la méthode associée
            _productService.SaveProduct(productViewModelTest);


            // Assert
            // Vérification que le produit a bien été ajouté à la BDD
            var productVerify = _context.Product.FirstOrDefault(p => p.Name == "Test product");
            Assert.NotNull(productVerify);
            Assert.Equal(19.99, productVerify.Price);
            Assert.Equal(10, productVerify.Quantity);
            _productService.DeleteProduct(productVerify.Id);
        }

        [Fact]
        public void Test_Supprimer_Produit_BDD()
        {
            // Arrange
            // Création d'un produit à ajouter à la BDD qui sera supprimé
            var productViewModelDeleteTest = new ProductViewModel
            {
                Name = "Test product to delete",
                Price = "69.99",
                Stock = "4"
            };
            // Ajout du produit à la BDD via la méthode associée
            _productService.SaveProduct(productViewModelDeleteTest);

            var addedProduct = _context.Product.FirstOrDefault(p => p.Name == productViewModelDeleteTest.Name);
            Assert.NotNull(addedProduct);

            // Act
            // Suppression du produit à la BDD via la méthode associée
            _productService.DeleteProduct(addedProduct.Id);

            // Assert
            // Vérification que le produit a bien été supprimé à la BDD
            var deletedProduct = _context.Product.FirstOrDefault(p => p.Id == addedProduct.Id);
            Assert.Null(deletedProduct);
        }

        [Fact]
        public void Test_UpdateProductQuantities()
        {
            // Arrange
            // Création d'un produit à ajouter à la BDD
            var productViewModelTest = new ProductViewModel
            {
                Name = "Test product for updating quantities",
                Price = "10.00",
                Stock = "5"
            };
            _productService.SaveProduct(productViewModelTest);

            var addedProduct = _context.Product.FirstOrDefault(p => p.Name == productViewModelTest.Name);
            Assert.NotNull(addedProduct);

            // Création d'un panier avec une ligne de commande contenant ce produit
            _cart.AddItem(new Product { Id = addedProduct.Id, Name = addedProduct.Name }, 3);

            // Act
            // Appel de la méthode pour mettre à jour les quantités des produits en fonction des lignes de commande du panier
            _productService.UpdateProductQuantities();

            // Assert
            // Vérification que les quantités des produits dans la base de données ont été correctement mises à jour
            var updatedProduct = _context.Product.FirstOrDefault(p => p.Name == "Test product for updating quantities");
            Assert.NotNull(updatedProduct);
            Assert.Equal(2, updatedProduct.Quantity); // 5 - 3 = 2
            _productService.DeleteProduct(updatedProduct.Id); 
        }


        [Fact]
        public void Test_Recuperer_Infos_Produit()
        {
            // Arrange
            // Création d'un produit à ajouter à la BDD pour récupérer ses infos
            var productViewModelSelectTest = new ProductViewModel
            {
                Name = "Test product infos",
                Price = "545.45",
                Stock = "130"
            };
            // Ajout du produit à la BDD via la méthode associée
            _productService.SaveProduct(productViewModelSelectTest);

            var addedProduct = _context.Product.FirstOrDefault(p => p.Name == productViewModelSelectTest.Name);
            Assert.NotNull(addedProduct);

            // Act
            // Récupération des infos du produit via la méthode associée
            var productInfo = _productService.GetProductById(addedProduct.Id);


            // Assert
            // Vérification que les infos du produit ont bien été récupérées
            Assert.NotNull(productInfo);
            Assert.Equal("Test product infos", productInfo.Name);
            Assert.Equal(545.45, productInfo.Price);
            Assert.Equal(130, productInfo.Quantity);
            _productService.DeleteProduct(productInfo.Id);
        }
    }
}