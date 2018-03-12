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
    public class TestClass1
    {

        [Test]
        [Description("Empty Test")]
        public void EmptyTest()
        {
            Assert.Pass("Empty test done");
        } //



        [Test]
        public void GoogleSearch()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Url = "https://www.google.ro";
                var obj = driver.FindElement(By.XPath(".//*[@id='lst-ib']"));
                obj.SendKeys("selenium");

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
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
        public void GoogleMail()
        {
            //--------------------------------------------------
            string address = "test.user@gmail.com";
            string pwd = "test.password";
            string subject = "TestMessage";

            using (IWebDriver driver = new FirefoxDriver())
            {
                string url = "https://mail.google.com/";
                driver.Url = url;
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("identifier")));
                obj.SendKeys(address);

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



} // namespace
