using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public void ProductViewModel_DoitEtreNomme_RetourneMessageErreurAssocie()
        {
            // Arrange
            var pVMTest = new ProductViewModel
            {
                Name = null,
                Price = "12.3",
                Stock = "123"
            };

            // On utilise la classe ValidationContext pour récupérer
            // les résultats d'une tentative de validation
            var validationContext = new ValidationContext(pVMTest);

            // Les résultats sont enregistré dans une liste
            var validationResults = new List<ValidationResult>();

            // Act
            // On test la validation via la méthode "TryValidatorObject" qui prends
            // les trois éléments en parametre (l'instance, le contexte, et les resultats)
            bool isValid = Validator.TryValidateObject(pVMTest, validationContext, validationResults);

            // Assert
            Assert.False(isValid); // On vérifie que la validation echoue bien
            Assert.Single(validationResults); // Il ne devrait y avoir qu'un seul message d'erreur
            Assert.Equal("MissingName", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void ProductViewModel_DoitAvoirUnPrix_RetourneMessageErreurAssocie()
        {
            // Arrange
            var pVMTest = new ProductViewModel { Name = "test", Price = null, Stock = "123" };
            var validationContext = new ValidationContext(pVMTest);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(pVMTest, validationContext, validationResults);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Equal("MissingPrice", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void ProductViewModel_PrixDoitEtreUnNombre_RetourneMessageErreurAssocie()
        {
            // Arrange
            var pVMTest = new ProductViewModel { Name = "test", Price = "abc", Stock = "123" };
            var validationContext = new ValidationContext(pVMTest);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(pVMTest, validationContext, validationResults);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Equal("PriceNotANumber", validationResults[0].ErrorMessage);
        }



        [Fact]
        public void ProductViewModel_PrixDoitEtreSuperieurAZero_RetourneMessageErreurAssocie()
        {
            // Arrange
            var pVMTest = new ProductViewModel { Name = "test", Price = "0", Stock = "123" };

            var validationContext = new ValidationContext(pVMTest);

            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(pVMTest, validationContext, validationResults);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Equal("PriceNotGreaterThanZero", validationResults[0].ErrorMessage);

        }

        [Fact]
        public void ProductViewModel_DoitAvoirUneQuantite_RetourneMessageErreurAssocie()
        {
            // Arrange
            var pVMTest = new ProductViewModel { Name = "test", Price = "12.3", Stock = null };

            var validationContext = new ValidationContext(pVMTest);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(pVMTest, validationContext, validationResults);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Equal("MissingStock", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void ProductViewModel_StockDoitEtreUnEntier_RetourneMessageErreurAssocie()
        {
            // Arrange
            var pVMTest = new ProductViewModel { Name = "test", Price = "12.3", Stock = "12.3" };

            var validationContext = new ValidationContext(pVMTest);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(pVMTest, validationContext, validationResults);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Equal("StockNotAnInteger", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void ProductViewModel_StockDoitEtreSuperieurAZero_RetourneMessageErreurAssocie()
        {
            // Arrange
            var pVMTest = new ProductViewModel { Name = "test", Price = "12.3", Stock = "0" };

            var validationContext = new ValidationContext(pVMTest);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(pVMTest, validationContext, validationResults);

            // Assert
            Assert.False(isValid);
            Assert.Single(validationResults);
            Assert.Equal("StockNotGreaterThanZero", validationResults[0].ErrorMessage);
        }
    }
}