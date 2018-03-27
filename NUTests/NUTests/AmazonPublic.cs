using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUTests
{
    [TestFixture]
    [SingleThreaded]
    [Description("using controls on AWS as guest user")]
    class AmazonPublic
    {
        internal FirefoxDriver driver = null;
        internal IJavaScriptExecutor jse = null;



        [OneTimeSetUp]
        [Description("configuring driver context")]
        public void FF_DriverSetup()
        {
            // initializing driver and JavaScript executor
            driver = new FirefoxDriver();
            jse = (IJavaScriptExecutor)driver;
            // setting one of the standard resolutions for desktops
            driver.Manage().Window.Size = new System.Drawing.Size(1200, 720);

        } // DriverSetup



        [OneTimeTearDown]
        [Description("logging out")]
        public void FF_Quit()
        {
            driver.Quit();

        } // Quit



        [SetUp]
        [Description("Navigating to the landing page and opening flyout menu")]
        public void GoToLandingPage()
        {
            string startUrl = "https://aws.amazon.com";

            driver.Navigate().GoToUrl(startUrl);
            var todo = new Actions(driver);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("aws-nav-flyout-trigger")));
            todo.MoveToElement(obj).Perform();

        } //



        [Test]
        [Description("go to S3 page, use advert video")]
        public void FF_AWS_Video()
        {
            // going to Products > Storage > S3
            var todo = new Actions(driver);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[@id='aws-nav-flyout-1-root']/div/div[3]/a")));
            todo.MoveToElement(obj).Click().Perform();
            todo = new Actions(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[@id='aws-nav-flyout-2-products']/div/div[2]/a")));
            todo.MoveToElement(obj).Perform();
            todo = new Actions(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[@id='aws-nav-flyout-3-storage']/div/div/a")));
            todo.MoveToElement(obj).Click().Perform();

            // expecting a page with specific "Amazon S3" link
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementExists(By.Id("Get_started_with_Amazon_S3")));
            obj = driver.FindElement(By.XPath(".//div[starts-with(@class,'lb-tiny-iblock')]"));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);

            // start video
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("vjs-big-play-button")));
            obj.Click();

            // wait 5 seconds while the video is playing
            SimpleWait(5.0);

            // go to video control bar
            todo = new Actions(driver);
            obj = driver.FindElement(By.XPath(".//div[starts-with(@class,'vjs-control-bar')]"));
            todo.MoveToElement(obj).Perform();

            // go to the volume control icon
            todo = new Actions(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            obj = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//div[starts-with(@class,'vjs-volume-panel')]")));
            todo.MoveToElement(obj).Perform();

            // go to the volume control bar and hit at 40% of its width
            todo = new Actions(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            obj = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//div[starts-with(@class,'vjs-volume-bar')]")));
            int offsetX = (int)(obj.Size.Width * 0.1);
            // volume is set to +50% -10%, so to 40%,
            // because the "Origo" of the current object is its middle zone
            todo.MoveToElement(obj).MoveByOffset(-offsetX, 0).Click().Perform();

            // wait 8 seconds while the video is playing
            SimpleWait(8.0);

            // go to video control bar
            todo = new Actions(driver);
            obj = driver.FindElement(By.XPath(".//div[starts-with(@class,'vjs-control-bar')]"));
            todo.MoveToElement(obj).Perform();

            // pause video
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[@title='Pause']")));
            obj.Click();

            // check if it's paused
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[@title='Play']")));

        } // Menu_Video



        internal void SimpleWait(double nSeconds)
        {
            // emulating timer by waiting for inexistent DOM node
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(nSeconds));
            try
            {
                wait.Until(ExpectedConditions.ElementExists(By.Id("tytyty")));
            }
            catch { }

        } // SimpleWait



    } // class AmazonPublic



} // namespace
