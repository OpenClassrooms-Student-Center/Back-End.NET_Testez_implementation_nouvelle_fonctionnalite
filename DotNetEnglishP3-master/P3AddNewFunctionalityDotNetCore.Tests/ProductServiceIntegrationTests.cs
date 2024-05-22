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
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceIntegrationTests
    {
        private readonly DbContextOptions<AppIdentityDbContext> _options;
        private readonly AppIdentityDbContext _context;
        private readonly ProductService _productService;

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
            _context = new P3Referential(_options);

            // Creation des éléments d'instance de ProductService pour les tests
            // Nouveau panier
            var mockCart = Mock<ICart>();

            // Nouveau repository de Product basé sur le contexte de connexion à la bdd 
            var productRepository = new ProductRepository(_context);

            // Nouveau repository de Order basé sur le contexte de connexion à la bdd
            var orderRepository = new OrderRepository(_context);

            // Nouveau Localizer
            var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();

            // Nouvelle instance de productService
            _productService = new ProductService(cart, productRepository, orderRepository, mockLocalizer);
        }

        [Fact]
        public void Test_Ajouter_Produit_BDD()
        {
            // Arrange
            // Création d'un produit à ajouter à la BDD

            // Act
            // Ajout du produit à la BDD via la méthode associée

            // Assert
            // Vérification que le produit a bien été ajouté à la BDD
        }

        [Fact]
        public void Test_Supprimer_Produit_BDD()
        {
            // Arrange
            // Création d'un produit à ajouter à la BDD
            // Ajout du produit à la BDD via la méthode associée

            // Act
            // Suppression du produit à la BDD via la méthode associée

            // Assert
            // Vérification que le produit a bien été supprimé à la BDD
        }

        [Fact]
        public void Test_MettreAJour_Quantite_Produit()
        {
            // Arrange
            // Création d'un produit à ajouter à la BDD
            // Ajout du produit à la BDD via la méthode associée

            // Act
            // Mise à jour de la quantité du produit via la méthode associée

            // Assert
            // Vérification que la quantité du produit a bien été mise à jour en BDD
        }

        [Fact]
        public void Test_Recuperer_Infos_Produit()
        {
            // Arrange
            // Création d'un produit à ajouter à la BDD
            // Ajout du produit à la BDD via la méthode associée

            // Act
            // Récupération des infos du produit via la méthode associée

            // Assert
            // Vérification que les infos du produit ont bien été récupérées
        }
    }
}
