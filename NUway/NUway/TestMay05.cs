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
    [Description("Testing interactions - sortables")]
    public class TestSortables
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
        [Description("#33 Sortable: Default functionality")]
        [Order(21)]
        public void SortableDefault()
        {
            startpage.Goto();
            Assert.That(startpage.IsForm(false), Is.False);
            startpage.SelectMenu("Interactions", "Sortable");

            var testpage = new SortablePart(driver);
            testpage.SelectFunctionality("Default");
            testpage.DefaultDragDrop();
            Assert.That(testpage.DefaultVerify(), Is.True);
        } //


        [Test]
        [Description("#34 Sortable: Connect lists")]
        [Order(22)]
        public void SortableConnect()
        {
            startpage.Goto();
            Assert.That(startpage.IsForm(false), Is.False);
            startpage.SelectMenu("Interactions", "Sortable");

            var testpage = new SortablePart(driver);
            testpage.SelectFunctionality("Connect");
            testpage.ConnectDragDrop();
            Assert.That(testpage.ConnectVerify(), Is.True);
        } //


        [Test]
        [Description("#35 Sortable: Grid count")]
        [Order(23)]
        public void SortableGridcount()
        {
            startpage.Goto();
            Assert.That(startpage.IsForm(false), Is.False);
            startpage.SelectMenu("Interactions", "Sortable");

            var testpage = new SortablePart(driver);
            testpage.SelectFunctionality("Grid");
            Assert.That(testpage.GridCountVerify(), Is.True);
        } //


        [Test]
        [Description("#36 Sortable: Display as grid")]
        [Order(24)]
        public void SortableAsgrid()
        {
            startpage.Goto();
            Assert.That(startpage.IsForm(false), Is.False);
            startpage.SelectMenu("Interactions", "Sortable");

            var testpage = new SortablePart(driver);
            testpage.SelectFunctionality("Grid");
            testpage.GridDragDrop();
            Assert.That(testpage.GridVerify(), Is.True);
        } //


    } // class TestSortables


} // namespace
