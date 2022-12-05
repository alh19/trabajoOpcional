using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Sandwich2Go.UIT.Controllers.Ofertas
{
    public class UC_CrearOferta_UIT : IDisposable
    {
        IWebDriver _driver;
        string _URI = "https://localhost:5001/";
        bool _pipeline = false;
        string username = "elena@uclm.com";
        string password = "Password1234%";
        public UC_CrearOferta_UIT()
        {
            var optionsc = new ChromeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };

            if (_pipeline) optionsc.AddArgument("--headless");

            _driver = new ChromeDriver(optionsc);
        }
        private void precondition_perform_login(string username, string password)
        {
            _driver.Navigate().GoToUrl(_URI +
            "Identity/Account/Login");

            _driver.FindElement(By.Id("Input_Email"))
            .SendKeys(username);

            _driver.FindElement(By.Id("Input_Password"))
            .SendKeys(password);

            _driver.FindElement(By.Id("login-submit"))
            .Click();


        }

        private void first_step_accessing_comprarSandwich()
        {
            _driver.FindElement(By.Id("CrearOferta")).Click();
        }

        private void UC_CrearOferta_FA1_filtrarPorNombre(string sandwichFilter)
        {
            _driver.FindElement(By.Id("SandwichName")).Clear();
            _driver.FindElement(By.Id("SandwichName")).SendKeys(sandwichFilter);
            _driver.FindElement(By.Id("filterButton")).Click();
        }

        private void UC_Crear_Oferta_FA1_filtrarPorPrecio(string precioFilter)
        {
            _driver.FindElement(By.Id("sandwichPrecio")).Clear();
            _driver.FindElement(By.Id("sandwichPrecio")).SendKeys(precioFilter);
            _driver.FindElement(By.Id("filterButton")).Click();
        }

        [Fact]
        public void Initial_step_opening_the_web_page()
        {
            //Arrange
            string expectedTitle = "Home Page - Sandwich2Go";
            string expectedText = "Register";
            //Act
            //El navegador cargará la URI indicada
            _driver.Navigate().GoToUrl(_URI);
            //Assert
            //Comprueba que el título coincide con el esperado
            Assert.Equal(expectedTitle, _driver.Title);
            //Comprueba si la página contiene el string indicado
            Assert.Contains(expectedText, _driver.PageSource);
        }

        [Theory]
        [InlineData(true)]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_0_1_FlujoBasico_CrearOferta(string nombreOferta, string fechaInicio, string fechaFinalizacion, string Descripcio)
        {

        }
        public void Dispose()
        {
            _driver.Close();
            _driver.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}

