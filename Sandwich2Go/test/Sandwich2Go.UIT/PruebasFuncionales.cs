using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
//Necesario para obtener Find dentro de las ICollection o IList
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;
using static System.Net.WebRequestMethods;

namespace Sandwich2Go.UIT
{
    public class PruebasFuncionales : IDisposable
    {
        //Falta Modificar Sandwich2Go.UIT con la información del pdf selenium

        //Webdriver: A reference to the browser
        IWebDriver _driver;
        //A reference to the URI of the web page to test
        string _URI = "https://localhost:44343/";

      
        public PruebasFuncionales()
        {
            var optionsc = new ChromeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                AcceptInsecureCertificates = true
            };

            _driver = new ChromeDriver(optionsc);
        }
        public void Initial_step_opening_the_web_page2()
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
            .SendKeys("peter@uclm.com");
            _driver.FindElement(By.Id("Input_Password"))
            .SendKeys("OtherPass12$");
            _driver.FindElement(By.Id("login-submit"))
            .Click();
        }


        [Fact]
        private void UC3_EscenarioFiltrar_PorTitulo()
        {
            //Arrange
            string[] expectedText = { "Lechuga", "2,00 €"};
            //Act
            precondition_perform_login();
            first_step_accessing_purchases();
            second_step_accessing_link_Create_New();
            third_filter_movies_byTitle("Lechuga");
            //Assert
         var movieRow = _driver
        .FindElements(By.Id("Ingrediente_Name_"+expectedText[0]));
        //Comprueba que la fila con ese id existe en la tabla
        Assert.NotNull(movieRow);
                    //Comprueba que todas las cadenas esperadas están incluidas en la fila
                    //obtenida.
                    foreach (string expected in expectedText)
                Assert.NotNull(movieRow.First(l => l.Text.Contains(expected)));
        }

        private void first_step_accessing_purchases()
        {
            _driver.FindElement(By.Id("IngredientesController")).Click();
        }


        private void second_step_accessing_link_Create_New()
        {
            _driver.FindElement(By.Id("IngredientesController")).Click();
        }

        private void third_filter_movies_byTitle(string titleFilter)
        {
            _driver.FindElement(By.Id("ingredienteNombre")).SendKeys(titleFilter);
            _driver.FindElement(By.Id("filterbyAlergeno")).Click();
        }
        public void Dispose()
        {
            //To close and release all the resources allocated by the web driver
            _driver.Close();
            _driver.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}


