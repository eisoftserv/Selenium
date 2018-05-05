using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUway
{
    [TestFixture]
    [SingleThreaded]
    [Description("Testing interactions - selectables")]
    public class TestSelectables
    {
        internal FirefoxDriver driver = null;
        internal LoginPart startpage = null;

        [OneTimeSetUp]
        [Description("logging in")]
        public void LoggingIn()
        {
            driver = new FirefoxDriver();
            driver.Manage().Window.Size = new System.Drawing.Size(1280, 720);

            startpage = new LoginPart(driver);
            startpage.Navigate();
            startpage.OperateForm(Helper.username, Helper.password);
        } // LoggingIn

        
        [OneTimeTearDown]
        public void StopDriver()
        {
            driver.Quit();
        } // 


        [Test]
        [Description("#30 Selectable: Default functionality")]
        [Order(11)]
        public void SelectableDefault()
        {
            startpage.Goto();
            Assert.That(startpage.IsForm(false), Is.False);
            startpage.SelectMenu("Interactions", "Selectable");

            var testpage = new SelectablePart(driver);
            testpage.SelectFunctionality("Default");
            testpage.SelectItem("Default");
            Assert.That(testpage.VerifyItem("Default"), Is.True);
        } //


        [Test]
        [Description("#31 Selectable: Grid")]
        [Order(12)]
        public void SelectableGrid()
        {
            startpage.Goto();
            Assert.That(startpage.IsForm(false), Is.False);
            startpage.SelectMenu("Interactions", "Selectable");

            var testpage = new SelectablePart(driver);
            testpage.SelectFunctionality("Grid");
            testpage.SelectItem("Grid");
            Assert.That(testpage.VerifyItem("Grid"), Is.True);
        } //


        [Test]
        [Description("#32 Selectable: Serialize")]
        [Order(13)]
        public void SelectableSerialize()
        {
            startpage.Goto();
            Assert.That(startpage.IsForm(false), Is.False);
            startpage.SelectMenu("Interactions", "Selectable");

            var testpage = new SelectablePart(driver);
            testpage.SelectFunctionality("Serial");
            testpage.SelectItem("Serial");
            Assert.That(testpage.VerifyItem("Serial") && testpage.VerifySerialMessage(), Is.True);
        } //


    } // class ShoPom1


} //
