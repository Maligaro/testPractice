using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Linq;

namespace seleniumPractice
{
    [TestFixture]
    public class PetNames
    {
        public ChromeDriver driver;
        public string validEmail = "example@email.com";


        public By emailInputLocator = By.Name("email");
        public By sendButtonLocator = By.Id("sendMe");
        public By emailResultTextLacator = By.ClassName("your-email");
        public By anotherEmailLinkLocator = By.Id("anotherEmail");
        public By boyRadioLocator = By.Id("boy");
        public By girlRadioLocator = By.Id("girl");
        public By resultTextLocator = By.ClassName("result-text");
        
        public void InputValidEmailAndSend()
        {
            driver.FindElement(emailInputLocator).SendKeys(validEmail);
            driver.FindElement(sendButtonLocator).Click();
        }
        
        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");
        }

        [Test]
        public void Site_InputValidEmail_EmailsAreEqualAndSendButtonIsHidden()
        {
            InputValidEmailAndSend();
            Assert.Multiple(() =>
            {
                Assert.AreEqual(validEmail, driver.FindElement(emailResultTextLacator).Text, "Письмо отправлено не на тот email");
                Assert.False(driver.FindElement(sendButtonLocator).Displayed, "Кнопка отправить не была скрыта после отправки формы");
            });
        }
        
        [Test]
        public void Site_ClickAnotherEmail_AnotherEmailLinkIsHiddenAndEmailInputIsEmpty()
        {
            InputValidEmailAndSend();
            driver.FindElement(anotherEmailLinkLocator).Click();
            Assert.Multiple(() =>
            {
                Assert.IsFalse(driver.FindElement(anotherEmailLinkLocator).Displayed, "Ссылка \"указать другой e-mail\" не исчезла после нажатия");
                Assert.AreEqual(string.Empty, driver.FindElement(emailInputLocator).Text, "Поле для ввода почты не очистилось после нажатия на \"указать другой e-mail\"");
            });
            
        }

        [Test]
        public void Site_ClickFemaleRadio_FemaleResultTextIsShown()
        {
            driver.FindElement(girlRadioLocator).Click();
            InputValidEmailAndSend();
            Assert.AreEqual("Хорошо, мы пришлём имя для вашей девочки на e-mail:", driver.FindElement(resultTextLocator).Text, "Неверное сообщение если выбрать женский вариант имени"); 
        }
        
        [Test]
        public void Site_ClickMaleRadio_MaleResultTextIsShown()
        {
            driver.FindElement(boyRadioLocator).Click();
            InputValidEmailAndSend();
            Assert.AreEqual("Хорошо, мы пришлём имя для вашего мальчика на e-mail:", driver.FindElement(resultTextLocator).Text, "Неверное сообщение если выбрать мужской вариант имени"); 
        }

        [Test]
        public void Site_InputDifferentValidEmails_Success()
        {
            Assert.Multiple(() =>
            {
                foreach (var email in new[]
                {
                   "email@example.com",
                   "firstname.lastname@example.com",
                   "email@subdomain.example.com",
                   "firstname+lastname@example.com",
                   "email@123.123.123.123",
                   "email@[123.123.123.123]",
                   "\"email\"@example.com",
                   "1234567890@example.com",
                   "email@example-one.com",
                   "_______@example.com",
                   "email@example.name",
                   "email@example.museum",
                   "email@example.co.jp",
                   "firstname-lastname@example.com",
                   "much.”more\\ unusual”@example.com",
                   "very.unusual.”@”.unusual.com@example.com",
                   "very.”(),:;<>[]”.VERY.”very@\\\\ \"very”.unusual@strange.example.com",
                   //255 symbols
                   "longEmailThatSouldBeAllowedqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq@email.com",
                })
                {
                    if (!driver.FindElement(sendButtonLocator).Displayed)
                        driver.FindElement(anotherEmailLinkLocator).Click();
                    driver.FindElement(emailInputLocator).Clear();
                    driver.FindElement(emailInputLocator).SendKeys(email);
                    driver.FindElement(sendButtonLocator).Click();
                    Assert.IsFalse(driver.FindElement(sendButtonLocator).Displayed, "Отклонён валидный email: " + email);
                }
            });
        }
        
        [Test]
        public void Site_InputDifferentInvalidEmails_Failure()
        {
            Assert.Multiple(() =>
            {
                foreach (var email in new String[]
                {
                   "plainaddress",
                   "#@%^%#$@#$@#.com",
                   "@example.com",
                   "Joe Smith <email@example.com>",
                   "email.example.com",
                   "email@example@example.com",
                   ".email@example.com",
                   "email.@example.com",
                   "email..email@example.com",
                   "あいうえお@example.com",
                   "email@example.com (Joe Smith)",
                   "email@example",
                   "email@-example.com",
                   "email@example.web",
                   "email@111.222.333.44444",
                   "email@example..com",
                   "Abc..123@example.com",
                   "”(),:;<>[\\]@example.com",
                   "just”not”right@example.com",
                   "this\\ is\"really\"not\\allowed@example.com",
                   //256 symbols
                   "longEmailThatSouldBeAllowedqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq@email.com",

                })
                {
                    if (!driver.FindElement(sendButtonLocator).Displayed)
                        driver.FindElement(anotherEmailLinkLocator).Click();
                    driver.FindElement(emailInputLocator).Clear();
                    driver.FindElement(emailInputLocator).SendKeys(email);
                    driver.FindElement(sendButtonLocator).Click();
                    Assert.IsTrue(driver.FindElement(sendButtonLocator).Displayed, "Принят невалидный  email: " + email);
                }
            });
        }
        
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}