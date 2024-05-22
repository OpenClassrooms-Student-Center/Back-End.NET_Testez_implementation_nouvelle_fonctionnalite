using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceIntegrationTests
    {
        private readonly DbContextOptions<AppIdentityDbContext> _options;
        private readonly AppIdentityDbContext _context;
    
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
            var optionsBuilder = new DbContextOptionsBuilder<AppIdentityDbContext>();

            // Configuration de la connexion à la bdd
            optionsBuilder.UseSqlServer(connectionString);

            // On enregistre les options pour pouvoir les utiliser plus tard dans la classe
            _options = optionsBuilder.Options;

            // On configur le _context avec les options données
            _context = new AppIdentityDbContext(optionsBuilder.Options, configuration);
        }
    }
}
