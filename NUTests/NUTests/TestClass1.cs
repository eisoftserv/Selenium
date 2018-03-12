using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUTests
{
    [TestFixture]
    [Description("simple tests")]
    public class TestClass1
    {

        [Test]
        [Description("Empty Test")]
        public void EmptyTest()
        {
            Assert.Pass("Empty test done");
        } //



        [Test]
        [Description("doing a Google search")]
        public void GoogleSearch()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
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

                // checking the URL
                bool urlOk = driver.Url.StartsWith("https://www.seleniumhq.org/");
                Assert.That(urlOk, Is.True);

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
                string url = "https://mail.google.com/";
                driver.Url = url;
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



    } // TestClass1



    [TestFixture]
    [SingleThreaded]
    [Description("exercises for phptravels.net")]
    public class TestClass2
    {
        internal IWebDriver driver = null;



        [OneTimeSetUp]
        [Description("logging in as demo user")]
        public void LoggingIn()
        {
            string loginUrl = "https://www.phptravels.net/login";
            string user = "user@phptravels.com";
            string password = "demouser";

            driver = new FirefoxDriver();
            driver.Url = loginUrl;

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("username")));
            obj.SendKeys(user);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("password")));
            obj.SendKeys(password);
            obj.Submit();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            // we are looking for the NAV bar, in order to check that we are logged in
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//nav[@id='offcanvas-menu']")));

        } // LoggingIn



        [OneTimeTearDown]
        [Description("logging out as demo user")]
        public void LoggingOut()
        {
            string logoutUrl = "https://www.phptravels.net/account/logout/";
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

            // handling JavaScript Modal dialog - if any
            driver.Navigate().GoToUrl(logoutUrl);
            try
            {
                driver.SwitchTo().Alert().Accept();
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
            }
            catch
            { }

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            // normally we are redirected to the Login page
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var obj = wait.Until(ExpectedConditions.ElementExists(By.Name("username")));

            driver.Quit();

        } // LoggingOut



        [SetUp]
        [Description("Navigating to the start page")]
        public void GoToStartPage()
        {
            string startUrl = "https://www.phptravels.net/";

            driver.Navigate().GoToUrl(startUrl);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

        } // GoToStartPage



        [Test]
        [Order(1)]
        [Description("booking a tour of Dubai")]
        public void BookingTour()
        {
            var jse = (IJavaScriptExecutor)driver;
            
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//a[starts-with(@href,'#TOURS')]")));
            obj.Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            //--------------- tour

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[@id='s2id_autogen12']/a")));
            obj.Click();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[contains(@class,'select2-focused')]")));
            obj.SendKeys("Dubai");
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//li[starts-with(@class,'select2-results-dept-1')]")));
            obj.Click();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            //------------------ appointment

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(7));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//input[@name='date']")));
            obj.Click();
            obj.Clear();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

            DateTime ddate = DateTime.Now.AddDays(DateTime.Now.Minute);
            string sdate = ddate.Day + "/" + ddate.Month + "/" + ddate.Year;
            obj.SendKeys(sdate + Keys.Tab);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            //----------------- guests

            var objs = driver.FindElements(By.XPath(".//select[@name='adults']"));
            // unfortunately we have 2 select nodes with the same id and name attributes
            var dd = new SelectElement(objs[1]);
            dd.SelectByText("1");
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
            objs[1].Submit();

            //---------------------- reviewing selection

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//button[text()='Book Now']")));
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", obj);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[text()='Book Now']")));
            obj.Click();

            //---------------------- confirmation

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//button[text()='CONFIRM THIS BOOKING']")));
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", obj);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[text()='CONFIRM THIS BOOKING']")));
            obj.Click();

            //----------------------- payment options

            //wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            //wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            //obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//button[text()='Pay on Arrival']")));
            //jse.ExecuteScript("arguments[0].scrollIntoView(true)", obj);

            //wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            //wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            //wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            //obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[contains(text()='Pay on Arrival']")));
            //obj.Click();
            //wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
            //// handling JavaScript modal dialog
            //try
            //{
            //    driver.SwitchTo().Alert().Accept();
            //    wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
            //}
            //catch
            //{ }

            //wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            //wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            //obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[starts-with(text(),'You must confirm your booking')]")));

            Assert.Pass("Tour test done");

        } // BookingTour



    } // TestClass2



} // namespace
