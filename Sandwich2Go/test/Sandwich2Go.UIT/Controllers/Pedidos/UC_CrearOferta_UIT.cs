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
        [InlineData("Detalles de la oferta:","Oferta Mixto","Mixto","30/04/2023","3,00 €", "10/10/2023", "Oferta en Sándwich","10","Queso Pan Jamon")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_0_FlujoBasico_CrearOferta(string pagina,string nombreOferta, string nombreSandwich,string fechaInicio, string precio,string fechaFinalizacion, string descripcion, string porcentaje, string ingredientes)
        {
            //Arrange
            string[] expectedPagina = { pagina,nombreOferta,fechaInicio,fechaFinalizacion,descripcion};
            string [] expectedSandwich  = { nombreSandwich, precio, ingredientes, porcentaje};
            //Act
            Precondition_perform_login(this.usernameG,this.passwordG);

            First_step_accessing_crearOferta();

            Third_step_select_sandwich(nombreSandwich);

            _driver.FindElement(By.Id("SiguienteButton")).Click();

            EscribirDatos("NombreOferta",nombreOferta);

            EscribirDatos("FechaInicio",fechaInicio);

            EscribirDatos("FechaFin", fechaFinalizacion);

            EscribirDatos("Descripcion", descripcion);

            EscribirDatos("Porcentaje_" + nombreSandwich, porcentaje);

            _driver.FindElement(By.Id("CreateButton")).Click();

            foreach(string expected in expectedPagina){

                Assert.Contains(expected, _driver.PageSource);
            }

            var filaSandwich = _driver.FindElements(By.Id("Sandwich_"+nombreSandwich));

            foreach(string expected in expectedSandwich)
            {
                Assert.NotNull(filaSandwich.First(l => l.Text.Contains(expected)));
            }

        }

        [Theory]
        [InlineData("Selecciona tus sándwiches","Cubano","4,00 €", "Queso Pan Huevo Jamon")]
        [InlineData("Selecciona tus sándwiches", "Submarino","5,00 €", "Queso Pepinillo Pan Huevo Jamon")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_1_1_FiltroNombre_CrearOferta(string pagina,string nombreSandwich, string precio, string ingredientes)
        {
            string[] expectedSandwich = { nombreSandwich,precio,ingredientes};

            Precondition_perform_login(this.usernameG,this.passwordG);
            First_step_accessing_crearOferta();
            EscribirDatos("filtrarPorNombre", nombreSandwich);

            _driver.FindElement(By.Id("filtrarSandwichesPorOferta")).Click();

            var filaSandwich = _driver.FindElements(By.Id("Sandwich_Name_" + nombreSandwich));

            Assert.Contains(pagina, _driver.PageSource);

            foreach (string expected in expectedSandwich)
            {
                Assert.NotNull(filaSandwich.First(l => l.Text.Contains(expected)));
            }
        }

        [Theory]
        [InlineData("Selecciona tus sándwiches", "Mixto", "3,00 €", "3","Queso Pan Jamon")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_2_1_FiltroPrecio_CrearOferta(string pagina, string nombreSandwich, string precioE, string precio, string ingredientes)
        {
            string[] expectedSandwich = { nombreSandwich, precioE, ingredientes };

            Precondition_perform_login(this.usernameG, this.passwordG);
            First_step_accessing_crearOferta();
            EscribirDatos("filtrarPorPrecio", precio);

            _driver.FindElement(By.Id("filtrarSandwichesPorOferta")).Click();

            var filaSandwich = _driver.FindElements(By.Id("Sandwich_Name_" + nombreSandwich));

            Assert.Contains(pagina, _driver.PageSource);

            foreach (string expected in expectedSandwich)
            {
                Assert.NotNull(filaSandwich.First(l => l.Text.Contains(expected)));
            }

        }

        [Theory]
        [InlineData("Selecciona tus sándwiches", "No hay sándwiches disponibles","No Existe")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_3_2_SandwichesNoEncontrados_CrearOferta(string pagina, string resultadoEsperado, string nombreSandwich)
        {
            string[] expected = { pagina, resultadoEsperado };

            Precondition_perform_login(this.usernameG, this.passwordG);
            First_step_accessing_crearOferta();
            EscribirDatos("filtrarPorNombre", nombreSandwich);

            _driver.FindElement(By.Id("filtrarSandwichesPorOferta")).Click();


            

            foreach (string linea in expected)
            {
                Assert.Contains(linea, _driver.PageSource);
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

