using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests
{
    [TestFixture]
    public class FulibaoComplete
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;
        
        [SetUp]
        public void SetupTest()
        {
            driver = new FirefoxDriver();
            baseURL = "http://fulibao7.com/";
            verificationErrors = new StringBuilder();
        }
        
        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }
        
        [Test]
        public void TheFulibaoCompleteTest()
        {
            driver.Navigate().GoToUrl(baseURL + "/");
            driver.FindElement(By.Id("username")).Clear();
            driver.FindElement(By.Id("username")).SendKeys("zhangyanwin");
            driver.FindElement(By.Id("password")).Clear();
            driver.FindElement(By.Id("password")).SendKeys("zymeng90612");
            driver.FindElement(By.Id("validCode")).Clear();
            driver.FindElement(By.Id("validCode")).SendKeys("pu9n");
            driver.FindElement(By.Id("subDiv")).Click();

            // Switch to iframe
            // ERROR: Caught exception [ERROR: Unsupported command [selectFrame | gameiframe | ]]
            driver.FindElement(By.Id("djs")).Click(); // time
            driver.FindElement(By.Id("newqh")).Click(); // expect
            driver.FindElement(By.XPath("//div[@id='swanfa']/div/ul/li[8]/div")).Click(); // dingwei
            driver.FindElement(By.Id("wuwei1")).Click();
            driver.FindElement(By.Id("wuwei3")).Click();
            driver.FindElement(By.Id("wuwei4")).Click();
            driver.FindElement(By.Id("wuwei6")).Click();
            driver.FindElement(By.Id("wuwei7")).Click();

            driver.FindElement(By.Id("btadd")).Click();
            driver.FindElement(By.Id("dzje")).Clear();
            driver.FindElement(By.Id("dzje")).SendKeys("5");

            driver.FindElement(By.Id("submitbt")).Click();
            driver.FindElement(By.XPath("(//button[@type='button'])[2]")).Click();

            // Switch back to main page to view
            // ERROR: Caught exception [ERROR: Unsupported command [selectWindow | null | ]]
            driver.FindElement(By.LinkText("我的订单")).Click();
            driver.FindElement(By.LinkText("查看")).Click();
            driver.FindElement(By.LinkText("×")).Click();
            driver.FindElement(By.XPath("(//a[contains(text(),'×')])[2]")).Click();
        }
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        
        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }
        
        private string CloseAlertAndGetItsText() {
            try {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert) {
                    alert.Accept();
                } else {
                    alert.Dismiss();
                }
                return alertText;
            } finally {
                acceptNextAlert = true;
            }
        }
    }
}
