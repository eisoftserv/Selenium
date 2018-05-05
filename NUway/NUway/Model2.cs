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
    class SortablePart
    {
        IWebDriver driver;
        IWebElement eDefault => driver.FindElement(By.LinkText("Default functionality"));
        IWebElement eConnect => driver.FindElement(By.LinkText("Connect Lists"));
        IWebElement eGrid => driver.FindElement(By.LinkText("Display as grid"));

        IWebElement iDefault => driver.FindElement(By.XPath("//iframe[@src='sortable/default.html']"));
        IWebElement iConnect => driver.FindElement(By.XPath("//iframe[@src='sortable/default2.html']"));
        IWebElement iGrid => driver.FindElement(By.XPath("//iframe[@src='sortable/default3.html']"));

        IWebElement dFirst => driver.FindElement(By.XPath("//ul[@id='sortable']/li[1]"));
        IWebElement dSecond => driver.FindElement(By.XPath("//ul[@id='sortable']/li[2]"));
        IWebElement dFifth => driver.FindElement(By.XPath("//ul[@id='sortable']/li[5]"));
        IWebElement dSeventh => driver.FindElement(By.XPath("//ul[@id='sortable']/li[7]"));

        IWebElement dTenth => driver.FindElement(By.XPath("//ul[@id='sortable']/li[10]"));
        IWebElement dTwelvth => driver.FindElement(By.XPath("//ul[@id='sortable']/li[12]"));

        IWebElement connect12 => driver.FindElement(By.XPath("//ul[@id='sortable1']/li[2]"));
        IWebElement connect14 => driver.FindElement(By.XPath("//ul[@id='sortable1']/li[4]"));
        IWebElement connect15 => driver.FindElement(By.XPath("//ul[@id='sortable1']/li[5]"));
        IWebElement connect24 => driver.FindElement(By.XPath("//ul[@id='sortable2']/li[4]"));
        IWebElement connect25 => driver.FindElement(By.XPath("//ul[@id='sortable2']/li[5]"));

        public SortablePart(IWebDriver drv)
        { driver = drv; }


        void SelectIframe(string functionality)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            if (functionality == "Default")
            {
                wait.Until(d => { if (iDefault.Displayed) return true; else return false; });
                driver.SwitchTo().Frame(iDefault);
            }
            else if (functionality == "Connect")
            {
                wait.Until(d => { if (iConnect.Displayed) return true; else return false; });
                driver.SwitchTo().Frame(iConnect);
            }
            else if (functionality == "Grid")
            {
                wait.Until(d => { if (iGrid.Displayed) return true; else return false; });
                driver.SwitchTo().Frame(iGrid);
            }
        } //


        public void SelectFunctionality(string functionality)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            if (functionality == "Default")
            {
                wait.Until(d => { if (eDefault.Displayed) return true; else return false; });
                eDefault.Click();
            }
            else if (functionality == "Connect")
            {
                wait.Until(d => { if (eConnect.Displayed) return true; else return false; });
                eConnect.Click();
            }
            else if (functionality == "Grid")
            {
                wait.Until(d => { if (eGrid.Displayed) return true; else return false; });
                eGrid.Click();
            }
            Helper.FluidWait(5000, driver);
        } // SelectFunctionality


        public void DefaultDragDrop()
        {
            SelectIframe("Default");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (dSeventh.Displayed) return true; else return false; });
            int yEnd = dSeventh.Location.Y;

            var todo = new Actions(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (dSecond.Displayed) return true; else return false; });
            var obj = dSecond;
            int yBegin = obj.Location.Y;
            todo.DragAndDropToOffset(obj, 0, yEnd - yBegin + 20).Perform();
            Helper.FluidWait(3000, driver);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (dFirst.Displayed) return true; else return false; });
            yEnd = dFirst.Location.Y;

            todo = new Actions(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (dFifth.Displayed) return true; else return false; });
            obj = dFifth;
            yBegin = obj.Location.Y;
            todo.DragAndDropToOffset(obj, 0, yEnd - yBegin - 20).Perform();
            Helper.FluidWait(3000, driver);

            driver.SwitchTo().DefaultContent();
        } //


        public bool DefaultVerify()
        {
            bool ok = false;
            SelectIframe("Default");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (dFirst.Displayed) return true; else return false; });
            string first = dFirst.Text;

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (dSeventh.Displayed) return true; else return false; });
            string last = dSeventh.Text;

            if (first == "Item 6" && last == "Item 2") ok = true;

            driver.SwitchTo().DefaultContent();
            return ok;
        } //


        public void ConnectDragDrop()
        {
            SelectIframe("Connect");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (connect25.Displayed) return true; else return false; });
            var obj = connect25;
            int xEnd = obj.Location.X;
            int yEnd = obj.Location.Y;

            var todo = new Actions(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (connect12.Displayed) return true; else return false; });
            obj = connect12;
            int xBegin = obj.Location.X;
            int yBegin = obj.Location.Y;
            todo.DragAndDropToOffset(obj, xEnd-xBegin+20, yEnd-yBegin+20).Perform();

            Helper.FluidWait(3000, driver);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (connect14.Displayed) return true; else return false; });
            obj = connect14;
            xEnd = obj.Location.X;
            yEnd = obj.Location.Y;

            todo = new Actions(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (connect24.Displayed) return true; else return false; });
            obj = connect24;
            xBegin = obj.Location.X;
            yBegin = obj.Location.Y;
            todo.DragAndDropToOffset(obj, xEnd - xBegin + 20, yEnd - yBegin + 20).Perform();

            Helper.FluidWait(3000, driver);

            driver.SwitchTo().DefaultContent();
        } //


        public bool ConnectVerify()
        {
            bool ok = false;
            SelectIframe("Connect");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (connect15.Displayed) return true; else return false; });
            string firstText = connect15.Text;
            string firstStyle = connect15.GetAttribute("class");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (connect24.Displayed) return true; else return false; });
            string secondText = connect24.Text;
            string secondStyle = connect24.GetAttribute("class");

            if (firstText == "Item 4" && firstStyle.StartsWith("ui-state-highlight") && secondText == "Item 2" && secondStyle.StartsWith("ui-state-default")) ok = true;

            driver.SwitchTo().DefaultContent();
            return ok;
        } //


        public bool GridCountVerify()
        {
            bool ok = false;
            SelectIframe("Grid");
            Helper.FluidWait(5000, driver);

            var objs = driver.FindElements(By.XPath("//ul[@id='sortable']/li"));
            int count = 0;
            foreach (IWebElement elem in objs) if (elem.Displayed) count++;
            if (count == 12) ok = true;

            driver.SwitchTo().DefaultContent();
            return ok;
        } //


        public void GridDragDrop()
        {
            SelectIframe("Grid");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (dTwelvth.Displayed) return true; else return false; });
            var obj = dTwelvth;
            int xEnd = obj.Location.X;
            int yEnd = obj.Location.Y;

            var todo = new Actions(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (dSecond.Displayed) return true; else return false; });
            obj = dSecond;
            int xBegin = obj.Location.X;
            int yBegin = obj.Location.Y;
            todo.DragAndDropToOffset(obj, xEnd - xBegin + 12, yEnd - yBegin + 12).Perform();

            Helper.FluidWait(3000, driver);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (dFirst.Displayed) return true; else return false; });
            obj = dFirst;
            xEnd = obj.Location.X;
            yEnd = obj.Location.Y;

            todo = new Actions(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (dTenth.Displayed) return true; else return false; });
            obj = dTenth;
            xBegin = obj.Location.X;
            yBegin = obj.Location.Y;
            todo.DragAndDropToOffset(obj, xEnd - xBegin + 12, yEnd - yBegin -12).Perform();

            Helper.FluidWait(3000, driver);

            driver.SwitchTo().DefaultContent();
        } //


        public bool GridVerify()
        {
            bool ok = false;
            SelectIframe("Grid");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (dFirst.Displayed) return true; else return false; });
            string firstText = dFirst.Text;

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => { if (dTwelvth.Displayed) return true; else return false; });
            string lastText = dTwelvth.Text;

            if (firstText == "11" && lastText == "2") ok = true;

            driver.SwitchTo().DefaultContent();
            return ok;
        } //

    } // class SortablePart


} // namespace
 