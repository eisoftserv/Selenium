using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWithPOM
{
    [TestFixture]
    [SingleThreaded]
    [Description("POM exercises for automationpractice.com")]
    public partial class ShoPom1
    {
        internal FirefoxDriver driver = null;

        //=======================================================
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

        //=======================================================
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

        //=======================================================
        [SetUp]
        [Description("navigating to the landing page")]
        public void GoToLandingPage()
        {
            string startUrl = "http://automationpractice.com/index.php";
            driver.Navigate().GoToUrl(startUrl);
            bool ok = Helper.FluidWait(10000, driver);
            Assert.That(ok && (driver.Title == "My Store"), Is.True);
        } // GoToLandingPage

        //=======================================================
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

            Assert.That(summary != null && detail != null && summary.Name == detail.Name && summary.Price == detail.Price, Is.True);
        } // ProductSummary_MoreView

        //=======================================================
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

        //=======================================================
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
        } // AddButtonCart

        //=======================================================
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


        //=======================================================
        [Test]
        [Description("Verify empty Cart by Subtract button")]
        [Order(35)]
        public void EmptyCartBySubtractButton()
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

        //=======================================================
        [Test]
        [Description("Checkout and verify latest order in history")]
        [Order(36)]
        public void CheckoutAndVerifyHistory()
        {
            var menu = new MenuPart(driver);
            // hitting the menu should return at least one result
            Assert.That(menu.BringDresses(), Is.True);

            var offer = new OfferPart(driver);
            Assert.That(offer.PutDifferentProductsInCart(3), Is.True);

            Order myOrder = new Order();

            var cart = new CartPart(driver);
            myOrder.Value = cart.CheckoutCart();

            var address = new CheckoutAddressPart(driver);
            address.NextPage();

            var carrier = new CheckoutCarrierPart(driver);
            carrier.CheckTerms();
            carrier.NextPage();

            var payment = new CheckoutPaymentPart(driver);
            payment.SelectCheque();

            var order = new OrderConfirmationPart(driver);
            myOrder.Id = order.Confirm();
            order.NextPage();

            var history = new HistoryPart(driver);
            Order latestOrder = history.GetLatest();

            cart.EmptyAjaxCart();

            Assert.That((myOrder.Id == latestOrder.Id && myOrder.Value == latestOrder.Value), Is.True);
        } // CheckoutAndVerifyHistory

        //=======================================================
        [Test]
        [Description("Try checkout without accepting the Terms and Conditions")]
        [Order(37)]
        public void CheckoutWithoutTerms()
        {
            var menu = new MenuPart(driver);
            // hitting the menu should return at least one result
            Assert.That(menu.BringTshirts(), Is.True);

            var offer = new OfferPart(driver);
            double price = offer.PutProductInCart(0, 1);
            Assert.That(price >= 0, Is.True);

            Order myOrder = new Order();

            var cart = new CartPart(driver);
            myOrder.Value = cart.CheckoutCart();

            var address = new CheckoutAddressPart(driver);
            address.NextPage();

            var modal = new AlertPart(driver);
            var carrier = new CheckoutCarrierPart(driver);

            // trying to go to the Payment Page without checking Terms & Conditions
            carrier.NextPage();
            // verifying if I'm getting the expected alert message
            // NULL is returned when the message is not the same
            bool? alert = modal.IsAlert("You must agree to the terms of service before continuing.");
            if (alert != false) modal.Close();
            // are we on the Payment page?
            bool payment = carrier.IsPaymentPage();

            cart.EmptyAjaxCart();

            Assert.That(payment == false && alert != false, Is.True);
        } // CheckoutWithoutTerms


    } // class ShoPom1
    //=======================================================

} // namespace ShopWithPOM
