using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace NUTests
{
    public partial class Shopping1
    {
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

            string path = ((upwards == "+") ? ".//a[starts-with(@id,'cart_quantity_up')]" : ".//a[starts-with(@id,'cart_quantity_down')]");
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
