using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NUTests
{
    [TestFixture]
    [SingleThreaded]
    [Description("exercises for automationpractice.com")]
    public class Shopping1
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
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("login")));
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

            // hit the "blue" square (I want a blue t-shirt)
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.Id("color_2")));
            jse.ExecuteScript("arguments[0].focus()", obj);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("color_2")));
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
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("Submit")));
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



        public void JustWait(int millisec)
        {
            var tsk = Task.Run(async () => {
                await Task.Delay(millisec);
            });
            tsk.Wait();
            tsk.Dispose();

        } // JustWait



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

            // select "In Stock" option for result list
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            obj = wait.Until(ExpectedConditions.ElementExists(By.Id("selectProductSort")));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            var dd = new SelectElement(obj);
            dd.SelectByText("In stock");

            // "Price" slider - getting the position of the left handle
            int nLeft, nRight, nOffset;
            obj = driver.FindElement(By.XPath(".//div[@id='layered_price_slider']/a[1]"));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[@id='layered_price_slider']/a[1]")));
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
            JustWait(10000);

            // dragging the left handle to 30%
            nOffset = (int)((nRight - nLeft) * 0.3);
            oLoad = driver.FindElement(By.Id("layered_ajax_loader"));
            obj = driver.FindElement(By.XPath(".//div[@id='layered_price_slider']/a[1]"));
            todo = new Actions(driver);
            todo.DragAndDropToOffset(obj, nOffset, 0).Perform();
            // waiting until the new item list gets loaded
            JustWait(10000);

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
            JustWait(2000);

            // checking Category "Tops"
            obj = driver.FindElement(By.Id("layered_category_4"));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            obj.Click();
            JustWait(5000);

            // checking size "L"
            obj = driver.FindElement(By.Id("layered_id_attribute_group_3"));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            obj.Click();
            JustWait(5000);

            // add to cart the first item
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//a[contains(@class,'ajax_add_to_cart_button')]")));
            obj.Click();

            // close the light-box by hitting "Continue Shopping"
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[@title='Continue shopping']")));
            obj.Click();
            JustWait(2000);

            // check number of items in Cart
            Assert.That(CheckCart(), Is.EqualTo("1"));
            // remove selected item from cart
            EmptyAjaxCart();

        } // Choose480x800



        [Test]
        [Description("Verify product name and price in More detail view")]
        [Order(21)]
        public void FF_VerifyMoreNamePrice()
        {
            // go to the "Dresses" menu
            var todo = new Actions(driver);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var obj = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//ul[starts-with(@class,'sf-menu')]/li[2]")));
            todo.MoveToElement(obj).Click().Perform();
            // waiting until products are loaded
            JustWait(5000);

            obj = driver.FindElement(By.XPath(".//div[@class='product-container']"));
            todo = new Actions(driver);
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);

            // get the name and price of the item
            obj = driver.FindElement(By.XPath(".//div[starts-with(@class,'right-block')]/h5/a"));
            string name1 = obj.Text;
            obj = driver.FindElement(By.XPath(".//div[starts-with(@class,'right-block')]/div/span"));
            string price1 = obj.Text;

            // go to item and hit the "More" button
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[@class='product-container']")));
            todo.MoveToElement(obj).Perform();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//a[contains(@class,'lnk_view')]")));
            obj.Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[starts-with(@class,'pb-center-column')]/h1")));
            string name2 = obj.Text;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("our_price_display")));
            string price2 = obj.Text;

            bool ok = ((name1 == name2) && (price1 == price2));
            Assert.That(ok, Is.True);

        } // VerifyMoreNamePrice



        [Test]
        [Description("Verify product name and price in Quick detail view")]
        [Order(22)]
        public void FF_VerifyQuickNamePrice()
        {
            // go to the "Dresses" menu
            var todo = new Actions(driver);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var obj = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//ul[starts-with(@class,'sf-menu')]/li[2]")));
            todo.MoveToElement(obj).Click().Perform();
            // waiting until products are loaded
            JustWait(5000);

            obj = driver.FindElement(By.XPath(".//div[@class='product-container']"));
            todo = new Actions(driver);
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);

            // get the name and price of the item
            obj = driver.FindElement(By.XPath(".//div[starts-with(@class,'right-block')]/h5/a"));
            string name1 = obj.Text;
            obj = driver.FindElement(By.XPath(".//div[starts-with(@class,'right-block')]/div/span"));
            string price1 = obj.Text;

            // go to item and hit the "Quick View" link
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[@class='product-container']")));
            todo.MoveToElement(obj).Perform();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//a[@class='quick-view']")));
            obj.Click();

            // "Quick View" is an IFRAME, we need to step into
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//iframe[starts-with(@id,'fancybox')]")));
            string frameid = obj.GetAttribute("id");
            driver.SwitchTo().Frame(frameid);

            // get name and price info
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[starts-with(@class,'pb-center-column')]/h1")));
            string name2 = obj.Text;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("our_price_display")));
            string price2 = obj.Text;

            // back into the page
            driver.SwitchTo().DefaultContent();
            // close quick view
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//a[starts-with(@class,'fancybox-item') and contains(@class,'fancybox-close')]")));

            bool ok = ((name1 == name2) && (price1 == price2));
            Assert.That(ok, Is.True);

        } // VerifyQuickNamePrice



        [Test]
        [Description("Verify Add button in Cart")]
        [Order(23)]
        public void FF_AddButtonCart()
        {
            // add a T-Shirt to Cart
            double price = PutProductInCart(false);

            // hit "+" in Classic Cart, bring new quantity and value
            double[] resultCC = StepClassicCart("+");

            // bring Ajax Cart quantity and value
            double[] resultAC = GetAjaxCart();

            // empty the Cart
            EmptyAjaxCart();

            // check Classic Cart
            Assert.That((2 == resultCC[0]), Is.True);
            Assert.That((2 * price == resultCC[1]), Is.True);

            //check Ajax Cart
            Assert.That((2 == resultAC[0]), Is.True);
            Assert.That((2 * price == resultAC[1]), Is.True);

        } // AddButtonInCart



        internal double[] GetAjaxCart()
        {
            double[] numbers = { 0.0, 0.0 };
            // open Ajax Cart
            var obj = driver.FindElement(By.XPath(".//a[@title='View my shopping cart']"));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//a[@title='View my shopping cart']")));
            var todo = new Actions(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
             obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("shopping_cart")));
            todo.MoveToElement(obj);

            // get item count and total per first item
            obj = driver.FindElement(By.ClassName("ajax_cart_quantity"));
            if (obj.Displayed == true)
            {
                numbers[0] = MyNumber(obj.Text);
                var objs = driver.FindElements(By.XPath(".//*[@class='price']"));
                numbers[1] = MyMoney(objs[8].Text);
            }

            return numbers;

        } // GetAjaxCartFirst



        internal double PutProductInCart(bool twoProducts)
        {
            // hit menu "T-Shirts"
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[@id='block_top_menu']/ul/li[3]/a")));
            obj.Click();
            // wait until products are listed
            JustWait(8000);

            // click first product's MORE button
            obj = driver.FindElement(By.XPath(".//div[@class='product-container']"));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            var todo = new Actions(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[@class='product-container']")));
            todo.MoveToElement(obj).Perform();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//a[contains(@class,'lnk_view')]")));
            obj.Click();

            // get unit price
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("our_price_display")));
            double price = MyMoney(obj.Text);
          
            // increase quantity
            if (twoProducts == true)
            {
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//a[contains(@class,'product_quantity_up')]")));
                obj.Click();
            }

            // hit "add to cart" button
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("Submit")));
            obj.Click();

            // hit "continue shopping" button
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[@title='Continue shopping']")));
            obj.Click();

            return price;

        } // PutProductInCart



        internal double[] StepClassicCart(string upwards)
        {
            double[] numbers = { 0.0, 0.0 };
            // open classic cart
            var obj = driver.FindElement(By.XPath(".//a[@title='View my shopping cart']"));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//a[@title='View my shopping cart']")));
            obj.Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//td[@class='cart_product']")));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);

            string path = ((upwards =="+") ? ".//a[starts-with(@id,'cart_quantity_up')]" : ".//a[starts-with(@id,'cart_quantity_down')]");
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(path)));
            obj.Click();

            // check cart empty condition when needed
            if (upwards == ".")
            {
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                obj = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//*[contains(@class,'alert-warning')]")));
                return numbers;
            }
            
            string newQuantity = (upwards == "+") ? "2" : "1";
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.TextToBePresentInElementValue(By.XPath(".//input[starts-with(@name,'quantity_') and @type='hidden']"), newQuantity));

            // get quantity
            obj = driver.FindElement(By.XPath(".//input[starts-with(@class,'cart_quantity_input')]"));
            numbers[0] = MyNumber(obj.GetAttribute("value"));

            // get total value per product
            obj = driver.FindElement(By.XPath(".//*[starts-with(@id,'total_product_price_')]"));
            numbers[1] = MyMoney(obj.Text);

            return numbers;

        } // HitClassicCartButton



        private double MyMoney(string what)
        {
            double val;
            Double.TryParse(what.Substring(1), out val);
            return val;
        } // MyMoney



        private double MyNumber(string what)
        {
            double val;
            Double.TryParse(what, out val);
            return val;
        } // MyNumber



        [Test]
        [Description("Verify Subtract button in Cart")]
        [Order(24)]
        public void FF_SubtractButtonCart()
        {
            // add two T-Shirts to Cart
            double price = PutProductInCart(true);

            // hit "-" in Classic Cart, bring new quantity and value
            double[] resultCC = StepClassicCart("-");

            // bring Ajax Cart quantity and value
            double[] resultAC = GetAjaxCart();

            // empty the Cart
            EmptyAjaxCart();

            // check Classic Cart
            Assert.That((1 == resultCC[0]), Is.True);
            Assert.That((price == resultCC[1]), Is.True);

            //check Ajax Cart
            Assert.That((1 == resultAC[0]), Is.True);
            Assert.That((price == resultAC[1]), Is.True);

        } // SubtractButtonInCart



        [Test]
        [Description("Verify empty Cart by Subtract button")]
        [Order(25)]
        public void FF_EmptyBySubtractButtonCart()
        {
            // add a T-Shirt to Cart
            PutProductInCart(false);

            // hit "-" in Classic Cart, bring new quantity and value
            double[] resultCC = StepClassicCart(".");
             
            // bring Ajax Cart quantity and value
            double[] resultAC = GetAjaxCart();

            // check Classic Cart
            Assert.That((0 == resultCC[0]), Is.True);
            Assert.That((0 == resultCC[1]), Is.True);

            //check Ajax Cart
            Assert.That((0 == resultAC[0]), Is.True);
            Assert.That((0 == resultAC[1]), Is.True);

        } // EmptyBySubtractButtonCart



    } // class



} // namespace