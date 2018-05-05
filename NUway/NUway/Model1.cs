using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUway
{
    public class SelectablePart
    {
        IWebDriver driver;
        IWebElement eDefault => driver.FindElement(By.LinkText("Default functionality"));
        IWebElement eGrid => driver.FindElement(By.LinkText("Display as grid"));
        IWebElement eSerial => driver.FindElement(By.LinkText("Serialize"));

        IWebElement eDefaultTest => driver.FindElement(By.XPath("//ol[@id='selectable']/li[4]"));
        IWebElement eGridTest => driver.FindElement(By.XPath("//ol[@id='selectable']/li[7]"));
        IWebElement eSerialTest => driver.FindElement(By.XPath("//ol[@id='selectable']/li[5]"));

        IWebElement iDefault => driver.FindElement(By.XPath("//iframe[@src='selectable/default.html']"));
        IWebElement iGrid => driver.FindElement(By.XPath("//iframe[@src='selectable/default2.html']"));
        IWebElement iSerial => driver.FindElement(By.XPath("//iframe[@src='selectable/default3.html']"));

        IWebElement eSerialMessage => driver.FindElement(By.Id("feedback"));

        string class_selected = "ui-selected";


        public SelectablePart(IWebDriver drv)
        { driver = drv; }


        void SelectIframe(string functionality)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            if (functionality == "Default")
            {
                wait.Until(d => { if (iDefault.Displayed) return true; else return false; });
                driver.SwitchTo().Frame(iDefault);
            }
            else if (functionality == "Grid")
            {
                wait.Until(d => { if (iGrid.Displayed) return true; else return false; });
                driver.SwitchTo().Frame(iGrid);
            }
            else if (functionality == "Serial")
            {
                wait.Until(d => { if (iSerial.Displayed) return true; else return false; });
                driver.SwitchTo().Frame(iSerial);
            }
        } //


        public void SelectFunctionality(string functionality)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            if (functionality == "Default")
            {
                wait.Until( d => { if (eDefault.Displayed) return true; else return false; });
                eDefault.Click();
            }
            else if (functionality == "Grid")
            {
                wait.Until(d => { if (eGrid.Displayed) return true; else return false; });
                eGrid.Click();
            }
            else if (functionality == "Serial")
            {
                wait.Until(d => { if (eSerial.Displayed) return true; else return false; });
                eSerial.Click();
            }
            Helper.FluidWait(5000, driver);
        } // SelectFunctionality


        public void SelectItem(string functionality)
        {
            SelectIframe(functionality);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            if (functionality == "Default")
            {
                wait.Until(d => { if (eDefaultTest.Displayed) return true; else return false; });
                eDefaultTest.Click();
            }
            else if (functionality == "Grid")
            {
                wait.Until(d => { if (eGridTest.Displayed) return true; else return false; });
                eGridTest.Click();
            }
            else if (functionality == "Serial")
            {
                wait.Until(d => { if (eSerialTest.Displayed) return true; else return false; });
                eSerialTest.Click();
            }

            Helper.FluidWait(5000, driver);
            driver.SwitchTo().DefaultContent();
        } // SelectItem


        public bool VerifyItem(string functionality)
        {
            SelectIframe(functionality);
            bool ok = false;
            IWebElement obj = null;

            if (functionality == "Default") obj = eDefaultTest;
            else if (functionality == "Grid") obj = eGridTest;
            else if (functionality == "Serial") obj = eSerialTest;

            if (obj.GetAttribute("class").Contains(class_selected))
            {
                if (functionality == "Default" && obj.Text == "Item 4") ok = true;
                else if (functionality == "Grid" && obj.Text == "7") ok = true;
                else if (functionality == "Serial" && obj.Text == "Item 5") ok = true;
            }

            driver.SwitchTo().DefaultContent();
            return ok;
        } //


        public bool VerifySerialMessage()
        {
            SelectIframe("Serial");
            bool ok = false;

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (eSerialMessage.Displayed) return true; else return false; });
            string bigtext = eSerialMessage.GetAttribute("innerHTML");
            if (bigtext.Contains("You've selected:") && bigtext.Contains("#5")) ok = true;

            driver.SwitchTo().DefaultContent();
            return ok;
        } //


    } // class SelectablePart



    public class LoginPart
    {
        IWebDriver driver;
        string LoginUrl = "http://way2automation.com/way2auto_jquery/index.php";

        string PageTitle = "Welcome to the Test Site";
        string ErrorMessage = "Invalid username password.";

        IWebElement eRegistration => driver.FindElement(By.XPath("//*[text()='Registration Form']"));
        IWebElement eLogin => driver.FindElement(By.XPath("//*[text()='Login']"));
        IWebElement eSignin => driver.FindElement(By.LinkText("Signin"));
        IWebElement eErrorMessage => driver.FindElement(By.XPath("//*[@id='alert1']"));

        IWebElement eUsername => driver.FindElement(By.XPath("//form[@id='load_form' and @class='ajaxlogin']/fieldset[1]/input"));
        IWebElement ePassword => driver.FindElement(By.XPath("//form[@id='load_form' and @class='ajaxlogin']/fieldset[2]/input"));
        IWebElement eSubmit => driver.FindElement(By.XPath("//form[@id='load_form' and @class='ajaxlogin']/div/div[2]/input"));

        IWebElement eInteraction => driver.FindElement(By.XPath("//ul[@id='toggleNav']/li[2]/a"));
        IWebElement eSelectable => driver.FindElement(By.XPath("//ul[@id='toggleNav']/li[2]/ul/li[4]/a"));
        IWebElement eSortable => driver.FindElement(By.XPath("//ul[@id='toggleNav']/li[2]/ul/li[5]/a"));

        public LoginPart(IWebDriver drv)
        { driver = drv; }


        public bool Navigate()
        {
            driver.Url = LoginUrl;
            Helper.FluidWait(10000, driver);
            if (driver.Title == PageTitle) return true;
            return false;
        } //


        public void Goto()
        {
            driver.Navigate().GoToUrl(LoginUrl);
            Helper.FluidWait(10000, driver);
        } //


        public void SelectMenu(string menu, string submenu)
        {
            var todo = new Actions(driver);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            if (menu == "Interactions")
            {
                wait.Until(d => { if (eInteraction.Displayed) return true; else return false; });
                todo.MoveToElement(eInteraction).Perform();

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                if (submenu == "Selectable")
                {
                    wait.Until(d => { if (eSelectable.Displayed) return true; else return false; });
                    eSelectable.Click();
                }
                else if (submenu == "Sortable")
                {
                    wait.Until(d => { if (eSortable.Displayed) return true; else return false; });
                    eSortable.Click();
                }
            }
        } // SelectMenu


        public bool IsForm(bool registration)
        {
            bool ok = false;

            try
            {
                WebDriverWait wait;
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(d => {
                    bool lok = false;
                    try
                    {
                        if (registration) { if (eRegistration.Displayed) lok = true; }
                        else { if (eLogin.Displayed) lok = true; }
                    }
                    catch { }
                    return lok;
                });
                ok = true;
            }
            catch
            { }

            return ok;
        } //


        public bool IsErrorMessage()
        {
            bool ok = false;
            WebDriverWait wait;
            try
            {
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(d => {
                    bool lok = false;
                    try
                    { if (eErrorMessage.Displayed) lok = true; }
                    catch { }
                    return lok;
                });
                if (eErrorMessage.Text == ErrorMessage) ok = true;
            }
            catch
            { }

            return ok;
        } //


        public void OperateForm(string username, string password)
        {
            IWebElement obj = null;
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (eSignin.Displayed) return true; else return false; });
            eSignin.Click();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {
                obj = eUsername;
                if (obj.Displayed && obj.Enabled) return true; else return false;
            });
            obj.Click();
            obj.SendKeys(username + Keys.Tab);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {
                obj = ePassword;
                if (obj.Displayed && obj.Enabled) return true; else return false;
            });
            obj.SendKeys(password + Keys.Tab);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(8));
            wait.Until(d => {
                obj = eSubmit;
                if (obj.Displayed) return true; else return false;
            });
            obj.Click();

            Helper.FixedWait(5000);
            Helper.FluidWait(10000, driver);
        } //


    } // class LoginPart



    public static class Helper
    {
        public static string username = "your_username";
        public static string password = "your_password";

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


    } // class Helper


} // NUway
