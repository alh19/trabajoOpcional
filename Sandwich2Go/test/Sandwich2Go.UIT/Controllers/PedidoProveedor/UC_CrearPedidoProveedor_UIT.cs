﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
//Necesario para obtener Find dentro de las ICollection o IList
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace Sandwich2Go.UIT.Controllers.PedidoProveedor
{
    public class UC_CrearPedidoProveedor_UIT:IDisposable
    {
        readonly IWebDriver _driver;
        readonly string _URI = "https://localhost:5001/";
        readonly bool _pipeline = true;
        readonly string usernameG = "elena@uclm.com";
        readonly string passwordG = "Password1234%";
        readonly string usernameC = "gregorio@uclm.com";
        readonly string passwordC = "APassword1234%";




        public UC_CrearPedidoProveedor_UIT()
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



        private void first_step_accessing_purchases()
        {
            _driver.FindElement(By.Id("CrearPedidoProveedor")).Click();
        }

        private void second_step_accessing_link_Create_New()
        {
            _driver.FindElement(By.Id("ComprarIngredientes")).Click();
        }


        
        private void EscribirDatos(string id, string filterByIngrediente)
        {
            _driver.FindElement(By.Id(id)).Clear();
            _driver.FindElement(By.Id(id)).SendKeys(filterByIngrediente);
        }
        private void Third_step_select_sandwich(string nombre)
        {
            _driver.FindElement(By.Id("Ingrediente_"+nombre)).Click();
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
        public void UC3_0_FlujoBasicoTarjeta_CrearPedidoProveedor()
        {
            //Arrange
            string[] expectedTarjeta = { "1234567890123456", "123", "11", "2040" };
            string[] expectedDirecc = {  "Calle Arquitecto Vandelvira, 5" };
            string[] expectedIngrediente = { "Queso", "1", "1€" ,};
            string[] expectedDetails = { "Queso", "2", "1 €" };
            string[] expectedDetails1 = { "Queso", "2", "1€" };
            string[] expectedProveedor = { "Alberto", "11111a", "Calle1" };

            //Act
            Precondition_perform_login(this.usernameG, this.passwordG);
            first_step_accessing_purchases();
            second_step_accessing_link_Create_New();
            Third_step_select_sandwich(expectedIngrediente[0]);
            _driver.FindElement(By.Id("Guardar")).Click();
            EscribirDatos("Stock_Cantidad_" + expectedIngrediente[0], expectedIngrediente[1]);
            EscribirDatos("DireccionEntrega", expectedDirecc[0]);
            _driver.FindElement(By.Id("r11")).Click();
            Thread.Sleep(1000);
            EscribirDatos("NumeroTarjetaCredito", expectedTarjeta[0]);
            EscribirDatos("CCV", expectedTarjeta[1]);
            EscribirDatos("MesCad", expectedTarjeta[2]);
            EscribirDatos("AnoCad", expectedTarjeta[3]);
           
            _driver.FindElement(By.Id("RealizarCompra")).Click();

            

            var filaIngrediente = _driver.FindElements(By.Id("Ingrediente_" + expectedIngrediente[0]));
            foreach (string expected in expectedDetails)
            {
                Assert.NotNull(filaIngrediente.First(l => l.Text.Contains(expected)));
            }
            var precio = _driver.FindElements(By.Id("PrecioTotal"))[0].Text;

            Assert.Equal("Precio total del Pedido: "+expectedDetails1[2], precio);
        }






        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_1_FlujoBasicoEfectivo_CrearPedidoProveedor()
        {
            //Arrange
            string[] expectedTarjeta = { "1234567890123456", "123", "11", "2040" };
            string[] expectedDirecc = { "Calle Arquitecto Vandelvira, 5" };
            string[] expectedIngrediente = { "Huevo", "1", "1 €", };
            string[] expectedDetails = { "Huevo", "2", "1 €" };
            string[] expectedDetails1 = { "Huevo", "2", "1€" };
            string[] expectedProveedor = { "Alberto", "11111a", "Calle1" };

            //Act
            Precondition_perform_login(this.usernameG, this.passwordG);
            first_step_accessing_purchases();
            second_step_accessing_link_Create_New();
            Third_step_select_sandwich(expectedIngrediente[0]);
            _driver.FindElement(By.Id("Guardar")).Click();
            EscribirDatos("Stock_Cantidad_" + expectedIngrediente[0], expectedIngrediente[1]);
            EscribirDatos("DireccionEntrega", expectedDirecc[0]);
            _driver.FindElement(By.Id("r12")).Click();
            Thread.Sleep(1000);

            _driver.FindElement(By.Id("RealizarCompra")).Click();



            var filaIngrediente = _driver.FindElements(By.Id("Ingrediente_" + expectedIngrediente[0]));
            foreach (string expected in expectedDetails)
            {
                Assert.NotNull(filaIngrediente.First(l => l.Text.Contains(expected)));
            }
            var precio = _driver.FindElements(By.Id("PrecioTotal"))[0].Text;

            Assert.Equal("Precio total del Pedido: " + expectedDetails1[2], precio);
            
        }







        void IDisposable.Dispose()
        {
            _driver.Close();
            _driver.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
