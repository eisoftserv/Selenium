using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading.Tasks;

namespace ShopWithPOM
{
    public class LoginFrag
    {
        private IWebDriver driver;

        public LoginFrag(IWebDriver drv)
        {
            this.driver = drv;
        } //


        public bool DoLogin(string user, string password)
        {
            bool ok = false;

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("login")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementToBeClickable(obj));
            obj.Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("email")));
            obj.Click();
            obj.SendKeys(user + Keys.Tab);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.TextToBePresentInElementValue(By.Id("email"), user));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("passwd")));
            obj.Click();
            obj.SendKeys(password + Keys.Tab);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.TextToBePresentInElementValue(By.Id("passwd"), password));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("SubmitLogin")));
            obj.Click();

            Helper.MyFluidWait(10000, driver);
            if (driver.Title == "My Account - My Store") ok = true;

            return ok;

        } // DoLogin

    } // LoginFrag



    public class DesktopMenuFrag
    {

    } // DesktopMenuFrag
    


    public class AjaxCartFrag
    {

    } // AjaxCartFrag



    public class ClassicCartFrag
    {

    } // ClassicCartFrag



    public class ProductListFrag
    {

    } // ProductListFrag

    [TestFixture]
    [SingleThreaded]
    [Description("POM exercises for automationpractice.com")]
    public partial class ShoPom1
    {
        internal FirefoxDriver driver = null;


        [OneTimeSetUp]
        [Description("logging in")]
        public void LoggingIn()
        {
            string loginUrl = "http://automationpractice.com/index.php?controller=my-account";
            string user = "ellailona2016@gmail.com";
            string password = "";

            // initializing driver
            driver = new FirefoxDriver();
            // starting with default screen area
            driver.Manage().Window.Size = new System.Drawing.Size(1280, 720);
            // navigating to the landing page
            driver.Url = loginUrl;

            var frag = new LoginFrag(driver);
            Assert.That(frag.DoLogin(user, password), Is.True);

        } // LoggingIn


        [OneTimeTearDown]
        [Description("logging out")]
        public void LoggingOut()
        {
            string logoutUrl = "http://automationpractice.com/index.php?mylogout=";
            driver.Navigate().GoToUrl(logoutUrl);

            Assert.That((driver.Title == "Login - My Store"), Is.True);

            driver.Quit();

        } // LoggingOut


        [SetUp]
        [Description("navigating to the landing page")]
        public void GoToLandingPage()
        {
            string startUrl = "http://automationpractice.com/index.php";
            driver.Navigate().GoToUrl(startUrl);

            Assert.That((driver.Title == "My Store"), Is.True);

        } // GoToLandingPage



        [Test]
        [Description("test")]
        [Order(30)]
        public void PassTest()
        {
            Assert.Pass("xyxy");
        } //



    } // ShoPom1


    // miscellaneous helper methods
    public static class Helper
    {
        // the UI thread is waiting the indicated number of milliseconds
        public static void MyFixedWait(int fixedMillisec, int fluidMillisec)
        {
            if (fixedMillisec > 0)
            {
                // waiting on the foreground thread (UI) until timer stops on a threadpool thread
                var tsk = Task.Run(async () =>
                {
                    await Task.Delay(fixedMillisec);
                });
                tsk.Wait();
                tsk.Dispose();
            }

        } // MyFixedWait


        // checking if jQuery Ajax is not running and document is completely loaded
        public static bool MyFluidWait(int fluidMillisec, IWebDriver driver)
        {
            bool ok = true;
            if (fluidMillisec > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(fluidMillisec));
                ok = wait.Until(d => {
                    return (bool)((IJavaScriptExecutor)d).ExecuteScript("return (jQuery.active==0 && document.readyState=='complete');");
                });
            }
            return ok;

        } // MyFluidWait


        // transforms money string to double
        private static double MyMoney(string what)
        {
            double val;
            Double.TryParse(what.Substring(1), out val);
            return val;
        } // MyMoney


        // transforms string to double
        private static double MyNumber(string what)
        {
            double val;
            Double.TryParse(what, out val);
            return val;
        } // MyNumber


    } // Helper


} // ShopWithPOM
