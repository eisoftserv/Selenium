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



        [Test]
        [Description("Checkout and verify latest order in history")]
        [Order(26)]
        public void FF_CheckoutAndVerifyHistory()
        {
            // add 3 products in cart
            PutMoreProductsInCart(3);

            // do first part of checkout, get command's Total
            double total1 = Checkout123();

            // do second part of checkout, get command's history: Id & Total
            string[] history = Checkout456();
            double total2 = MyMoney(history[1]);

            // check totals
            Assert.That((total1 == total2), Is.True);

        } // CheckoutAndVerifyHistory



        [Test]
        [Description("Checkout and verify latest order in history")]
        [Order(27)]
        public void FF_CheckoutWithoutTerms()
        {
            // add products in cart
            PutProductInCart(true);

            // do first part of checkout, get command's Total
            Checkout123();

            // hit checkout button to continue
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(6));
            var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("processCarrier")));
            obj.Click();

            // verify presence of Alert
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(7));
            obj = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@class='fancybox-error']")));
            Assert.That((obj.Text == "You must agree to the terms of service before continuing."), Is.True);

            // try to pay
            Assert.That(TryToPay(), Is.False);

            // close alert
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(8));
            obj = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(@class,'fancybox-close')]")));
            obj.Click();

            // again: try to pay
            Assert.That(TryToPay(), Is.False);

        } // CheckoutWithoutTerms



        internal bool TryToPay()
        {
            bool ok = false;

            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                var obj = wait.Until(ExpectedConditions.ElementExists(By.XPath("//a[@class='bankwire']")));
                ok = true;
            }
            catch
            {
                try
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                    var obj = wait.Until(ExpectedConditions.ElementExists(By.XPath("//a[@class='cheque']")));
                    ok = true;
                }
                catch { }
            }

            return ok;

        } // TryToPay



        internal void PutMoreProductsInCart(int howmany)
        {
            // go to the "Dresses" menu
            var todo = new Actions(driver);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var obj = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//ul[starts-with(@class,'sf-menu')]/li[2]")));
            todo.MoveToElement(obj).Click().Perform();
            // waiting until products are loaded
            JustWait(5000);

            // collect product containers and "Add to cart" buttons
            var objs = driver.FindElements(By.XPath("//div[@class='product-container']"));
            var btns = driver.FindElements(By.XPath("//a[contains(@class,'ajax_add_to_cart_button')]"));
            int nCount = Math.Min(howmany, objs.Count);

            for (int i=0; i<nCount; i++)
            {
                //var btns = driver.FindElements(By.XPath(".//div[@class='right-block']/div[2]/a"));
                // move to the next product
                jse.ExecuteScript("arguments[0].scrollIntoView(true);", objs[i]);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(6));
                wait.Until(ExpectedConditions.ElementToBeClickable(objs[i]));
                todo = new Actions(driver);
                todo.MoveToElement(objs[i]).Perform();
                // hit "Add to cart" button
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(7));
                wait.Until(ExpectedConditions.ElementToBeClickable(btns[i]));
                btns[i].Click();
                // hit "Continue shopping" button
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(6));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[@title='Continue shopping']")));
                obj.Click();
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//*[@title='Continue shopping']")));
            } // end for

        } // PutMoreProductsInCart



        internal double Checkout123()
        {
            // open classic cart
            var obj = driver.FindElement(By.XPath(".//a[@title='View my shopping cart']"));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementToBeClickable(obj));
            obj.Click();

            // getting the command's total
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.Id("total_price")));
            double total = MyMoney(obj.Text);

            // hit button to continue
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//a[contains(@class,'standard-checkout')]")));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementToBeClickable(obj));
            obj.Click();

            // hit button to continue
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.Name("processAddress")));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementToBeClickable(obj));
            obj.Click();

            return total;

        } // Checkout123



        internal string[] Checkout456()
        {
            // check terms and conditions
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var obj = wait.Until(ExpectedConditions.ElementExists(By.Id("uniform-cgv")));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("uniform-cgv")));
            obj.Click();

            // hit button to continue
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("processCarrier")));
            obj.Click();

            // select payment method
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//a[@class='cheque']")));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(8));
            wait.Until(ExpectedConditions.ElementToBeClickable(obj));
            obj.Click();

            // confirm order
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//button[contains(@class,'button-medium')]")));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementToBeClickable(obj));
            obj.Click();

            // go to instant order history
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(6));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//a[starts-with(@class,'button-exclusive')]")));
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", obj);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(6));
            wait.Until(ExpectedConditions.ElementToBeClickable(obj));
            obj.Click();

            string[] result = { "", "" };
            // get order id
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[@class='color-myaccount']")));
            result[0] = obj.Text;
            // get total
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@class='history_price']/span")));
            result[1] = obj.Text;

            return result;

        } // Checkout456



    } // class



} // namespace
