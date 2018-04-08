using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading.Tasks;

namespace NUTests
{
    [TestFixture]
    [SingleThreaded]
    [Description("exercises for automationpractice.com")]
    public partial class Shopping1
    {
        internal FirefoxDriver driver = null;
        internal IJavaScriptExecutor jse = null;



        [OneTimeSetUp]
        [Description("logging in")]
        public void FF_Login()
        {
            string loginUrl = "http://automationpractice.com/";
            string user = "ellailona2016@gmail.com";
            string password = "";

            // initializing driver and JavaScript executor
            driver = new FirefoxDriver();
            jse = (IJavaScriptExecutor)driver;
            // starting with default screen area
            driver.Manage().Window.Size = new System.Drawing.Size(1280, 720);

            // navigating to the landing page
            driver.Url = loginUrl;

            // "sign in" workflow
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("login")));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
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

            // normally the "sign out" button should be present now
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("logout")));

        } // LoggingIn



        [OneTimeTearDown]
        [Description("logging out")]
        public void FF_Logout()
        {
            string logout = "http://automationpractice.com/index.php?mylogout=";
            driver.Navigate().GoToUrl(logout);

            // normally the "sign in" button should be present now
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var obj = wait.Until(ExpectedConditions.ElementExists(By.ClassName("login")));

            driver.Quit();

        } // LoggingOut



        [SetUp]
        [Description("navigating to the landing page")]
        public void FF_GoToLandingPage()
        {
            string startUrl = "http://automationpractice.com/index.php";
            driver.Navigate().GoToUrl(startUrl);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("search_query_top")));

        } // GoToLandingPage



        [Test]
        [Description("ipad-alike UI")]
        [Order(12)]
        public void FF_Choose1024x768()
        {
            // select tablet resolution
            driver.Manage().Window.Size = new System.Drawing.Size(1024, 768);

            // set focus to the "Women" menu item
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Women")));
            jse.ExecuteScript("arguments[0].focus()", obj);
            // hit the "Tops" category
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Tops")));
            obj.Click();
            // wait for Ajax process
            StaticWait(10000);
            JustWait(10000);

            // hit the "blue" square (I want a blue t-shirt)
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.Id("color_2")));
            jse.ExecuteScript("arguments[0].focus()", obj);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementToBeClickable(obj));
            obj.Click();

            // set details and add item to cart
            SetDetailsAndAddToCart();
            // check number of items in Cart
            Assert.That(CheckCart(), Is.EqualTo("1"));
            // remove selected item from cart
            EmptyAjaxCart();

        } // Choose1024x768



        internal void SetDetailsAndAddToCart()
        {
            // select a size
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var obj = wait.Until(ExpectedConditions.ElementExists(By.Id("group_1")));
            var dd = new SelectElement(obj);
            dd.SelectByText("L");

            // hit the "Add to Cart" button
            obj = driver.FindElement(By.Name("Submit"));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementToBeClickable(obj));
            obj.Click();

            // close the light-box by hitting "Continue shopping"
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[@title='Continue shopping']")));
            obj.Click();

        } // DetailsAndAddTocart



        internal string CheckCart()
        {
            // get number of items in Cart
            var obj = driver.FindElement(By.XPath(".//*[starts-with(@class,'ajax_cart_quantity')]"));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//*[starts-with(@class,'ajax_cart_quantity')]")));
            return (obj.Text);

        } // CheckCart



        internal void EmptyAjaxCart()
        {
            // displaying Ajax Cart - a "mouseover" event is expected here
            var todo = new Actions(driver);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var obj = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//a[starts-with(@title,'View my shopping cart')]")));
            todo.MoveToElement(obj).Perform();

            // removing the item from the Cart
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//a[starts-with(@class,'ajax_cart_block_remove_link')]")));
            obj.Click();

            // normally the SPAN with "(empty)" text should become visible
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            obj = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//*[@class='ajax_cart_no_product']")));

        } // EmptyAjaxCart()



        public bool JustWait(int millisec)
        {
            // check if jQuery Ajax is not running and document is completely loaded
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(millisec));
            bool ok = wait.Until( driver => {
                return (bool)jse.ExecuteScript("return (jQuery.active==0 && document.readyState=='complete');");
            });

            return ok;

        } // JustWait



        internal void StaticWait(int millisec)
        {
            // waiting on the foreground thread (UI) until timer stops on a threadpool thread
            // this is useful solely to wait for a job running on background threads (Ajax or WebSockets)
            var tsk = Task.Run(async () =>
            {
                await Task.Delay(millisec);
            });
            tsk.Wait();
            tsk.Dispose();

        } //



        [Test]
        [Description("desktop-alike UI")]
        [Order(13)]
        public void FF_Choose1280x720()
        {
            driver.Manage().Window.Size = new System.Drawing.Size(1280, 720);

            // hit the "Dresses" menu
            var todo = new Actions(driver);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var obj = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//ul[starts-with(@class,'sf-menu')]/li[2]")));
            todo.MoveToElement(obj).Perform();
            obj.Click();
            // wait for Ajax process
            StaticWait(10000);
            JustWait(10000);

            // select "In Stock" option for result list
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            obj = wait.Until(ExpectedConditions.ElementExists(By.Id("selectProductSort")));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            var dd = new SelectElement(obj);
            dd.SelectByText("In stock");
            // wait for Ajax and/or UI refresh
            Assert.That(JustWait(10000), Is.True);

            // "Price" slider - getting the position of the left handle
            int nLeft, nRight, nOffset;
            obj = driver.FindElement(By.XPath(".//div[@id='layered_price_slider']/a[1]"));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementToBeClickable(obj));
            nLeft = obj.Location.X;

            // getting the position of the right handle
            obj = driver.FindElement(By.XPath(".//div[@id='layered_price_slider']/a[2]"));
            nRight = obj.Location.X;

            // dragging the right handle to 75%
            nOffset = (int)((nRight - nLeft) * 0.25);
            var oLoad = driver.FindElement(By.Id("layered_ajax_loader"));
            todo = new Actions(driver);
            todo.DragAndDropToOffset(obj, -nOffset, 0).Perform();

            // waiting until the new item list gets loaded
            Assert.That(JustWait(10000), Is.True);

            // dragging the left handle to 30%
            nOffset = (int)((nRight - nLeft) * 0.3);
            oLoad = driver.FindElement(By.Id("layered_ajax_loader"));
            obj = driver.FindElement(By.XPath(".//div[@id='layered_price_slider']/a[1]"));
            todo = new Actions(driver);
            todo.DragAndDropToOffset(obj, nOffset, 0).Perform();

            // waiting until the new item list gets loaded
            Assert.That(JustWait(10000), Is.True);

            // position on an item (it should display a gadget)
            todo = new Actions(driver);
            obj = driver.FindElement(By.XPath(".//div[@class='product-container']"));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(7));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[@class='product-container']")));
            todo.MoveToElement(obj).Perform();

            // hit the "More" button displayed on the gadget
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//a[contains(@class,'lnk_view')]")));
            obj.Click();

            // set details and add item to cart
            SetDetailsAndAddToCart();
            // check number of items in Cart
            Assert.That(CheckCart(), Is.EqualTo("1"));
            // remove selected item from cart
            EmptyAjaxCart();

        } // Choose1280x720



        [Test]
        [Description("smartphone-alike UI")]
        [Order(11)]
        public void FF_Choose480x800()
        {
            driver.Manage().Window.Size = new System.Drawing.Size(480, 800);

            // using the acordion widget to open Categories > Women > Catalog
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[starts-with(@class,'cat-title')]")));
            obj.Click();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//a[@title='Women' and text()='Women']")));
            obj.Click();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("layered_block_left")));
            obj.Click();

            Assert.That(JustWait(10000), Is.True);

            // checking Category "Tops"
            obj = driver.FindElement(By.Id("layered_category_4"));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            obj.Click();
            // wait for Ajax process
            Assert.That(JustWait(10000), Is.True);

            // checking size "L"
            obj = driver.FindElement(By.Id("layered_id_attribute_group_3"));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            obj.Click();
            // wait for Ajax process
            Assert.That(JustWait(10000), Is.True);

            // add to cart the first item
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//a[contains(@class,'ajax_add_to_cart_button')]")));
            obj.Click();

            // close the light-box by hitting "Continue Shopping"
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[@title='Continue shopping']")));
            obj.Click();
            // wait for Ajax process
            Assert.That(JustWait(10000), Is.True);

            // check number of items in Cart
            Assert.That(CheckCart(), Is.EqualTo("1"));
            // remove selected item from cart
            EmptyAjaxCart();

        } // Choose480x800



    } // class



} // namespace