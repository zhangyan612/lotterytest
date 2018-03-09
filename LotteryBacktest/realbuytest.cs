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
    public class Realbuytest
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
        public void TheRealbuyTest()
        {
            driver.Navigate().GoToUrl(baseURL + "/");
            driver.FindElement(By.Id("username")).Clear();
            driver.FindElement(By.Id("username")).SendKeys("zhangyanwin");
            driver.FindElement(By.Id("password")).Clear();
            driver.FindElement(By.Id("password")).SendKeys("zymeng90612");
            driver.FindElement(By.Id("validCode")).Clear();
            driver.FindElement(By.Id("validCode")).SendKeys("rgz7");
            driver.FindElement(By.Id("subDiv")).Click();
            // ERROR: Caught exception [ERROR: Unsupported command [selectFrame | gameiframe | ]]
            driver.FindElement(By.XPath("//div[@id='swanfa']/div/ul/li[8]/div")).Click();
            // ERROR: Caught exception [ERROR: Unsupported command [selectWindow | null | ]]
            driver.FindElement(By.LinkText("我的订单")).Click();
            driver.FindElement(By.LinkText("×")).Click();
            // ERROR: Caught exception [ERROR: Unsupported command [selectFrame | gameiframe | ]]
            driver.FindElement(By.Id("wuwei8")).Click();
            driver.FindElement(By.Id("wuwei5")).Click();
            driver.FindElement(By.Id("wuwei4")).Click();
            driver.FindElement(By.Id("wuwei2")).Click();
            driver.FindElement(By.Id("wuwei1")).Click();
            driver.FindElement(By.Id("btadd")).Click();
            driver.FindElement(By.Id("submitbt")).Click();
            driver.FindElement(By.CssSelector("button.aui_state_highlight")).Click();
            Assert.AreEqual("投注成功!", CloseAlertAndGetItsText());
            driver.FindElement(By.LinkText("×")).Click();
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
