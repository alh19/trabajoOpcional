using System;
using System.Linq;
using System.Threading;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Globalization;

namespace Sandwich2Go.UIT.Controllers.Pedidos
{
    public class UC_ComprarSandwich_UIT : IDisposable
    {
        readonly IWebDriver _driver;
        readonly string _URI = "https://localhost:5001/";
        readonly bool _pipeline = true;
        readonly string usernameG = "elena@uclm.com";
        readonly string passwordG = "Password1234%";
        readonly string usernameC = "gregorio@uclm.com";
        readonly string passwordC = "APassword1234%";

        public UC_ComprarSandwich_UIT()
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

        private void First_step_accessing_comprarSandwich()
        {
            _driver.FindElement(By.Id("ComprarSandwich")).Click();
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
        public void UC2_0_FlujoBasicoTarjeta_ComprarSandwich()
        {
            //Arrange
            string[] expectedTarjeta = { "1234567890123456", "123", "11", "2040" };
            string[] expectedSandwich = { "Cubano", "4,00 €", "Queso Pan Huevo Jamon", "Leche Glúten Huevo", "1" };
            string[] expectedDetails = { "Cubano", "Queso Pan Huevo Jamon", "4,00 €", "1" };
            string[] expectedCliente = { "Gregorio", "Diaz Descalzo", "Calle Arquitecto Vandelvira, 5" };
            string[] expectedFecha = { "Fecha del pedido:" };
            
            //Act
            Precondition_perform_login(this.usernameC, this.passwordC);
            First_step_accessing_comprarSandwich();
            Third_step_select_sandwich(expectedSandwich[0]);
            _driver.FindElement(By.Id("Seleccionar")).Click();
            EscribirDatos("DireccionEntrega", expectedCliente[2]);
            _driver.FindElement(By.Id("r11")).Click();
            Thread.Sleep(1000);
            EscribirDatos("NumeroTarjetaCredito", expectedTarjeta[0]);
            EscribirDatos("CCV", expectedTarjeta[1]);
            EscribirDatos("MesCad", expectedTarjeta[2]);
            EscribirDatos("AnoCad", expectedTarjeta[3]);
            EscribirDatos("Cantidad_" + expectedSandwich[0], expectedSandwich[4]);
            _driver.FindElement(By.Id("CreateButton")).Click();

            //Assert
            Assert.Contains(expectedFecha[0], _driver.PageSource);
            Assert.Contains(DateTime.Today.ToString("d", CultureInfo.GetCultureInfo("es-ES")), _driver.PageSource);
            foreach (string expected in expectedCliente)
            {
                Assert.Contains(expected, _driver.PageSource);
            }
            Assert.Contains("Pagado con tarjeta *3456", _driver.PageSource);

            var filaSandwich = _driver.FindElements(By.Id("Sandwich_" + expectedSandwich[0]));
            foreach (string expected in expectedDetails)
            {
                Assert.NotNull(filaSandwich.First(l => l.Text.Contains(expected)));
            }
            Assert.Contains("Precio total:\r\n        4,00 €", _driver.PageSource);
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC2_1_FlujoBasicoEfectivo_ComprarSandwich()
        {
            //Arrange
            string[] expectedCliente = { "Gregorio", "Diaz Descalzo", "Calle Arquitecto Vandelvira, 5" };
            string[] expectedSandwich = { "Cubano", "4,00 €", "Queso Pan Huevo Jamon", "Leche Glúten Huevo", "1" };
            string[] expectedDetails = { "Cubano", "Queso Pan Huevo Jamon", "4,00 €", "1" };
            string[] expectedFecha = { "Fecha del pedido:" };

            //Act
            Precondition_perform_login(this.usernameC, this.passwordC);
            First_step_accessing_comprarSandwich();
            Third_step_select_sandwich(expectedSandwich[0]);
            _driver.FindElement(By.Id("Seleccionar")).Click();
            EscribirDatos("DireccionEntrega", expectedCliente[2]);
            EscribirDatos("Cantidad_" + expectedSandwich[0], expectedSandwich[4]);
            _driver.FindElement(By.Id("r12")).Click();
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("necesitaCambio")).Click();
            _driver.FindElement(By.Id("CreateButton")).Click();

            //Assert
            Assert.Contains(expectedFecha[0], _driver.PageSource);
            Assert.Contains(DateTime.Today.ToString("d", CultureInfo.GetCultureInfo("es-ES")), _driver.PageSource);
            foreach (string expected in expectedCliente)
            {
                Assert.Contains(expected, _driver.PageSource);
            }
            Assert.Contains("Solicitado pago en efectivo. Has pedido cambio.", _driver.PageSource);

            var filaSandwich = _driver.FindElements(By.Id("Sandwich_" + expectedSandwich[0]));
            foreach (string expected in expectedDetails)
            {
                Assert.NotNull(filaSandwich.First(l => l.Text.Contains(expected)));
            }
            Assert.Contains("Precio total:\r\n        4,00 €", _driver.PageSource);
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC2_2_FiltroAlergeno_ComprarSandwich()
        {
            //Arrange
            string[] expectedSandwich = { "Mixto sin Gúten", "", "4,00 €", "Queso Jamon Pan sin Glúten", "Leche" };

            //Act
            Precondition_perform_login(this.usernameC, this.passwordC);
            First_step_accessing_comprarSandwich();
            var alergeno = _driver.FindElement(By.Id("sandwichAlergenoSelected"));
            SelectElement selectElement = new SelectElement(alergeno);
            selectElement.SelectByText("Glúten");
            _driver.FindElement(By.Id("filterButton")).Click();
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
        public void UC2_3_FiltroPrecioyAlergeno_ComprarSandwich()
        {
            //Arrange
            string[] expectedSandwich = { "Mixto sin Gúten", "", "4,00 €", "Queso Jamon Pan sin Glúten", "Leche" };

            //Act
            Precondition_perform_login(this.usernameC, this.passwordC);
            First_step_accessing_comprarSandwich();
            var alergeno = _driver.FindElement(By.Id("sandwichAlergenoSelected"));
            SelectElement selectElement = new SelectElement(alergeno);
            selectElement.SelectByText("Glúten");
            EscribirDatos("filterByPrecio", "4");
            _driver.FindElement(By.Id("filterButton")).Click();
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
        public void UC2_4_FiltroPrecio_ComprarSandwich()
        {
            //Arrange
            string[] expectedSandwich = { "Mixto", "Ejemplo con Descuento de: 10%", "3,00 €", "Queso Pan Jamon", "Leche Glúten" };

            //Act
            Precondition_perform_login(this.usernameC, this.passwordC);
            First_step_accessing_comprarSandwich();
            EscribirDatos("filterByPrecio", "3");
            _driver.FindElement(By.Id("filterButton")).Click();
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
        public void UC2_5_SandwichesIngredientesNoDisponibles_ComprarSandwich()
        {
            //Arrange
            string[] noExpected = { "Submarino" };

            //Act
            Precondition_perform_login(this.usernameC, this.passwordC);
            First_step_accessing_comprarSandwich();

            //Assert
            Assert.DoesNotContain(noExpected[0], _driver.PageSource);
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC2_6_SandwichesNoSeleccionados_ComprarSandwich()
        {
            //Arrange
            string[] expected = { "Debes seleccionar al menos un Sándwich" };

            //Act
            Precondition_perform_login(this.usernameC, this.passwordC);
            First_step_accessing_comprarSandwich();
            _driver.FindElement(By.Id("Seleccionar")).Click();

            //Assert
            foreach (string linea in expected)
            {
                Assert.Contains(linea, _driver.PageSource);
            }

        }
        [Fact]
        public void UC2_7_MostrarOferta_ComprarSandwich()
        {
            //Arrange
            string[] expectedSandwich = { "Mixto", "Ejemplo con Descuento de: 10%", "3,00 €", "Queso Pan Jamon", "Leche Glúten" };

            //Act
            Precondition_perform_login(this.usernameC, this.passwordC);
            First_step_accessing_comprarSandwich();
            EscribirDatos("filterByPrecio", "3");
            _driver.FindElement(By.Id("filterButton")).Click();
            var filaSandwich = _driver.FindElements(By.Id("Sandwich_Name_" + expectedSandwich[0]));

            //Assert
            Assert.Contains("Selecciona tus sándwiches", _driver.PageSource);
            foreach (string expected in expectedSandwich)
            {
                Assert.NotNull(filaSandwich.First(l => l.Text.Contains(expected)));
            }

        }
        [Theory]
        [InlineData("", "1234567890123456", "123", "11", "2040", "1", "Crear", "Pedido", "Por favor, introduce una dirección de entrega.")]
        [InlineData("Calle Arquitecto Vandelvira, 5", "", "123", "11", "2040", "1", "Crear", "Pedido", "Por favor, rellena el número de la tarjeta de crédito")]
        [InlineData("Calle Arquitecto Vandelvira, 5", "1234567890123456", "", "11", "2040", "1", "Crear", "Pedido", "Por favor, rellena el CCV de la tarjeta de crédito")]
        [InlineData("Calle Arquitecto Vandelvira, 5", "1234567890123456", "123", "", "2040", "1", "Crear", "Pedido", "Por favor, rellena el mes de caducidad de la tarjeta de crédito")]
        [InlineData("Calle Arquitecto Vandelvira, 5", "1234567890123456", "123", "11", "", "1", "Crear", "Pedido", "Por favor, rellena el año de caducidad de la tarjeta de crédito")]
        [InlineData("Calle Arquitecto Vandelvira, 5", "1234567890123456", "123", "11", "2040", "", "Crear", "Pedido", "The cantidad field is required.")]
        [InlineData("Calle Arquitecto Vandelvira, 5", "1234567890123456", "123", "11", "2040", "20", "Crear", "Pedido", "Minimo un sándwich, máximo 10 de un mismo tipo por pedido")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC2_891011121314_DatosNoIntroducidos_ComprarSandwich(string direccion, string tarjeta, string cvv, string mescad, string anocad, string cantidad, string pag1, string pag2, string error)
        {
            //Arrange
            string[] datos = { direccion, tarjeta, cvv, mescad, anocad, cantidad };
            string[] expectedPagina = { pag1, pag2, error };
            //Act
            Precondition_perform_login(this.usernameC, this.passwordC);
            First_step_accessing_comprarSandwich();
            Third_step_select_sandwich("Mixto");
            _driver.FindElement(By.Id("Seleccionar")).Click();
            EscribirDatos("DireccionEntrega", datos[0]);
            _driver.FindElement(By.Id("r11")).Click();
            Thread.Sleep(1000);
            EscribirDatos("NumeroTarjetaCredito", datos[1]);
            EscribirDatos("CCV", datos[2]);
            EscribirDatos("MesCad", datos[3]);
            EscribirDatos("AnoCad", datos[4]);
            EscribirDatos("Cantidad_Mixto", datos[5]);
            _driver.FindElement(By.Id("CreateButton")).Click();
            //Assert
            foreach (string expected in expectedPagina)
            {
                Assert.Contains(expected, _driver.PageSource);
            }
        }

        [Fact]
        public void UC2_15_PrecioTotalConDescuento_ComprarSandwich()
        {
            //Arrange
            string[] expectedTarjeta = { "1234567890123456", "123", "11", "2040" };
            string[] expectedSandwich = { "Mixto", "3,00 €", "Queso Pan Jamon", "Leche Glúten", "1" };
            string[] expectedDetails = { "Mixto", "Queso Pan Jamon", "2,70 €", "1" };
            string[] expectedCliente = { "Gregorio", "Diaz Descalzo", "Calle Arquitecto Vandelvira, 5" };
            string[] expectedOferta = { "Ofertas aplicadas:", "Mixto con oferta Oferta 1 y descuento del 10% ..... -2,70 €" };
            string[] expectedFecha = { "Fecha del pedido:" };

            //Act
            Precondition_perform_login(this.usernameC, this.passwordC);
            First_step_accessing_comprarSandwich();
            Third_step_select_sandwich(expectedSandwich[0]);
            _driver.FindElement(By.Id("Seleccionar")).Click();
            EscribirDatos("DireccionEntrega", expectedCliente[2]);
            _driver.FindElement(By.Id("r11")).Click();
            Thread.Sleep(1000);
            EscribirDatos("NumeroTarjetaCredito", expectedTarjeta[0]);
            EscribirDatos("CCV", expectedTarjeta[1]);
            EscribirDatos("MesCad", expectedTarjeta[2]);
            EscribirDatos("AnoCad", expectedTarjeta[3]);
            EscribirDatos("Cantidad_" + expectedSandwich[0], expectedSandwich[4]);
            _driver.FindElement(By.Id("CreateButton")).Click();

            //Assert
            Assert.Contains(expectedFecha[0], _driver.PageSource);
            Assert.Contains(DateTime.Today.ToString("d", CultureInfo.GetCultureInfo("es-ES")), _driver.PageSource);
            foreach (string expected in expectedCliente)
            {
                Assert.Contains(expected, _driver.PageSource);
            }
            Assert.Contains("Pagado con tarjeta *3456", _driver.PageSource);

            var filaSandwich = _driver.FindElements(By.Id("Sandwich_" + expectedSandwich[0]));
            foreach (string expected in expectedDetails)
            {
                Assert.NotNull(filaSandwich.First(l => l.Text.Contains(expected)));
            }
            Assert.Contains(expectedOferta[0], _driver.PageSource);
            Assert.Contains(expectedOferta[1], _driver.PageSource);
            Assert.Contains("Precio total:\r\n        2,70 €", _driver.PageSource);
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC2_16_SesionNoIniciada_ComprarSandwich()
        {
            //Arrange
            string[] expectedPagina = { "Log in", "Use a local account to log in." };

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
        public void UC2_17_RolInvalido_ComprarSanwich()
        {
            //Arrange
            string[] expectedPagina = { "Access denied", "You do not have access to this resource." };

            //Act
            Precondition_perform_login(this.usernameG, this.passwordG);
            _driver.Navigate().GoToUrl(_URI +
            "Sandwiches/SelectSandwichForPurchase");

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
