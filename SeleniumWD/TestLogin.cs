using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using OpenQA.Selenium.Interactions;

namespace SeleniumWebDriver
{
    [TestFixture]
    public class TestLogin
    {
        private IWebDriver driver;
        private string baseUrl;

        [SetUp]
        public void TestSetup()
        {
            var service = FirefoxDriverService.CreateDefaultService();
            this.driver = new FirefoxDriver(service);
            this.baseUrl = "http://yandex.by";

            this.driver.Navigate().GoToUrl(this.baseUrl);
            this.driver.Manage().Window.Maximize();
        }

        [Test]
        public void Login()
        {
            this.driver.FindElement(By.XPath("//div[@class='desk-notif-card__login-new-item-title']")).Click();
            this.driver.FindElement(By.Id("passp-field-login")).SendKeys("Snieczka");
            this.driver.FindElement(By.ClassName("Button2_view_action")).Click();
            this.driver.FindElement(By.Id("passp-field-passwd")).SendKeys("2020327abc");
            this.driver.FindElement(By.ClassName("Button2_view_action")).Click();
            isElementVisible(By.XPath("//a[contains(@href,'/compose')]"));
            var expectedExpression = this.driver.FindElement(By.XPath("//a[contains(@href,'/compose')]")).Text;
            Assert.AreEqual("Написать письмо", expectedExpression);

        }

        public void isElementVisible(By element, int timeout = 10)
        {
            new WebDriverWait(this.driver, TimeSpan.FromSeconds(timeout)).Until(ExpectedConditions.ElementIsVisible(element));
        }

        [TearDown]
         public void CleanUp()
         {
             this.driver.Close();
             this.driver.Quit();
         }
    }
}