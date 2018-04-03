using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;

namespace NUTests
{
    [TestFixture]
    [Description("simple, independent tests")]
    public class TestClass1
    {
        [Test]
        [Description("doing a Google search")]
        public void FF_GoogleSearch()
        {
            using (var driver = new FirefoxDriver())
            {
                string searchKey = "selenium";
                driver.Manage().Window.Maximize();
                driver.Url = "https://www.google.ro";

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("lst-ib")));
                obj.SendKeys(searchKey + Keys.Tab);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(ExpectedConditions.TextToBePresentInElementValue(By.Id("lst-ib"), searchKey));

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[@name='btnK']")));
                obj.Click();

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//h3/a")));
                string url = obj.GetAttribute("href");
                driver.Navigate().GoToUrl(url);

                // checking the page title
                string title = driver.Title;
                bool titleOk = title.Contains("Selenium");
                Assert.That(titleOk, Is.True);

                driver.Quit();

            } // 

        } // GoogleSearch



        [Test]
        [Description("Searching on OLX")]
        public void FF_OlxSimpleSearch()
        {
            string res = "Search error";
            string searchKey = "ipad";

            using (var driver = new FirefoxDriver())
            {
                driver.Manage().Window.Maximize();
                var jse = (IJavaScriptExecutor)driver;

                driver.Url = "https://www.olx.ro";

                // closing bottom bar for informing about 
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//a[starts-with(@class,'cookiesBarClose') and contains(@class,'cfff')]")));
                obj.Click();

                // looking for a link to a list of cities
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//a[@title='Harta judetelor']")));
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", obj);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(ExpectedConditions.ElementToBeClickable(obj));
                obj.Click();

                // looking for the link of city Oradea
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//a[@title='Anunturi Oradea']")));
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", obj);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(ExpectedConditions.ElementToBeClickable(obj));
                obj.Click();

                // looking for the Search autocomplete field
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("search-text")));
                obj.SendKeys(searchKey);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(ExpectedConditions.TextToBePresentInElementValue(By.Id("search-text"), searchKey));

                // submitting the search
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("search-submit")));
                obj.Click();

                // waiting for the results page
                try
                {
                    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//div[starts-with(@class, 'dontHasPromoted')]")));
                    obj = driver.FindElement(By.XPath(".//*[starts-with(text(),'Am gasit')]"));
                    res = obj.Text;
                }
                catch
                {
                    obj = driver.FindElement(By.XPath(".//*[starts-with(text(),'Nu am gasit anunturi')]"));
                    res = obj.Text;
                }

                driver.Quit();

            } // end using

            Assert.Pass(res);

        } // OlxSimpleSearch



    } // TestClass1



} // namespace
