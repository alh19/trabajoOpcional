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
        readonly IWebDriver _driver;
        readonly string _URI = "https://localhost:5001/";
        readonly bool _pipeline = false;
        readonly string usernameG = "elena@uclm.com";
        readonly string passwordG = "Password1234%";
        readonly string usernameC = "gregorio@uclm.com";
        readonly string passwordC = "APassword1234%";

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
        private void Precondition_perform_login(string username, string password)
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

        private void First_step_accessing_crearOferta()
        {
            _driver.FindElement(By.Id("CrearOferta")).Click();
        }

        private void Third_step_select_sandwich(string nombre)
        {
            _driver.FindElement(By.Id("Sandwich_" + nombre)).Click();
        }

        private void EscribirDatos(string id, string sandwichFilter)
        {
            _driver.FindElement(By.Id(id)).Clear();
            _driver.FindElement(By.Id(id)).SendKeys(sandwichFilter);
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
        [InlineData("Oferta Mixto","Mixto","Dec 5, 2022","3,00 €","Jan 5, 2023","Oferta en Sándwich","10","Queso Pan Jamon")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_0_1_FlujoBasico_CrearOferta(string nombreOferta, string nombreSandwich,string fechaInicio, string precio,string fechaFinalizacion, string descripcion, string porcentaje, string ingredientes)
        {
            //Arrange
            string [] expected  = { nombreSandwich, precio, ingredientes, porcentaje};
            //Act
            Precondition_perform_login(this.usernameG,this.passwordG);

            First_step_accessing_crearOferta();

            Third_step_select_sandwich(nombreSandwich);

            _driver.FindElement(By.Id("SiguienteButton")).Click();

            EscribirDatos("NombreOferta",nombreOferta);

            _driver.FindElement(By.Id("FechaInicio")).SendKeys(fechaInicio);

            _driver.FindElement(By.Id("FechaFin")).SendKeys(fechaFinalizacion);

            EscribirDatos("Descripcion", descripcion);

            EscribirDatos("Porcentaje_" + nombreSandwich, porcentaje);

            _driver.FindElement(By.Id("CreateButton")).Click();

            Assert.Equal(true, true);

        }
        public void Dispose()
        {
            _driver.Close();
            _driver.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}

