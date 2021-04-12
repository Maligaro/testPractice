using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Linq;

namespace seleniumPractice
{
    public class PetNames
    {
        public ChromeDriver driver;
        private string validEmail = "example@email.com";


        public By emailInputLocator = By.Name("email");
        public By sendButtonLocator = By.Id("sendMe");
        public By emailResultTextLacator = By.ClassName("your-email");
        public By anotherEmailLinkLocator = By.Id("anotherEmail");
        public By boyRadioLocator = By.Id("boy");
        public By girlRadioLocator = By.Id("girl");
        public By resultTextLocator = By.ClassName("result-text");

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            driver = new ChromeDriver(options);
        }
        
        [Test]
        public void Site_SendFormWithEmail_Success()//todo
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");
            driver.FindElement(emailInputLocator).SendKeys(validEmail);
            driver.FindElement(sendButtonLocator).Click();
            Assert.False(driver.FindElement(sendButtonLocator).Displayed, "Форма не отправилась");//todo
        }

        [Test]
        public void Site_NameSendToCorrectEmail_EmailsAreEqual()//todo
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");
            var expectedEmail = validEmail;
            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(sendButtonLocator).Click();
            Assert.AreEqual(expectedEmail, driver.FindElement(emailResultTextLacator).Text, "Имя отправилось не на тот имеил");//todo
        }
        
        [Test]
        public void Site_ClickAnotherEmail_AnotherEmailLinkIsHiddenAndEmailInputIsEmpty()//todo
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");
            driver.FindElement(emailInputLocator).SendKeys(validEmail);
            driver.FindElement(sendButtonLocator).Click();
            driver.FindElement(anotherEmailLinkLocator).Click();
            Assert.Multiple(() =>
            {
                Assert.IsFalse(driver.FindElement(anotherEmailLinkLocator).Displayed, "Ссылка для сброса почты не исчезла");//todo
                Assert.AreEqual(string.Empty, driver.FindElement(emailInputLocator).Text, "Поле для ввода почты не очистилось");//todo
            });
            
        }

        [Test]
        public void Site_ClickFemaleRadio_FemaleTextIsShown()//todo
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");
            driver.FindElement(girlRadioLocator).Click();
            driver.FindElement(emailInputLocator).SendKeys(validEmail);
            driver.FindElement(sendButtonLocator).Click();
            Assert.AreEqual("Хорошо, мы пришлём имя для вашей девочки на e-mail:", driver.FindElement(resultTextLocator).Text, ""); //todo
        }
        
        [Test]
        public void Site_ClickMaleRadio_MaleTextIsShown()//todo
        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");
            driver.FindElement(boyRadioLocator).Click();
            driver.FindElement(emailInputLocator).SendKeys(validEmail);
            driver.FindElement(sendButtonLocator).Click();
            Assert.AreEqual("Хорошо, мы пришлём имя для вашего мальчика на e-mail:", driver.FindElement(resultTextLocator).Text, ""); //todo
        }
        
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}