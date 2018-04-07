using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ShopWithPOM
{
    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
    } // Product


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
    

    public class MenuPart
    {
        IWebDriver driver;
        IWebElement e_dresses => driver.FindElement(By.XPath("//ul[starts-with(@class,'sf-menu')]/li[2]"));

        public MenuPart(IWebDriver drv)
        {
            this.driver = drv;
        } //


        public bool BringDresses()
        {
            // go to the "Dresses" menu
            var todo = new Actions(driver);
            todo.MoveToElement(e_dresses).Click().Perform();
            // waiting until products are loaded (via Ajax)
            Helper.FixedWait(10000);
            return (Helper.FluidWait(10000, driver));

        } // BringDresses

    } // MenuPart


    public class OfferPart
    { 
        IWebDriver driver;
        ReadOnlyCollection<IWebElement> e_products => driver.FindElements(By.XPath("//div[@class='product-container']"));
        ReadOnlyCollection<IWebElement> e_more_buttons => driver.FindElements(By.XPath("//a[contains(@class,'lnk_view')]"));

        public OfferPart(IWebDriver drv)
        {
            this.driver = drv;
        } //


        public Product GetSummarytInfo(int position)
        {
            Product prod = new Product();
            // get the name and price of the item
            var names = driver.FindElements(By.XPath("//div[starts-with(@class,'right-block')]/h5/a"));
            var prices = driver.FindElements(By.XPath("//div[starts-with(@class,'right-block')]/div/span"));
            int pos = Math.Min(names.Count-1, prices.Count-1);
            pos = Math.Min(pos, position);
            if (pos < 0) return null;

            prod.Name = names[pos].Text;
            prod.Price = Helper.Money(prices[pos].Text);

            return prod;

        } // GetSummaryInfo


        public Product GetMoreViewInfo(int position)
        {
            Product prod = new Product();
            var objs = e_products;
            var btns = e_more_buttons;
            int pos = Math.Min(objs.Count - 1, btns.Count - 1);
            pos = Math.Min(pos, position);
            if (pos < 0) return null;

            var obj = objs[pos];
            driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", obj);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {
                if (obj.Displayed && obj.Enabled) return true;
                else return false;
            });
            var todo = new Actions(driver);
            todo.MoveToElement(obj).Perform();

            var btn = btns[pos];
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {
                if (btn.Displayed && btn.Enabled) return true;
                else return false;
            });
            btn.Click();

            Helper.FluidWait(10000, driver);

            obj = driver.FindElement(By.XPath("//div[starts-with(@class,'pb-center-column')]/h1"));
            prod.Name = obj.Text;
            obj = driver.FindElement(By.Id("our_price_display"));
            prod.Price = Helper.Money(obj.Text);

            return prod;

        } // GetMoreViewInfo


    } // OfferPart


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
            string password = "";

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
        [Description("Ticket #6 Product Summary View VS More Detail View")]
        [Order(30)]
        public void ProductSummary_MoreView()
        {
            var menu = new MenuPart(driver);
            // hitting the menu should return at least one result
            Assert.That(menu.BringDresses(), Is.True);

            var offer = new OfferPart(driver);
            Product summary = offer.GetSummarytInfo(0);
            Product detail = offer.GetMoreViewInfo(0);

            Assert.That(summary!=null && detail!=null && summary.Name==detail.Name && summary.Price==detail.Price, Is.True);
            
        } // ProductSummary_MoreView



    } // ShoPom1


    // miscellaneous helper methods
    public static class Helper
    {
        // the UI thread is waiting the indicated number of milliseconds
        public static void FixedWait(int fixedMillisec)
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

        } // FixedWait


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
        public static double Money(string what)
        {
            double val;
            Double.TryParse(what.Substring(1), out val);
            return val;
        } // MyMoney


        // transforms string to double
        public static double Number(string what)
        {
            double val;
            Double.TryParse(what, out val);
            return val;
        } // MyNumber


    } // Helper


} // ShopWithPOM
