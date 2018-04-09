using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShopWithPOM
{
    //=====================POCOs==================================
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


    public class Order
    {
        public string Id { get; set; }
        public double Value { get; set; }
    } //

    //=========================Helpers==============================
    public static class Helper
    {
        // the UI thread is waiting the indicated number of milliseconds
        public static void FixedWait(int fixedMillisec)
        {
            if (fixedMillisec > 0)
            {
                // waiting on the foreground thread (UI) until timer stops on a threadpool thread
                var tsk = Task.Run(async () => { await Task.Delay(fixedMillisec); });
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

    //=======================================================
    public class LoginPart
    {
        IWebDriver driver;
        IWebElement e_user => driver.FindElement(By.Id("email"));
        IWebElement e_password => driver.FindElement(By.Id("passwd"));
        IWebElement e_submit => driver.FindElement(By.Id("SubmitLogin"));

        public LoginPart(IWebDriver drv) { driver = drv; }

        public bool Login(string user, string password)
        {
            bool ok = false;

            var obj = e_user;
            driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", obj);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {if (obj.Displayed && obj.Enabled) return true; else return false; });
            obj.Click();
            obj.SendKeys(user + Keys.Tab);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {if (obj.GetAttribute("value") == user) return true; else return false; });
            obj = e_password;
            obj.Click();
            obj.SendKeys(password + Keys.Tab);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {if (obj.GetAttribute("value") == password) return true; else return false; });

            obj = e_submit;
            obj.Click();

            Helper.FluidWait(10000, driver);
            if (driver.Title == "My account - My Store") ok = true;

            return ok;
        } // Login

    } // LoginPart

        //=======================================================
    public class MenuPart
    {
        IWebDriver driver;

        public MenuPart(IWebDriver drv) { driver = drv; }

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

    //=======================================================
    public class OfferPart
    { 
        IWebDriver driver;

        IWebElement e_price => driver.FindElement(By.Id("our_price_display"));
        IWebElement e_submit => driver.FindElement(By.Name("Submit"));
        IWebElement e_continue => driver.FindElement(By.XPath("//*[@title='Continue shopping']"));

        ReadOnlyCollection<IWebElement> e_products => driver.FindElements(By.XPath("//div[@class='product-container']"));
        ReadOnlyCollection<IWebElement> e_more_buttons => driver.FindElements(By.XPath("//a[contains(@class,'lnk_view')]"));

        public OfferPart(IWebDriver drv) { driver = drv; }

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
            wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });

            var todo = new Actions(driver);
            todo.MoveToElement(obj).Perform();

            var btn = btns[pos];
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (btn.Displayed && btn.Enabled) return true; else return false; });
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
            wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
            var todo = new Actions(driver);
            todo.MoveToElement(obj).Perform();

            var btn = btns[pos];
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (btn.Displayed && btn.Enabled) return true; else return false; });
            btn.Click();

            // "Quick View" is an IFRAME, we need to step into
            obj = driver.FindElement(By.XPath("//iframe[starts-with(@id,'fancybox')]"));
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
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
            wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
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
            wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
            var todo = new Actions(driver);
            todo.MoveToElement(obj).Perform();

            var btn = btns[pos];
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (btn.Displayed && btn.Enabled) return true; else return false; });
            btn.Click();

            Helper.FluidWait(10000, driver);

            // get unit price
            double price = Helper.Money(e_price.Text);

            // increase quantity
            var plus = driver.FindElement(By.XPath("//a[contains(@class,'product_quantity_up')]"));
            for (int i=1; i<howmany; i++)
            {
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(d => { if (plus.Displayed && plus.Enabled) return true; else return false; });
                plus.Click();
            }

            // hit "add to cart" button
            obj = e_submit;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
            obj.Click();

            // hit "continue shopping" button
            btn = e_continue;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (btn.Displayed && btn.Enabled) return true; else return false; });
            btn.Click();
            wait.Until(d => { if (!btn.Displayed) return true; else return false; });

            return price;
        } // PutProductInCart


        public bool PutDifferentProductsInCart(int howmany)
        {
            var objs = e_products;
            var btns = driver.FindElements(By.XPath("//a[contains(@class,'ajax_add_to_cart_button')]"));
            int nCount = Math.Min(objs.Count, btns.Count);
            nCount = Math.Min(howmany, nCount);
            if (nCount < 1) return false;

            for (int i = 0; i < nCount; i++)
            {
                // move to the current product
                var obj = objs[i];
                driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", obj);
                var todo = new Actions(driver);
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
                todo.MoveToElement(obj).Perform();

                // hit the current "Add to cart" button
                var btn = btns[i];
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(d => { if (btn.Displayed && btn.Enabled) return true; else return false; });
                btn.Click();

                // hit "Continue shopping" button
                obj = e_continue;
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
                obj.Click();
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(d => { if (!obj.Displayed) return true; else return false; });
            } // end for

            return true;
        } //PutDifferentProductsInCart

    } // OfferPart
    
    //=======================================================
    public class CartPart
    {
        IWebDriver driver;

        IWebElement e_open => driver.FindElement(By.XPath("//a[@title='View my shopping cart']"));
        //        ReadOnlyCollection<IWebElement> e_products => driver.FindElements());

        public CartPart(IWebDriver drv)
        { driver = drv; }

        public Stock StepClassicCart(string how)
        {
            // this currently works only for the first product (row) in the table
            Stock prod = new Stock();

            // open classic cart
            var obj = e_open;
            driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", obj);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
            obj.Click();

            Helper.FluidWait(5000, driver);

            // scroll to details
            obj = driver.FindElement(By.XPath("//td[@class='cart_product']"));
            driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", obj);
            // hit "+" or "-"
            string path = ((how == "+") ? "//a[starts-with(@id,'cart_quantity_up')]" : "//a[starts-with(@id,'cart_quantity_down')]");
            obj = driver.FindElement(By.XPath(path));
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
            obj.Click();

            // check cart empty condition when needed
            if (how == ".")
            {
                Helper.FixedWait(5000);
                Helper.FluidWait(5000, driver);
                obj = driver.FindElement(By.XPath("//*[text()='Your shopping cart is empty.']"));
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(d => { if (obj.Displayed) return true; else return false; });
                prod = null;
                return prod;
            }

            // wait UI refresh after recalculating values
            string newQuantity = (how == "+") ? "2" : "1";
            obj = driver.FindElement(By.XPath("//input[starts-with(@name,'quantity_') and @type='hidden']"));
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (obj.GetAttribute("value") == newQuantity) return true; else return false; });

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
            wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });

            obj = driver.FindElement(By.ClassName("shopping_cart"));
            var todo = new Actions(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
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
            wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
            var todo = new Actions(driver);
            todo.MoveToElement(obj).Perform();

            Helper.FluidWait(5000, driver);

            string path = "//a[starts-with(@class,'ajax_cart_block_remove_link')]";
            var objs = driver.FindElements(By.XPath(path));
            int howmany = objs.Count;
            if (howmany < 1) return;

            for (int i=0; i<howmany; i++)
            {
                // removing one item from the Cart
                obj = driver.FindElement(By.XPath(path));
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
                obj.Click();
                // the html code is modified (element removed, not just hidden)
                Helper.FixedWait(5000);
                Helper.FluidWait(5000, driver);
            } // end for

            // normally the SPAN with "(empty)" text should become visible
            obj = driver.FindElement(By.XPath("//*[starts-with(@class,'ajax_cart_no_product')]"));
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => { if (obj.Displayed) return true; else return false; });
        } // EmptyAjaxCart


        public double CheckoutCart()
        {
            // open classic cart
            var obj = e_open;
            driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", obj);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
            obj.Click();

            // getting the command's total
            obj = driver.FindElement(By.Id("total_price"));
            double total = Helper.Money(obj.Text);

            // hit button to continue with the next page
            obj = driver.FindElement(By.XPath("//a[contains(@class,'standard-checkout')]"));
            driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", obj);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
            obj.Click();

            Helper.FluidWait(10000, driver);
            return total;
        } // CheckoutCart

    } // CartPart

    //=======================================================
    public class CheckoutAddressPart
    {
        IWebDriver driver = null;

        public CheckoutAddressPart(IWebDriver drv) { driver = drv; }

        public void NextPage()
        {
            // hit button to continue with the next page
            var obj = driver.FindElement(By.Name("processAddress"));
            driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", obj);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
            obj.Click();

            Helper.FluidWait(10000, driver);
        } // NextPage

    } // CheckoutAddressPart

    //=======================================================
    public class CheckoutCarrierPart
    {
        IWebDriver driver = null;
        IWebElement e_terms => driver.FindElement(By.Id("uniform-cgv"));
        IWebElement e_next => driver.FindElement(By.Name("processCarrier"));

        public CheckoutCarrierPart(IWebDriver drv) { driver = drv; }

        public void CheckTerms()
        {
            // check terms and conditions
            var obj = e_terms;
            driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", obj);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (obj.Displayed) return true; else return false; });
            obj.Click();

            Helper.FluidWait(5000, driver);
        } // CheckTerms


        public void NextPage()
        {
            // hit button to continue
            var obj = e_next;
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
            obj.Click();

            Helper.FluidWait(10000, driver);
        } // NextPage


        public bool IsPaymentPage()
        {
            bool ok = false;
            // if I'm not on the Payments page, "ok" should be "false"
            try
            {
                var obj = driver.FindElement(By.XPath("//*[@class='navigation_page' and text()='Your payment method']"));
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(d => { if (obj.Displayed) return true; else return false; });
                ok = true;
            }
            catch { }

            return ok;
        } // TryToPay

    } // CheckoutCarrierPart

    //=======================================================
    public class AlertPart
    {
        IWebDriver driver = null;

        public AlertPart(IWebDriver drv) { driver = drv; }

        public bool? IsAlert(string message)
        {
            bool? ok = false;
            // verify presence of Alert
            try
            {
                var obj = driver.FindElement(By.XPath("//*[@class='fancybox-error']"));
                driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", obj);
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(d => { if (obj.Displayed) return true; else return false; });
                if (obj.Text == message) ok = true;
                else ok = null;
            }
            catch { }

            return ok;
        } // IsAlert


        public void Close()
        {
            Helper.FixedWait(5000);
            Helper.FluidWait(5000, driver);
            var obj = driver.FindElement(By.XPath("//a[@title='Close']"));
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (obj.Displayed) return true; else return false; });
            obj.Click();
            Helper.FluidWait(5000, driver);
        } // Close

    } // AlertPart

    //=======================================================
    public class CheckoutPaymentPart
    {
        IWebDriver driver = null;

        public CheckoutPaymentPart(IWebDriver drv) { driver = drv; }

        public void SelectCheque()
        {
            // select payment method "by cheque"
            var obj = driver.FindElement(By.XPath("//a[@class='cheque']"));
            driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", obj);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(8));
            wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
            obj.Click();
            Helper.FluidWait(10000, driver);
        } // SelectMethod


        public void SelectBank()
        {
            // select payment method "by cheque"
            var obj = driver.FindElement(By.XPath("//a[@class='bankwire']"));
            driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", obj);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(8));
            wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
            obj.Click();
            Helper.FluidWait(10000, driver);
        } // SelectMethod

    } // CheckoutPaymentPart


    //=======================================================
    public class OrderConfirmationPart
    {
        IWebDriver driver = null;

        public OrderConfirmationPart(IWebDriver drv) { driver = drv; }

        public string Confirm()
        {
            // confirm order
            var obj = driver.FindElement(By.XPath("//button[contains(@class,'button-medium')]"));
            driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", obj);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
            obj.Click();

            Helper.FluidWait(10000, driver);

            // extract order id from malformed html section
            obj = driver.FindElement(By.XPath("//div[contains(@class,'order-confirmation')]"));
            string bigtext = obj.GetAttribute("innerHTML");
            var result = Regex.Matches(bigtext, "your order reference [A-Z]{9}");
            string id = "";
            if (result.Count > 0) id = result[0].Value.Substring(21);

            return id;
        } // Confirm


        public void NextPage()
        {
            // go to instant order history
            var obj = driver.FindElement(By.XPath("//a[starts-with(@class,'button-exclusive')]"));
            driver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", obj);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (obj.Displayed && obj.Enabled) return true; else return false; });
            obj.Click();
            Helper.FluidWait(10000, driver);
        } // NextPage

    } // OrderConfirmationPart

    //=======================================================
    public class HistoryPart
    {
        IWebDriver driver = null;

        public HistoryPart(IWebDriver drv) { driver = drv; }

        public Order GetLatest()
        {
            Order latest = new Order();
            // get order id
            var obj = driver.FindElement(By.XPath("//a[@class='color-myaccount']"));
            latest.Id = obj.Text;
            // get total
            obj = driver.FindElement(By.XPath("//*[@class='history_price']/span"));
            latest.Value = Helper.Money(obj.Text);

            return latest;
        } // GetLatest

    } // HistoryPart
    //=======================================================

} // namespace ShopWithPOM
