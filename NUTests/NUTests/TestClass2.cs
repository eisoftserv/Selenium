using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;

namespace NUTests
{
    [TestFixture]
    [SingleThreaded]
    [Description("exercises for phptravels.net")]
    public class TestClass2
    {
        internal FirefoxDriver driver = null;



        [OneTimeSetUp]
        [Description("logging in as demo user")]
        public void FF_LoggingIn()
        {
            string loginUrl = "https://www.phptravels.net/login";
            string user = "user@phptravels.com";
            string password = "demouser";

            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Url = loginUrl;

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("username")));
            obj.SendKeys(user);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("password")));
            obj.SendKeys(password);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[contains(@class,'loginbtn') and @type='submit']")));
            obj.Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("preloader")));

        } // LoggingIn



        [OneTimeTearDown]
        [Description("logging out as demo user")]
        public void FF_LoggingOut()
        {
            string logoutUrl = "https://www.phptravels.net/account/logout/";
            driver.Navigate().GoToUrl(logoutUrl);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("preloader")));

            // normally we are redirected to the Login page
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var obj = wait.Until(ExpectedConditions.ElementExists(By.Name("username")));

            driver.Quit();

        } // LoggingOut



        [SetUp]
        [Description("Navigating to the Home page")]
        public void FF_GoToStartPage()
        {
            string startUrl = "https://www.phptravels.net/";

            driver.Navigate().GoToUrl(startUrl);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("preloader")));

        } // GoToStartPage



        [Test]
        [Order(1)]
        [Description("booking a tour of Dubai")]
        public void FF_BookingTour()
        {
            var jse = (IJavaScriptExecutor)driver;

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//a[starts-with(@href,'#TOURS')]")));
            obj.Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("preloader")));

            //--------------- tour

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[@id='s2id_autogen10']/a")));
            obj.Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[contains(@class,'select2-focused')]")));
            obj.SendKeys("Dubai");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//li[starts-with(@class,'select2-results-dept-1') and contains(@class,'select2-result-selectable')]")));
            obj.Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("preloader")));

            //------------------ appointment

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//input[@name='date']")));
            obj.Click();
            obj.Clear();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

            DateTime ddate = DateTime.Now.AddDays(DateTime.Now.Minute);
            string sdate = ddate.Day + "/" + ddate.Month + "/" + ddate.Year;
            obj.SendKeys(sdate + Keys.Tab);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("preloader")));

            //----------------- guests

            obj = driver.FindElement(By.XPath(".//select[@name='adults' and contains(@class,'selectx')]"));
            var dd = new SelectElement(obj);
            dd.SelectByText("1");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("preloader")));

            //----------------- submit search

            // we have multiple submit buttons, the second one works for Tours
            var objs = driver.FindElements(By.XPath(".//button[@type='submit']"));
            objs[1].Click();

            //---------------------- reviewing selection

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("preloader")));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//button[text()='Book Now']")));
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", obj);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("preloader")));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[text()='Book Now']")));

            obj.Click();

            //---------------------- filling guest data and confirming booking

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("preloader")));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//input[@placeholder='Name']")));
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", obj);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("preloader")));

            string guestName = "Jane Doe" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
            string guestPassport = (Math.Pow(DateTime.Now.Hour * DateTime.Now.Minute * DateTime.Now.Second, 2)).ToString();
            string guestAge = (20 + DateTime.Now.Minute).ToString();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[@placeholder='Name']")));
            obj.SendKeys(guestName);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[@placeholder='Passport']")));
            obj.SendKeys(guestPassport);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[@placeholder='Age']")));
            obj.SendKeys(guestAge);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[text()='CONFIRM THIS BOOKING']")));

            obj.Click();

            //----------------------- payment options

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("preloader")));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//button[text()='Pay on Arrival']")));
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", obj);

            Hotel_Tour_Ending();

        } // BookingTour



        [Test]
        [Order(2)]
        [Description("booking a flight from London to Dubai")]
        public void FF_BookingFlight()
        {
            // generating randomized data for preventing duplicate booking error
            string fname = "John" + DateTime.Now.Hour.ToString();
            string lname = "Doe" + DateTime.Now.Minute.ToString();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//i[contains(@class,'fa-plane')]")));
            obj.Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("preloader")));


            //------------- stepping into the Angular gadjet's iframe

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//iframe[starts-with(@id,'travelstartIframe')]")));
            string frameid = obj.GetAttribute("id");
            driver.SwitchTo().Frame(frameid);

            //------------- filling in data for searching

            // opting for one-way flight
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[@id='trip-type-radio-group']/label")));
            obj.Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[starts-with(@id,'airports-inline-orig')]")));
            obj.Click();
            obj.SendKeys("LON");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//li[starts-with(@id,'typeahead-24')]/a/span/span/strong")));
            Assert.That(obj.Text, Is.EqualTo("LON"));
            obj.Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(".//ul[starts-with(@id,'typeahead-26')]")));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[starts-with(@id,'airports-inline-dest')]")));
            obj.Click();
            obj.SendKeys("DXB");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//li[starts-with(@id,'typeahead-26')]/a/span/span/strong")));
            Assert.That(obj.Text, Is.EqualTo("DXB"));
            obj.Click();

            // randomized departure day
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[@class='datepicker__next-month']")));
            obj.Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[starts-with(@class,'datepicker-table__btn')]")));

            var objs = driver.FindElements(By.XPath(".//button[starts-with(@class,'datepicker-table__btn')]"));
            int number = (DateTime.Now.Minute) % 29;
            if (objs.Count > number) objs[number].Click();
            else objs[0].Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[@id='search-for-flights-button']")));
            obj.Click();

            //------------------- selecting the first search result

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[@class='ng-scope' and text()='View']")));
            obj.Click();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[starts-with(@class,'flight-details__submit')]")));
            obj.Click();

            //------------------- filling personal data

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//select[@name='ADULT1:honorific-prefix']")));
            var dd = new SelectElement(obj);
            dd.SelectByText("Mr");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[@name='ADULT1:fname']")));
            obj.SendKeys(fname);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[@name='ADULT1:lname']")));
            obj.SendKeys(lname);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[text()='Same as first adult']")));
            obj.Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//input[@name='email']")));
            var jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", obj);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[@name='email']")));
            obj.SendKeys(fname + "@" + lname + ".com");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[@name='mobile']")));
            obj.SendKeys("0505555555");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[@class='ng-scope' and text()='Continue']")));
            obj.Click();

            //------------------- unchecking extra services included by default

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@class='ng-scope' and text()='Remove']")));
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", obj);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[@class='ng-scope' and text()='Remove']")));

            var lnks = driver.FindElements(By.XPath(".//*[starts-with(@class,'ts-card__option-add')]"));
            objs = driver.FindElements(By.XPath(".//*[@class='ng-scope' and text()='Remove']"));

            for (int i=0; i<2; i++)
            {
                if (objs.Count <= i) break;
                objs[i].Click();
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(ExpectedConditions.TextToBePresentInElement(lnks[i], "Add to Booking"));
            }

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[@class='ng-scope' and text()='Continue']")));
            obj.Click();

            //------------------- filling payment details

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[@name='cardnumber']")));
            obj.SendKeys("4111111111111111"); // test number for VISA cards

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[@name='ccname']")));
            obj.SendKeys(fname + " " + lname);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//select[@name='ccmonth']")));
            dd = new SelectElement(obj);
            dd.SelectByText("02");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//select[@name='ccyear']")));
            dd = new SelectElement(obj);
            dd.SelectByText("2020");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[@name='cvv']")));
            obj.SendKeys("123");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[@name='billingAddressLine1']")));
            obj.SendKeys("123 Main Street");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[@name='billingAddressLine2']")));
            obj.SendKeys("PO BOX 123");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[@name='billingPostalCode']")));
            obj.SendKeys("9966");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[@name='billingCity']")));
            obj.SendKeys("Dubai");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//input[@name='billingContactNo']")));
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", obj);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[@name='billingContactNo']")));
            obj.SendKeys("0505555555");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[text()='Book now']")));
            obj.Click();

            //------------------- handling voucher offer - sometimes it appears, other times not

            try
            {
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[@class='ng-scope' and text()='Continue (without voucher)']")));
                obj.Click();
            }
            catch
            { }

            // verifying that we are on the THANK YOU page
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[starts-with(text(),'Thank you')]")));

            // stepping out from the iframe of the Angular gadget
            driver.SwitchTo().DefaultContent();

            // dummy verification 
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("preloader")));

            Assert.Pass("Booking Flight done");

        } // BookingFlight



        [Test]
        [Order(3)]
        [Description("booking a hotel room in Dubai")]
        public void FF_BookingHotel()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//input[@id='citiesInput']")));
            obj.Click();
            obj.SendKeys("Dubai");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[@class='eac-item']")));
            obj.Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            // wait until the autocomplete list disappears - otherwise it would cover other fields
            wait.Until( (x) =>
            {
                return !(x.FindElement(By.XPath(".//div[@id='eac-container-citiesInput']/ul")).Displayed);
            });

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            //------------------ appointments

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//input[@name='checkin']")));
            obj.Click();
            obj.Clear();

            int mydays = DateTime.Now.Minute;
            DateTime ddate = DateTime.Now.AddDays(mydays);
            string sdate = ddate.Day + "/" + ddate.Month + "/" + ddate.Year;
            obj.SendKeys(sdate + Keys.Tab);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//input[@name='checkout']")));
            obj.Click();
            obj.Clear();

            ddate = DateTime.Now.AddDays(mydays + 7);
            sdate = ddate.Day + "/" + ddate.Month + "/" + ddate.Year;
            obj.SendKeys(sdate + Keys.Tab);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            //----------------- number of persons

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//select[@name='adults']")));
            var dd = new SelectElement(obj);
            dd.SelectByText("1");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            //----------------- submit search

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[@type='submit']")));
            obj.Click();

            //------------ reviewing offer

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//button[text()='Details']")));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[text()='Details']")));
            obj.Click();

            //------------- booking

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//button[text()='Book Now']")));
            var jse = (IJavaScriptExecutor)driver;
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", obj);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[text()='Book Now']")));
            obj.Click();

            //-------------- confirming

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//button[text()='CONFIRM THIS BOOKING']")));
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", obj);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[text()='CONFIRM THIS BOOKING']")));
            obj.Click();

            //--------------- pay on Arrival

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//button[text()='Pay on Arrival']")));
            jse.ExecuteScript("arguments[0].scrollIntoView(true)", obj);

            Hotel_Tour_Ending();

        } // BookingHotel



        public void Hotel_Tour_Ending()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[@id='preloader']")));

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(9));
            var obj = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//button[text()='Pay on Arrival']")));
            obj.Click();

            //--------------- handling JavaScript modal dialog: confirming payment on arriva;
            try
            {
                driver.SwitchTo().Alert().Accept();
            }
            catch
            { }
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("preloader")));

            //--------------- verifying that we are on the Invoice page
            bool ok = false;
            try
            {
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                obj = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//div[text()='Invoice']")));
                ok = true;
            }
            catch
            { }

            if (ok == true) Assert.Pass("Invoice found");
            else Assert.Fail("Invoice not found");

        } // InvoicePageCheck



    } // TestClass2



} // namespace
