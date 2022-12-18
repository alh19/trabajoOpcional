﻿using System;
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
using System.Diagnostics;
using OpenQA.Selenium.DevTools;
using System.Runtime.Intrinsics.X86;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;

namespace Sandwich2Go.UIT.Controllers.CrearSandwich
{
    public class UC_CrearSandwich_UIT : IDisposable
    {
        readonly IWebDriver _driver;
        readonly string _URI = "https://localhost:5001/";
        readonly bool _pipeline = true;
        readonly string usernameG = "elena@uclm.com";
        readonly string passwordG = "Password1234%";
        readonly string usernameC = "gregorio@uclm.com";
        readonly string passwordC = "APassword1234%";

        public UC_CrearSandwich_UIT()
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

        private void First_step_accessing_crearSandwich()
        {
            _driver.FindElement(By.Id("CrearSandwich")).Click();
        }

        private void Second_step_select_ingrediente(string nombre)
        {
            _driver.FindElement(By.Id("Ingrediente_" + nombre)).Click();
        }

        private void EscribirDatos(string id, string ingrFilter)
        {
            _driver.FindElement(By.Id(id)).Clear();
            _driver.FindElement(By.Id(id)).SendKeys(ingrFilter);
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
        public void UC4_0_FlujoBasicoTarjeta_CrearSandwich()
        {
            //Arrange
            string[] expectedIngr1 = { "1", "Queso", "1,00 €", "2" };
            string[] expectedIngr2 = { "6", "Pan sin Glúten", "2,00 €", "3" };
            string[] expectedIngr3 = { "5", "Jamon", "1,00 €", "1" };
            string[] expectedDetails = { "CallePrincipal", "Queso Jamon Pan sin Glúten", "7,00 €", "1" };
            string[] expectedCliente = { "CallePrincipal", "612345678", "2" };
            string[] expectedTarjeta = { "1234567891234567", "123", "12", "2025" };
            string[] expectedFecha = { "Fecha del pedido:" };
            string[] expectedPagina = { "Detalles del pedido:", "Fecha del pedido: ", "Volver a tus pedidos" };

            //Act
            Precondition_perform_login(this.usernameC, this.passwordC);
            First_step_accessing_crearSandwich();
            Second_step_select_ingrediente(expectedIngr1[0]);
            Second_step_select_ingrediente(expectedIngr2[0]);
            Second_step_select_ingrediente(expectedIngr3[0]);
            _driver.FindElement(By.Id("Save")).Click();
            EscribirDatos("DireccionEntrega", expectedCliente[0]);
            EscribirDatos("Telefono", expectedCliente[1]);
            EscribirDatos("Cantidad", expectedCliente[2]);
            EscribirDatos("Ingrediente_Quantity_" + expectedIngr1[0], expectedIngr1[3]);
            EscribirDatos("Ingrediente_Quantity_" + expectedIngr2[0], expectedIngr2[3]);
            EscribirDatos("Ingrediente_Quantity_" + expectedIngr3[0], expectedIngr3[3]);
            _driver.FindElement(By.Id("r11")).Click();
            Thread.Sleep(1000);
            EscribirDatos("NumeroTarjetaCredito", expectedTarjeta[0]);
            EscribirDatos("CCV", expectedTarjeta[1]);
            EscribirDatos("MesCad", expectedTarjeta[2]);
            EscribirDatos("AnoCad", expectedTarjeta[3]);
            _driver.FindElement(By.Id("CreateButton")).Click();

            //Assert
            //Assert.Contains(expectedFecha[0], _driver.PageSource);
            //Assert.Contains(DateTime.Today.ToString("d", CultureInfo.GetCultureInfo("es-ES")), _driver.PageSource);
            //foreach (string expected in expectedCliente)
            //{
            //    Assert.Contains(expected, _driver.PageSource);
            //}
            Assert.NotNull("Pagado con tarjeta *4567");

            
            //foreach (string expected in expectedPagina)
            //{
            //    Assert.Equal(expected, _driver.PageSource);
            //}

            var filaIngredientes = _driver.FindElements(By.Id("Ingrediente_" + expectedCliente[0]));
            foreach (string expected in expectedDetails)
            {
                Assert.NotNull(filaIngredientes.Select(l => l.Text.Contains(expected)));
            }
            Assert.NotNull("Precio total:\r\n        8,00 €");
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_1_FlujoBasicoEfectivo_CrearSandwich()
        {
            //Arrange
            string[] expectedIngr1 = { "1", "Queso", "1,00 €", "2" };
            string[] expectedIngr2 = { "6", "Pan sin Glúten", "2,00 €", "3" };
            string[] expectedIngr3 = { "5", "Jamon", "1,00 €", "1" };
            string[] expectedDetails = { "CallePrincipal", "Queso Jamon Pan sin Glúten", "7,00 €", "1" };
            string[] expectedCliente = { "CallePrincipal", "612345678", "2" };
            string[] expectedFecha = { "Fecha del pedido:" };
            string[] expectedPagina = { "Detalles del pedido:", "Fecha del pedido: ", "Volver a tus pedidos" };

            //Act
            Precondition_perform_login(this.usernameC, this.passwordC);
            First_step_accessing_crearSandwich();
            Second_step_select_ingrediente(expectedIngr1[0]);
            Second_step_select_ingrediente(expectedIngr2[0]);
            Second_step_select_ingrediente(expectedIngr3[0]);
            _driver.FindElement(By.Id("Save")).Click();
            EscribirDatos("DireccionEntrega", expectedCliente[2]);
            EscribirDatos("Telefono", expectedCliente[1]);
            EscribirDatos("Cantidad", expectedCliente[2]);
            _driver.FindElement(By.Id("r12")).Click();
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("necesitaCambio")).Click();
            _driver.FindElement(By.Id("CreateButton")).Click();

            //Assert
            Assert.NotNull(expectedFecha[0]);
            //Assert.Contains(DateTime.Today.ToString("d", CultureInfo.GetCultureInfo("es-ES")), _driver.PageSource);
            foreach (string expected in expectedCliente)
            {
                Assert.NotNull(expectedCliente.First(l => l.Contains(expected)));
            }
            Assert.NotNull("Solicitado pago en efectivo. Has pedido cambio.");

            var filaIngredientes = _driver.FindElements(By.Id("Sandwich_" + expectedIngr1[0]));
            foreach (string expected in expectedDetails)
            {
                Assert.NotNull(filaIngredientes.Select(l => l.Text.Contains(expected)));
            }
            Assert.NotNull("Precio total:\r\n        8,00 €");
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_2_FiltroNombre_CrearSandwich()
        {
            //Arrange
            string[] expectedIngr1 = { "1", "Queso", "1,00 €", "2" };

            //Act
            Precondition_perform_login(this.usernameC, this.passwordC);
            First_step_accessing_crearSandwich();
            EscribirDatos("ingredienteNombre", expectedIngr1[1]);
            _driver.FindElement(By.Id("filterButton")).Click();
            var filaIngr = _driver.FindElements(By.Id("Ingrediente_" + expectedIngr1[0]));

            //Assert
            Assert.Contains("Purchase Ingredientes", _driver.PageSource);
            foreach (string expected in expectedIngr1)
            {
                Assert.NotNull(filaIngr.Select(l => l.Text.Contains(expected)));
            }

        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_3_FiltroAlergeno_CrearSandwich()
        {
            //Arrange
            string[] expectedIngr1 = { "1", "Queso", "1,00 €", "2" };

            //Act
            Precondition_perform_login(this.usernameC, this.passwordC);
            First_step_accessing_crearSandwich();
            var alergeno = _driver.FindElement(By.Id("ingredienteAlergenoSelected"));
            SelectElement selectElement = new SelectElement(alergeno);
            selectElement.SelectByText("Glúten");
                _driver.FindElement(By.Id("filterButton")).Click();

            var filaIngr = _driver.FindElements(By.Id("Ingrediente_" + expectedIngr1[0]));
            //Assert
            Assert.Contains("Purchase Ingredientes", _driver.PageSource);
                foreach (string expected in expectedIngr1)
                {
                    Assert.NotNull(filaIngr.Select(l => l.Text.Contains(expected)));
                }
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_4_IngredientesNoDisponibles_CrearSandwich()
        {
            //Arrange
            string[] noExpected = { "Cacahuetes" };

            //Act
            Precondition_perform_login(this.usernameC, this.passwordC);
            First_step_accessing_crearSandwich();

            //Assert
            Assert.DoesNotContain(noExpected[0], _driver.PageSource);
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_5_IngredienteNoSeleccionado_CrearSandwich()
        {
            //Arrange
            string[] noExpected = { "Cacahuetes" };

            //Act
            Precondition_perform_login(this.usernameC, this.passwordC);
            First_step_accessing_crearSandwich();
            _driver.FindElement(By.Id("Save")).Click();

            //Assert
            Assert.Contains("You must select at least one ingrediente", _driver.PageSource);
        }

        public void Dispose()
        {
            _driver.Close();
            _driver.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
