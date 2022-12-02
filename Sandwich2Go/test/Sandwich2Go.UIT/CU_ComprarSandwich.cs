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

namespace Sandwich2Go.UIT
{
    public class CU_ComprarSandwich : IDisposable
    {
        IWebDriver _driver;
        string _URI = "https://localhost:5001/";
        bool _pipeline = false;

        public CU_ComprarSandwich()
        {
            var optionsc = new ChromeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };

            if(_pipeline) optionsc.AddArgument("--headless");

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
            .SendKeys("gregorio@uclm.com");

            _driver.FindElement(By.Id("Input_Password"))
            .SendKeys("APassword1234%");

            _driver.FindElement(By.Id("login-submit"))
            .Click();


        }

        private void first_step_accessing_comprarSandwich()
        {
            _driver.FindElement(By.Id("ComprarSandwich")).Click();
        }

        private void filter_sandwich_byPrecio(string titleFilter)
        {
            _driver.FindElement(By.Id("filterByPrecio")).Clear();
            _driver.FindElement(By.Id("filterByPrecio")).SendKeys(titleFilter);
            _driver.FindElement(By.Id("filterButton")).Click();
        }

        private void filter_sandwich_byAlergeno(string alergenoFilter)
        {
            var alergeno = _driver.FindElement(By.Id("sandwichAlergenoSelected"));
            SelectElement selectElement = new SelectElement(alergeno);
            selectElement.SelectByText(alergenoFilter);
            _driver.FindElement(By.Id("filterButton")).Click();
        }

        [Fact]
        public void UCSelectSandwich_Filtrar_Por_Precio()
        {
            //Arrange
            string[] expectedText = { "Mixto", "Ejemplo con Descuento de: 10%" , "3,00 €", "Queso Pan Jamon" , "Leche Glúten" };

            //Act
            precondition_perform_login();
            first_step_accessing_comprarSandwich();
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
        public void UCSelectSandwich_Filtrar_Con_Error()
        {
            //Arrange
            string expectedText = "No hay sándwiches disponibles";

            //Act
            precondition_perform_login();
            first_step_accessing_comprarSandwich();
            filter_sandwich_byPrecio("-1");

            //Assert
            Assert.Contains(expectedText, _driver.PageSource);
        }

        [Fact]
        public void UCSelectSandwich_Filtrar_Por_Alergeno()
        {
            //Arrange
            string[] expectedText = { "Mixto sin Glúten", "", "4,00 €", "Queso Jamon Pan sin Glúten", "Leche"};
            //Act
            precondition_perform_login();
            first_step_accessing_comprarSandwich();
            filter_sandwich_byAlergeno("Glúten");
            //Assert
            var sandwichRow = _driver.FindElements(By.Id("Sandwich_Name_" + expectedText[0]));
            Assert.NotNull(sandwichRow);
            foreach (string expected in expectedText)
            {
                Assert.NotNull(sandwichRow.First(l => l.Text.Contains(expected)));
            }
        }

        public void Dispose()
        {
            _driver.Close();
            _driver.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
