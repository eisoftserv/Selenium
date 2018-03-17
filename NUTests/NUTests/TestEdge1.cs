using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUTests
{

    [TestFixture]
    [Description("same code as in TestClass1.cs - just testing with EdgeDriver")]
    public class TestEdge1
    {


        [Test]
        [Description("doing a Google search")]
        public void Edge_GoogleSearch()
        {
            using (IWebDriver driver = new EdgeDriver())
            {
                driver.Manage().Window.Maximize();
                driver.Url = "https://www.google.ro";

                var obj = driver.FindElement(By.Id("lst-ib"));
                obj.SendKeys("selenium" + Keys.Tab);

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
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
        [Description("sending an email message to myself")]
        public void Edge_GoogleMail()
        {
            string address = "ellailona2016@gmail.com";
            string pwd = "fulonlogo";
            string subject = "TestMessage";
            string helloText = "Hello from Selenium!";

            using (var driver = new EdgeDriver())
            {
                driver.Manage().Window.Maximize();
                driver.Url = "https://mail.google.com/";

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("identifier")));
                obj.SendKeys(address);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(ExpectedConditions.TextToBePresentInElementValue(By.Name("identifier"), address));

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[text()='Next']")));
                obj.Click();

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("password")));
                obj.SendKeys(pwd);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(ExpectedConditions.TextToBePresentInElementValue(By.Name("password"), pwd));

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[text()='Next']")));
                obj.Click();

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[text()='COMPOSE']")));
                obj.Click();

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("to")));
                obj.SendKeys(address);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(ExpectedConditions.TextToBePresentInElementValue(By.Name("to"), address));

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("subjectbox")));
                obj.SendKeys(subject);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(ExpectedConditions.TextToBePresentInElementValue(By.Name("subjectbox"), subject));

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[@role='textbox']")));
                obj.SendKeys(helloText);

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[text()='Send']")));
                obj.Click();

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                bool closed = wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//*[text()='Send']")));
                Assert.That(closed, Is.True);

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[starts-with(@title,'Google Account')]")));
                obj.Click();

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Sign out")));
                obj.Click();

                // normally the page for login password is displayed
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                obj = wait.Until(ExpectedConditions.ElementExists(By.Name("password")));

                driver.Quit();

            } // end using

        } // GoogleMail



        [Test]
        [Description("Searching on OLX")]
        public void Edge_OlxBrowsing()
        {
            string res = "Not Found";

            using (var driver = new EdgeDriver())
            {
                driver.Manage().Window.Maximize();
                var jse = (IJavaScriptExecutor)driver;
                string searchKey = "ipad";

                driver.Url = "https://www.olx.ro";

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//a[starts-with(@class,'cookiesBarClose') and contains(@class,'cfff')]")));
                obj.Click();

                // looking for a link to a list of cities
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//a[@title='Harta judetelor']")));
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", obj);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//a[@title='Harta judetelor']")));
                obj.Click();

                // looking for the link of city Oradea
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//a[@title='Anunturi Oradea']")));
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", obj);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//a[@title='Anunturi Oradea']")));
                obj.Click();

                // looking for the Search autocomplete field
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("search-text")));
                obj.SendKeys(searchKey + Keys.Tab);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(ExpectedConditions.TextToBePresentInElementValue(By.Id("search-text"), searchKey));

                // submitting the search
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("search-submit")));
                obj.Click();

                // waiting for the results page (with promotions or without promotions or no results at all)
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//div[starts-with(@class,'section') or starts-with(@class,'dontHasPromoted') or starts-with(@class,'emptynew')]")));

                try
                {
                    obj = driver.FindElement(By.XPath(".//div[starts-with(@class,'emptynew')]"));
                }
                catch
                {
                    obj = driver.FindElement(By.XPath(".//*[starts-with(text(),'Am gasit')]"));
                    res = obj.Text;
                }

                driver.Quit();

            } // end using

            Assert.Pass(res);

        } // OlxBrowsing



    } // TestEdge1



} // namespace
