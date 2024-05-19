using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public void Product_DoitEtreNomme_RetourneMessageErreurAssocie()
        {
            // Arrange
            var pVMTest = new ProductViewModel
            {
                Name = null,
                Price = "123",
                Stock = "123"
            };

            // On utilise la classe ValidationContext pour récupérer
            // les résultats d'une tentative de validation
            var validationContext = new ValidationContext(pVMTest);

            // On précise la validation par rapport à l'attribut "Name"
            validationContext.MemberName = "Name";

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

        // TODO write test methods to ensure a correct coverage of all possibilities
    }
}