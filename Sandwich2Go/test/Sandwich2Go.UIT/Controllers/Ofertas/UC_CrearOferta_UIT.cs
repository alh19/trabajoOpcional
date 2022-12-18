using System;
using System.Linq;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Sandwich2Go.UIT.Controllers.Ofertas
{
    public class UC_CrearOferta_UIT : IDisposable
    {
        readonly IWebDriver _driver;
        readonly string _URI = "https://localhost:5001/";
        readonly bool _pipeline = true;
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
        [Trait("LevelTesting", "Functional Testing")]
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

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1_0_FlujoBasico_CrearOferta()
        {
            //Arrange
            string[] expectedPagina = { "Detalles de la oferta:", "Oferta Mixto", "30/04/2023", "10/10/2023", "Oferta en Sándwich Mixto"};
            string [] expectedSandwich  = { "Mixto", "3,00 €", "Queso Pan Jamon", "10" };

            //Act
            Precondition_perform_login(this.usernameG,this.passwordG);
            First_step_accessing_crearOferta();
            Third_step_select_sandwich(expectedSandwich[0]);
            _driver.FindElement(By.Id("SiguienteButton")).Click();
            EscribirDatos("NombreOferta", expectedPagina[1]);
            EscribirDatos("FechaInicio", expectedPagina[2]);
            EscribirDatos("FechaFin", expectedPagina[3]);
            EscribirDatos("Descripcion", expectedPagina[4]);
            EscribirDatos("Porcentaje_" + expectedSandwich[0], expectedSandwich[3]);
            _driver.FindElement(By.Id("CreateButton")).Click();

            //Assert
            foreach(string expected in expectedPagina){

                Assert.Contains(expected, _driver.PageSource);
            }
            var filaSandwich = _driver.FindElements(By.Id("Sandwich_"+expectedSandwich[0]));
            foreach(string expected in expectedSandwich)
            {
                Assert.NotNull(filaSandwich.First(l => l.Text.Contains(expected)));
            }

        }

        [Theory]
        [InlineData("Selecciona tus sándwiches","Cubano","4,00 €", "Queso Pan Huevo Jamon")]
        [InlineData("Selecciona tus sándwiches", "Submarino","5,00 €", "Queso Pepinillo Pan Huevo Jamon")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1_1_1_FiltroNombre_CrearOferta(string pagina,string nombreSandwich, string precio, string ingredientes)
        {
            //Arrange
            string[] expectedSandwich = { nombreSandwich,precio,ingredientes};

            //Act
            Precondition_perform_login(this.usernameG,this.passwordG);
            First_step_accessing_crearOferta();
            EscribirDatos("filtrarPorNombre", nombreSandwich);
            _driver.FindElement(By.Id("filtrarSandwichesPorOferta")).Click();
            var filaSandwich = _driver.FindElements(By.Id("Sandwich_Name_" + nombreSandwich));

            //Assert
            Assert.Contains(pagina, _driver.PageSource);
            foreach (string expected in expectedSandwich)
            {
                Assert.NotNull(filaSandwich.First(l => l.Text.Contains(expected)));
            }
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1_2_2_FiltroPrecio_CrearOferta()
        {
            //Arrange
            string[] expectedSandwich = { "Mixto", "3,00 €", "Queso Pan Jamon" };

            //Act
            Precondition_perform_login(this.usernameG, this.passwordG);
            First_step_accessing_crearOferta();
            EscribirDatos("filtrarPorPrecio", "3");
            _driver.FindElement(By.Id("filtrarSandwichesPorOferta")).Click();
            var filaSandwich = _driver.FindElements(By.Id("Sandwich_Name_" + expectedSandwich[0]));

            //Assert
            Assert.Contains("Selecciona tus sándwiches", _driver.PageSource);
            foreach (string expected in expectedSandwich)
            {
                Assert.NotNull(filaSandwich.First(l => l.Text.Contains(expected)));
            }

        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1_3_2_SandwichesNoEncontrados_CrearOferta()
        {
            //Arrange
            string[] expected = { "Selecciona tus sándwiches", "No hay sándwiches disponibles" };

            //Act
            Precondition_perform_login(this.usernameG, this.passwordG);
            First_step_accessing_crearOferta();
            EscribirDatos("filtrarPorNombre", "No Existe");
            _driver.FindElement(By.Id("filtrarSandwichesPorOferta")).Click();

            //Assert
            foreach (string linea in expected)
            {
                Assert.Contains(linea, _driver.PageSource);
            }

        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1_4_4_SandwichesNoSeleccionados_CrearOferta()
        {
            //Arrange
            string[] expected = {"Selecciona tus sándwiches", "Debes seleccionar al menos un Sándwich"};

            //Act
            Precondition_perform_login(this.usernameG, this.passwordG);
            First_step_accessing_crearOferta();
            _driver.FindElement(By.Id("SiguienteButton")).Click();

            var Text = _driver.FindElement(By.Id("ModelErrors")).Text;

            //Assert

            Assert.Equal(expected[1], Text);
            foreach (string linea in expected)
            {
                Assert.Contains(linea, _driver.PageSource);
            }

        }

        [Theory]
        [InlineData("", "30/04/2023","10/10/2023", "Oferta en Sándwich Mixto", "10","Create" , "Oferta" , "The Nombre de la oferta:  field is required.", "NombreOferta-error", "The Nombre de la oferta: field is required.")]
        [InlineData("Oferta Mixto", "", "10/10/2023", "Oferta en Sándwich Mixto", "10", "Create", "Oferta", "The Fecha de inicio:  field is required.", "FechaInicio-error", "The Fecha de inicio: field is required.")]
        [InlineData("Oferta Mixto", "30/04/2023", "", "Oferta en Sándwich Mixto", "10", "Create", "Oferta", "The Fecha de finalización:  field is required.", "FechaFin-error", "The Fecha de finalización: field is required.")]
        [InlineData("Oferta Mixto", "30/04/2023", "10/10/2023", "Oferta en Sándwich Mixto", "0", "Create", "Oferta", "Introduce un porcentaje válido para el sándwich Mixto", "ModelErrors", "Introduce un porcentaje válido para el sándwich Mixto")]
        [InlineData("Oferta Mixto", "30/04/2023", "10/10/2023", "Oferta en Sándwich Mixto", "", "Create", "Oferta", "The Porcentaje field is required.", "Porcentaje_Mixto-error", "The Porcentaje field is required.")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1_5678_5_DatosNoIntroducidos_CrearOferta(string nombre, string fechaInicio, string fechaFin, string descripcion, string porcentaje,string pag1, string pag2, string error, string errorId, string errorVista)
        {
            //Arrange
            string[] datos = { nombre, fechaInicio, fechaFin, descripcion, porcentaje ,errorId};
            string[] expectedPagina = { pag1, pag2, error}; 
            //Act
            Precondition_perform_login(this.usernameG, this.passwordG);
            First_step_accessing_crearOferta();
            Third_step_select_sandwich("Mixto");
            _driver.FindElement(By.Id("SiguienteButton")).Click();
            EscribirDatos("NombreOferta", datos[0]);
            EscribirDatos("FechaInicio", datos[1]);
            EscribirDatos("FechaFin", datos[2]);
            EscribirDatos("Descripcion", datos[3]);
            EscribirDatos("Porcentaje_Mixto", datos[4]);
            _driver.FindElement(By.Id("CreateButton")).Click();

            var Text = _driver.FindElement(By.Id(datos[5])).Text;
            //Assert

            Assert.Equal(errorVista,Text);

            foreach (string expected in expectedPagina)
            {

                Assert.Contains(expected, _driver.PageSource);
            }
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1_9_1_SesionNoIniciada_CrearOferta()
        {
            //Arrange
            string[] expectedPagina = { "Log in" , "Use a local account to log in."};

            //Act
            _driver.Navigate().GoToUrl(_URI +
            "Sandwiches/SelectSandwichesForOffer");

            //Assert
            foreach (string expected in expectedPagina)
            {

                Assert.Contains(expected, _driver.PageSource);
            }
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC1_10_1_RolInvalido_CrearOferta()
        {
            //Arrange
            string[] expectedPagina = { "Access denied" , "You do not have access to this resource." };

            //Act
            Precondition_perform_login(this.usernameC, this.passwordC);
            _driver.Navigate().GoToUrl(_URI +
            "Sandwiches/SelectSandwichesForOffer");

            //Assert
            foreach (string expected in expectedPagina)
            {

                Assert.Contains(expected, _driver.PageSource);
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

