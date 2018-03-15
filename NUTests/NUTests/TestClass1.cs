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
        public void GoogleSearch()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Manage().Window.Maximize();
                driver.Url = "https://www.google.ro";

                var obj = driver.FindElement(By.XPath(".//*[@id='lst-ib']"));
                obj.SendKeys("selenium");
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='tsf']/div[2]/div[3]/center/input")));
                obj.Click();

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//h3/a")));
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
        public void GoogleMail()
        {
            string address = "testuser@gmail.com";
            string pwd = "testpassword";
            string subject = "TestMessage";

            using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Manage().Window.Maximize();
                driver.Url = "https://mail.google.com/";

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("identifier")));
                obj.SendKeys(address);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[text()='Next']")));
                obj.Click();
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("password")));
                obj.SendKeys(pwd);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[text()='Next']")));
                obj.Click();
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[text()='COMPOSE']")));
                obj.Click();
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("to")));
                obj.SendKeys(address);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("subjectbox")));
                obj.SendKeys(subject);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[@role='textbox']")));
                obj.SendKeys("Hello from Selenium!");
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[text()='Send']")));
                obj.Click();
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                bool closed = wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//*[text()='Send']")));
                Assert.That(closed, Is.True);

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[starts-with(@title,'Google Account')]")));
                obj.Click();
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Sign out")));
                obj.Click();
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                driver.Quit();

            } // end using

        } // GoogleMail



        [Test]
        [Description("Searching on OLX")]
        public void OlxBrowsing()
        {
            string res = "Not Found";

            using (var driver = new FirefoxDriver())
            {
                driver.Manage().Window.Maximize();
                var jse = (IJavaScriptExecutor)driver;
                driver.Url = "https://www.olx.ro";

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//a[starts-with(@class,'cookiesBarClose') and contains(@class,'cfff')]")));
                obj.Click();

                // looking for a link to a list of cities
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//a[@title='Harta judetelor']")));
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", obj);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                obj.Click();

                // looking for the link of city Oradea
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//a[@title='Anunturi Oradea']")));
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", obj);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                obj.Click();

                // looking for the Search autocomplete field
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//input[@id='search-text']")));
                jse.ExecuteScript("arguments[0].scrollIntoView(true)", obj);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                obj.SendKeys("ipad" + Keys.Tab);

                // looking for a modal Advert with custom content, and then closing it 
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[@class='highlight-close']")));
                obj.Click();

                // submitting the search
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[@id='search-submit']")));
                obj.Click();

                // waiting for the results page (with promotions or without promotions or no results at all)
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
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



    } // TestClass1



} // namespace
