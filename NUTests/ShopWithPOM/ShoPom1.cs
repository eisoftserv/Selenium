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
    } // 


    public class Stock
    {
        public double Quantity { get; set; }
        public double Value { get; set; }
    } //


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

        public MenuPart(IWebDriver drv)
        {
            this.driver = drv;
        } //


        public bool BringDresses()
        {
            // go to the "Dresses" menu
            var obj = driver.FindElement(By.XPath("//ul[starts-with(@class,'sf-menu')]/li[2]"));
            var todo = new Actions(driver);
            todo.MoveToElement(obj).Click().Perform();
            // waiting until products are loaded (via Ajax)
            Helper.FixedWait(10000);
            return (Helper.FluidWait(10000, driver));

        } // BringDresses


        public bool BringTshirts()
        {
            // hit menu "T-Shirts"
            var obj = driver.FindElement(By.XPath("//div[@id='block_top_menu']/ul/li[3]/a"));
            var todo = new Actions(driver);
            todo.MoveToElement(obj).Click().Perform();
            // waiting until products are loaded (via Ajax)
            Helper.FixedWait(10000);
            return (Helper.FluidWait(10000, driver));

        } // BringTshirts

    } // MenuPart


    public class OfferPart
    { 
        IWebDriver driver;

        IWebElement e_price => driver.FindElement(By.Id("our_price_display"));
        IWebElement e_submit => driver.FindElement(By.Name("Submit"));
        IWebElement e_continue => driver.FindElement(By.XPath("//*[@title='Continue shopping']"));

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
            prod.Price = Helper.Money(e_price.Text);

            return prod;

        } // GetMoreViewInfo


        public Product GetQuickViewInfo(int position)
        {
            Product prod = new Product();
            var objs = e_products;
            var btns = driver.FindElements(By.XPath("//a[@class='quick-view']"));
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

            // "Quick View" is an IFRAME, we need to step into
            obj = driver.FindElement(By.XPath("//iframe[starts-with(@id,'fancybox')]"));
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => {
                if (obj.Displayed && obj.Enabled) return true;
                else return false;
            });
            string frameid = obj.GetAttribute("id");
            driver.SwitchTo().Frame(frameid);

            Helper.FluidWait(10000, driver);

            // get name and price info
            obj = driver.FindElement(By.XPath("//div[starts-with(@class,'pb-center-column')]/h1"));
            prod.Name = obj.Text;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            prod.Price = Helper.Money(e_price.Text);

            // back into the page
            driver.SwitchTo().DefaultContent();
            // close quick view
            obj = driver.FindElement(By.XPath("//a[starts-with(@class,'fancybox-item') and contains(@class,'fancybox-close')]"));
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {
                if (obj.Displayed && obj.Enabled) return true;
                else return false;
            });
            obj.Click();

            return prod;

        } // GetQuickViewInfo


        public double PutProductInCart(int position, int howmany)
        {
            // click first product's MORE button
            var objs = e_products;
            var btns = e_more_buttons;
            int pos = Math.Min(objs.Count - 1, btns.Count - 1);
            pos = Math.Min(pos, position);
            if (pos < 0) return -1;

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

            // get unit price
            double price = Helper.Money(e_price.Text);

            // increase quantity
            var plus = driver.FindElement(By.XPath("//a[contains(@class,'product_quantity_up')]"));
            for (int i=1; i<howmany; i++)
            {
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(d => {
                    if (plus.Displayed && plus.Enabled) return true;
                    else return false;
                });
                plus.Click();
            }

            // hit "add to cart" button
            obj = e_submit;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {
                if (obj.Displayed && obj.Enabled) return true;
                else return false;
            });
            obj.Click();

            // hit "continue shopping" button
            btn = e_continue;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {
                if (btn.Displayed && btn.Enabled) return true;
                else return false;
            });
            btn.Click();

            return price;

        } // PutProductInCart

    } // OfferPart


    public class CartPart
    {
        IWebDriver driver;

        IWebElement e_open => driver.FindElement(By.XPath("//a[@title='View my shopping cart']"));
        //        ReadOnlyCollection<IWebElement> e_products => driver.FindElements());

        public CartPart(IWebDriver drv)
        {
            this.driver = drv;
        } //


        public Stock StepClassicCart(string how)
        {
            // this currently works only for the first product (row) in the table
            Stock prod = new Stock();

            // open classic cart
            var obj = e_open;
            driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", obj);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {
                if (obj.Displayed && obj.Enabled) return true;
                else return false;
            });
            obj.Click();

            Helper.FluidWait(5000, driver);

            // scroll to details
            obj = driver.FindElement(By.XPath("//td[@class='cart_product']"));
            driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", obj);
            // hit "+" or "-"
            string path = ((how == "+") ? "//a[starts-with(@id,'cart_quantity_up')]" : "//a[starts-with(@id,'cart_quantity_down')]");
            obj = driver.FindElement(By.XPath(path));
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {
                if (obj.Displayed && obj.Enabled) return true;
                else return false;
            });
            obj.Click();

            // check cart empty condition when needed
            if (how == ".")
            {
                Helper.FixedWait(5000);
                Helper.FluidWait(5000, driver);
                obj = driver.FindElement(By.XPath("//*[text()='Your shopping cart is empty.']"));
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(d => {
                    if (obj.Displayed) return true;
                    else return false;
                });
                prod = null;
                return prod;
            }

            // wait UI refresh after recalculating values
            string newQuantity = (how == "+") ? "2" : "1";
            obj = driver.FindElement(By.XPath("//input[starts-with(@name,'quantity_') and @type='hidden']"));
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {
                if (obj.GetAttribute("value") == newQuantity) return true;
                else return false;
            });

            // get new quantity
            obj = driver.FindElement(By.XPath("//input[starts-with(@class,'cart_quantity_input')]"));
            prod.Quantity = Helper.Number(obj.GetAttribute("value"));
            // get new total value per product
            obj = driver.FindElement(By.XPath("//*[starts-with(@id,'total_product_price_')]"));
            prod.Value = Helper.Money(obj.Text);

            return prod;

        } // StepClassicCart


        public Stock GetAjaxCart()
        {
            // this currently works only for the first product in the Ajax Cart
            Stock prod = new Stock();

            // open Ajax Cart
            var obj = e_open;
            driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", obj);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {
                if (obj.Displayed && obj.Enabled) return true;
                else return false;
            });

            obj = driver.FindElement(By.ClassName("shopping_cart"));
            var todo = new Actions(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {
                if (obj.Displayed && obj.Enabled) return true;
                else return false;
            });
            todo.MoveToElement(obj);

            // get item count and total per first item
            obj = driver.FindElement(By.ClassName("ajax_cart_quantity"));
            if (obj.Displayed == true)
            {
                prod.Quantity = Helper.Number(obj.Text);
                var objs = driver.FindElements(By.XPath("//*[@class='price']"));
                prod.Value = Helper.Money(objs[8].Text);
            }
            else
            {
                prod = null;
            }

            return prod;

        } // GetAjaxCart


        public void EmptyAjaxCart()
        {
            var obj = e_open;
            driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", obj);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {
                if (obj.Displayed && obj.Enabled) return true;
                else return false;
            });
            var todo = new Actions(driver);
            todo.MoveToElement(obj).Perform();

            Helper.FluidWait(5000, driver);

            // removing the item from the Cart
            obj = driver.FindElement(By.XPath("//a[starts-with(@class,'ajax_cart_block_remove_link')]"));
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {
                if (obj.Displayed && obj.Enabled) return true;
                else return false;
            });
            obj.Click();

            Helper.FluidWait(5000, driver);

            // normally the SPAN with "(empty)" text should become visible
            obj = driver.FindElement(By.XPath("//*[starts-with(@class,'ajax_cart_no_product')]"));
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => {
                if (obj.Displayed) return true;
                else return false;
            });

        } // EmptyAjaxCart

    } // CartPart


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
        [Description("Product Summary VS More Detail View")]
        [Order(31)]
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


        [Test]
        [Description("Product Summary VS Quick Detail View")]
        [Order(32)]
        public void ProductSummary_QuickView()
        {
            var menu = new MenuPart(driver);
            // hitting the menu should return at least one result
            Assert.That(menu.BringDresses(), Is.True);

            var offer = new OfferPart(driver);
            Product summary = offer.GetSummarytInfo(0);
            Product detail = offer.GetQuickViewInfo(0);

            Assert.That(summary != null && detail != null && summary.Name == detail.Name && summary.Price == detail.Price, Is.True);

        } // ProductSummary_QuickView


        [Test]
        [Description("Verify Add button in Classic Cart")]
        [Order(33)]
        public void AddButtonCart()
        {
            var menu = new MenuPart(driver);
            // hitting the menu should return at least one result
            Assert.That(menu.BringTshirts(), Is.True);

            var offer = new OfferPart(driver);
            double price = offer.PutProductInCart(0, 1);
            Assert.That(price >= 0, Is.True);

            var cart = new CartPart(driver);
            Stock classic = cart.StepClassicCart("+");
            Assert.That(classic != null, Is.True);
            Stock ajax = cart.GetAjaxCart();
            Assert.That(ajax != null, Is.True);

            cart.EmptyAjaxCart();

            // check Classic Cart
            Assert.That((2 == classic.Quantity && 2 * price == classic.Value), Is.True);
            //check Ajax Cart
            Assert.That((2 == ajax.Quantity && 2 * price == ajax.Value), Is.True);

        } // ProductSummary_QuickView


        [Test]
        [Description("Verify Subtract button in Cart")]
        [Order(34)]
        public void SubtractButtonCart()
        {
            var menu = new MenuPart(driver);
            // hitting the menu should return at least one result
            Assert.That(menu.BringTshirts(), Is.True);

            var offer = new OfferPart(driver);
            double price = offer.PutProductInCart(0, 2);
            Assert.That(price >= 0, Is.True);

            var cart = new CartPart(driver);
            Stock classic = cart.StepClassicCart("-");
            Assert.That(classic != null, Is.True);
            Stock ajax = cart.GetAjaxCart();
            Assert.That(ajax != null, Is.True);

            cart.EmptyAjaxCart();

            // check Classic Cart
            Assert.That((1 == classic.Quantity && price == classic.Value), Is.True);
            //check Ajax Cart
            Assert.That((1 == ajax.Quantity && price == ajax.Value), Is.True);

        } // SubtractButtonInCart


        [Test]
        [Description("Verify empty Cart by Subtract button")]
        [Order(35)]
        public void FF_EmptyCartBySubtractButton()
        {
            var menu = new MenuPart(driver);
            // hitting the menu should return at least one result
            Assert.That(menu.BringTshirts(), Is.True);

            var offer = new OfferPart(driver);
            double price = offer.PutProductInCart(0, 1);
            Assert.That(price >= 0, Is.True);

            var cart = new CartPart(driver);
            Stock classic = cart.StepClassicCart(".");
            Assert.That(classic == null, Is.True);

            Stock ajax = cart.GetAjaxCart();
            Assert.That(ajax == null, Is.True);

        } // EmptyCartBySubtractButton


    } // ShoPom1


} // ShopWithPOM
