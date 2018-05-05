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
    [Description("Login tests")]
    public class TestLogin
    {
        internal FirefoxDriver driver = null;

        [SetUp]
        public void StartDriver()
        {
            driver = new FirefoxDriver();
            driver.Manage().Window.Size = new System.Drawing.Size(1280, 720);
        } //


        [TearDown]
        public void StopDriver()
        {
            driver.Quit();
        } //


        [Test]
        [Description("#27 Login with valid credentials")]
        [Order(1)]
        public void LoginValid()
        {
            var part = new LoginPart(driver);
            Assert.That(part.Navigate(), Is.True);

            if (part.IsForm(true) == true)
            {
                part.OperateForm(Helper.username, Helper.password);
                Assert.That(part.IsForm(false), Is.False);
            }
            else
            {
                Assert.Fail("Problem with login Form");
            }
        } //


        [Test]
        [Description("#28 Login with invalid username")]
        [Order(2)]
        public void LoginBadName()
        {
            var part = new LoginPart(driver);
            Assert.That(part.Navigate(), Is.True);

            if (part.IsForm(true) == true)
            {
                part.OperateForm("+1234", Helper.password);
                Assert.That(part.IsErrorMessage(), Is.True);
            }
            else
            {
                Assert.Fail("Problem with login Form");
            }
        } //


        [Test]
        [Description("#29 Login with invalid password")]
        [Order(3)]
        public void LoginBadPassword()
        {
            var part = new LoginPart(driver);
            Assert.That(part.Navigate(), Is.True);

            if (part.IsForm(true) == true)
            {
                part.OperateForm(Helper.username, "+1234");
                Assert.That(part.IsErrorMessage(), Is.True);
            }
            else
            {
                Assert.Fail("Problem with login Form");
            }
        } //


    } // class TestMay03


} // namespace NUway
