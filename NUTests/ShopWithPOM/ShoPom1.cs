using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading.Tasks;

namespace ShopWithPOM
{
    public class LoginPart
    {
        IWebDriver driver;
        IWebElement e_user => driver.FindElement(By.Id("email"));
        IWebElement e_password => driver.FindElement(By.Id("passwd"));
        IWebElement e_submit => driver.FindElement(By.Id("SubmitLogin"));

        public LoginPart(IWebDriver drv)
        {
            this.driver = drv;
        } //


        public bool Login(string user, string password)
        {
            bool ok = false;

            var obj = e_user;
            driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", obj);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {
                if (obj.Displayed && obj.Enabled) return true;
                else return false;
            });
            obj.Click();
            obj.SendKeys(user + Keys.Tab);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {
                if (obj.GetAttribute("value") == user) return true;
                else return false;
            });

            obj = e_password;
            obj.Click();
            obj.SendKeys(password + Keys.Tab);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {
                if (obj.GetAttribute("value") == password) return true;
                else return false;
            });

            obj = e_submit;
            obj.Click();

            Helper.FluidWait(10000, driver);
            if (driver.Title == "My account - My Store") ok = true;

            return ok;

        } // Login

    } // LoginPart



    public class DesktopMenuPart
    {

    } // DesktopMenuPart
    


    public class AjaxCartPart
    {

    } // AjaxCartPart



    public class ClassicCartPart
    {

    } // ClassicCartPart



    public class ProductListPart
    {

    } // ProductListPart

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
            string password = "maricosan";

            // initializing driver
            driver = new FirefoxDriver();
            driver.Manage().Window.Size = new System.Drawing.Size(1280, 720);

            // navigating to the login page
            driver.Url = loginUrl;
            bool ok = Helper.FluidWait(10000, driver);
            Assert.That(ok && (driver.Title == "Login - My Store"), Is.True);

            // logging in
            var part = new LoginPart(driver);
            Assert.That(part.Login(user, password), Is.True);

        } // LoggingIn


        [OneTimeTearDown]
        [Description("logging out")]
        public void LoggingOut()
        {
            string logoutUrl = "http://automationpractice.com/index.php?mylogout=";
            driver.Navigate().GoToUrl(logoutUrl);
            bool ok = Helper.FluidWait(10000, driver);
            driver.Quit();

            Assert.That(ok && (driver.Title == "Login - My Store"), Is.True);

        } // LoggingOut


        [SetUp]
        [Description("navigating to the landing page")]
        public void GoToLandingPage()
        {
            string startUrl = "http://automationpractice.com/index.php";
            driver.Navigate().GoToUrl(startUrl);
            bool ok = Helper.FluidWait(10000, driver);
            Assert.That(ok && (driver.Title == "My Store"), Is.True);

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
        public static bool FluidWait(int fluidMillisec, IWebDriver driver)
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
        private static double Money(string what)
        {
            double val;
            Double.TryParse(what.Substring(1), out val);
            return val;
        } // MyMoney


        // transforms string to double
        private static double Number(string what)
        {
            double val;
            Double.TryParse(what, out val);
            return val;
        } // MyNumber


    } // Helper


} // ShopWithPOM
