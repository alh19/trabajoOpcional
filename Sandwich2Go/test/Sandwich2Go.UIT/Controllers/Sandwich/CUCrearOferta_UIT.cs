using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;

namespace Sandwich2Go.UIT.Controllers.Sandwich
{
    public class CUCrearOferta_UIT : IDisposable
    {
        IWebDriver _driver;
        string _URI = "https://localhost:44343/";
        bool _pipeline = false;
        string username = "elena@uclm.com";
        string password = "Password1234%";

        public CUCrearOferta_UIT()
        {
            var optionsc = new ChromeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };

            if (_pipeline) optionsc.AddArgument("--headless");

            _driver = new ChromeDriver(optionsc);
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

        private void precondition_perform_login()
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

        private void first_step_accessing_crearOferta()
        {
            _driver.FindElement(By.Id("SandwichesController")).Click();
        }

        private void filter_sandwich_byName(string nameFilter)
        {
            _driver.FindElement(By.Id("filtrarPorNombre")).Clear();
            _driver.FindElement(By.Id("filtrarPorNombre")).SendKeys(nameFilter);
            _driver.FindElement(By.Id("filtrarSandwichesPorOferta")).Click();
        }

        private void filter_sandwich_byPrecio(string priceFilter)
        {
            _driver.FindElement(By.Id("filtrarPorPrecio")).Clear();
            _driver.FindElement(By.Id("filtrarPorPrecio")).SendKeys(priceFilter);
            _driver.FindElement(By.Id("filtrarSandwichesPorOferta")).Click();
        }

        private void filter_sandwich_byPrecioYNombre(string nameFilter, string priceFilter)
        {
            _driver.FindElement(By.Id("filtrarPorNombre")).Clear();
            _driver.FindElement(By.Id("filtrarPorNombre")).SendKeys(nameFilter);
            _driver.FindElement(By.Id("filtrarPorPrecio")).Clear();
            _driver.FindElement(By.Id("filtrarPorPrecio")).SendKeys(priceFilter);
            _driver.FindElement(By.Id("filtrarSandwichesPorOferta")).Click();
        }

        [Fact]
        public void UCSelectSandwich_Filtrar_Por_Nombre()
        {
            //Arrange
            string[] expectedText = { "Cubano", "4,00 €", "Queso Pan Huevo Jamon" };

            //Act
            precondition_perform_login();
            first_step_accessing_crearOferta();
            filter_sandwich_byName("Cubano");

            //Assert
            var sandwichRow = _driver.FindElements(By.Id("Sandwich_Name_" + expectedText[0]));

            Assert.NotNull(sandwichRow);

            foreach (string expected in expectedText)
            {
                Assert.NotNull(sandwichRow.First(l => l.Text.Contains(expected)));
            }
        }

        [Fact]
        public void UCSelectSandwich_Filtrar_Por_Precio()
        {
            //Arrange
            string[] expectedText = { "Mixto", "3,00 €", "Queso Pan Jamon" };

            //Act
            precondition_perform_login();
            first_step_accessing_crearOferta();
            filter_sandwich_byPrecio("3");

            //Assert
            var sandwichRow = _driver.FindElements(By.Id("Sandwich_Name_" + expectedText[0]));

            Assert.NotNull(sandwichRow);

            foreach (string expected in expectedText)
            {
                Assert.NotNull(sandwichRow.First(l => l.Text.Contains(expected)));
            }
        }

        [Fact]
        public void UCSelectSandwich_Filtrar_Por_PrecioYNombre()
        {
            //Arrange
            string[] expectedText = { "Mixto", "3,00 €", "Queso Pan Jamon" };

            //Act
            precondition_perform_login();
            first_step_accessing_crearOferta();
            filter_sandwich_byPrecioYNombre("Mixto", "3");

            //Assert
            var sandwichRow = _driver.FindElements(By.Id("Sandwich_Name_" + expectedText[0]));

            Assert.NotNull(sandwichRow);

            foreach (string expected in expectedText)
            {
                Assert.NotNull(sandwichRow.First(l => l.Text.Contains(expected)));
            }
        }

        [Fact]
        public void UCSelectSandwich_Filtrar_Con_Error()
        {
            //Arrange
            string expectedText = "No hay sándwiches disponibles";

            //Act
            precondition_perform_login();
            first_step_accessing_crearOferta();
            filter_sandwich_byName("Vacuno");

            //Assert
            Assert.Contains(expectedText, _driver.PageSource);
        }
        public void Dispose()
        {
            _driver.Close();
            _driver.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
