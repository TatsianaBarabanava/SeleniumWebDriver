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
    public class TestSendEmail
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
        public void SendEmail()
        {
            this.driver.FindElement(By.XPath("//div[@class='desk-notif-card__login-new-item-title']")).Click();
            this.driver.FindElement(By.Id("passp-field-login")).SendKeys("Snieczka");
            this.driver.FindElement(By.ClassName("Button2_view_action")).Click();
            this.driver.FindElement(By.Id("passp-field-passwd")).SendKeys("2020327abc");
            this.driver.FindElement(By.ClassName("Button2_view_action")).Click();
            isElementVisible(By.XPath("//a[contains(@href,'/compose')]"));
            this.driver.FindElement(By.XPath("//a[contains(@href,'/compose')]")).Click();

            this.driver.SwitchTo().Window(this.driver.WindowHandles.Last());
            isElementVisible(By.XPath("//div[@class='composeYabbles']"));
            this.driver.FindElement(By.XPath("//div[@class='composeYabbles']")).SendKeys("Snieczka@gmail.com");
            this.driver.FindElement(By.XPath("//input[contains(@class,'composeTextField')]")).Click();
            this.driver.FindElement(By.XPath("//input[contains(@class,'composeTextField')]")).SendKeys("Test Email");
            this.driver.FindElement(By.XPath("//div[@role='textbox']")).Click();
            this.driver.FindElement(By.XPath("//div[@role='textbox']")).SendKeys("Hello, My Gmail Mailbox");
            Actions builder1 = new Actions(driver);
            builder1.SendKeys(Keys.Escape).Perform();
            isElementVisible(By.XPath("//div[@title='Переслать (f)']"));
            this.driver.FindElement(By.XPath("//span[text()='Черновики']")).Click();
            isElementVisible(By.XPath("//span[@title='snieczka@gmail.com']"));
            int numberOfDrafts = this.driver.FindElements(By.XPath("//span[contains(@title,'snieczka')]")).Count();

            this.driver.FindElement(By.XPath("//span[@title='snieczka@gmail.com']")).Click();
            this.driver.FindElement(By.XPath("//button[contains(@class,'ComposeControlPanelButton-Button_action')]")).Click();

            isElementVisible(By.XPath("//span[text()='Письмо отправлено']"));

            Actions builder2 = new Actions(driver);
            builder2.SendKeys(Keys.Escape).Perform();

            bool isNumberRight = new WebDriverWait(this.driver, new TimeSpan(10)).Until(e => e.FindElements(By.XPath("//span[contains(@title,'snieczka')]")).Count.Equals(numberOfDrafts-1));
            Assert.IsTrue(isNumberRight);
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