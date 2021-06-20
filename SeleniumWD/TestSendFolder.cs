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
    public class TestSendFolder
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
        public void SendFolder()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            String randomText = new string(Enumerable.Range(1, 10).Select(_ => chars[random.Next(chars.Length)]).ToArray());
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
            this.driver.FindElement(By.XPath("//div[@role='textbox']")).SendKeys(randomText);
            Actions builder1 = new Actions(driver);
            builder1.SendKeys(Keys.Escape).Perform();

            isElementVisible(By.XPath("//div[@title='Переслать (f)']"));
            this.driver.FindElement(By.XPath("//span[text()='Черновики']")).Click();
            isElementVisible(By.XPath("//span[@title='snieczka@gmail.com']"));

            this.driver.FindElement(By.XPath("//span[@title='snieczka@gmail.com']")).Click();
            this.driver.FindElement(By.XPath("//button[contains(@class,'ComposeControlPanelButton-Button_action')]")).Click();
            Actions builder2 = new Actions(driver);
            builder2.SendKeys(Keys.Escape).Perform();
            isElementVisible(By.XPath("//span[text()='Вид']"));

            this.driver.FindElement(By.XPath("//span[text()='Отправленные']")).Click();
            var ExpectedSender = this.driver.FindElement(By.XPath("//span[contains(@class,'mail-MessageSnippet-Item mail-MessageSnippet-Item_sender')]")).Text;
            var ExpectedSubject = this.driver.FindElement(By.XPath("//span[contains(@class,'mail-MessageSnippet-Item_subject')]")).Text;
            var ExpectedContent = this.driver.FindElement(By.XPath("//span[contains(@class,'mail-MessageSnippet-Item_firstline')]")).Text;
            Assert.AreEqual("Snieczka@gmail.com", ExpectedSender);
            Assert.AreEqual("Test Email", ExpectedSubject);
            Assert.AreEqual(randomText, ExpectedContent);
        }

        public void isElementVisible(By element, int timeout = 10)
        {
            new WebDriverWait(this.driver, TimeSpan.FromSeconds(timeout)).Until(ExpectedConditions.ElementIsVisible(element));
        }

        [TearDown]
         public void CleanUp()
         {
             this.driver.FindElement(By.XPath("//a[contains(@class,'user-account_left-name')]")).Click();
             this.driver.FindElement(By.XPath("//span[text()='Выйти из сервисов Яндекса']")).Click();
             this.driver.Close();
             this.driver.Quit();
         }
    }
}