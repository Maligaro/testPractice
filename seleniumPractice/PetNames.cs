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


        public By EmailInputLocator = By.Name("email");
        public By SendButtonLocator = By.Id("sendMe");
        public By ResultEmailTextLacator = By.ClassName("your-email");
        public By AnotherEmailLinkLocator = By.Id("anotherEmail");
        public By BoyRadioLocator = By.Id("boy");
        public By GirlRadioLocator = By.Id("girl");
        public By ResultTextLocator = By.ClassName("result-text");

        public void InputValidEmailAndSend()
        {
            driver.FindElement(EmailInputLocator).SendKeys(validEmail);
            driver.FindElement(SendButtonLocator).Click();
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
        public void PetNameGenerator_InputValidEmail_EmailsAreEqualAndSendButtonIsHidden()
        {
            InputValidEmailAndSend();
            Assert.Multiple(() =>
            {
                Assert.AreEqual(validEmail, driver.FindElement(ResultEmailTextLacator).Text,
                    "Письмо отправлено не на тот email");
                Assert.False(driver.FindElement(SendButtonLocator).Displayed,
                    "Кнопка отправить не была скрыта после отправки формы");
            });
        }

        [Test]
        public void PetNameGenerator_ClickAnotherEmail_AllResultTextIsHiddenAndEmailInputIsEmpty()
        {
            InputValidEmailAndSend();
            driver.FindElement(AnotherEmailLinkLocator).Click();
            Assert.Multiple(() =>
            {
                Assert.IsFalse(driver.FindElement(AnotherEmailLinkLocator).Displayed, "Ссылка \"указать другой e-mail\" не исчезла после нажатия");
                Assert.AreEqual(0, driver.FindElements(ResultTextLocator).Count, "Ссылка \"указать другой e-mail\" не исчезла после нажатия");
                Assert.AreEqual(0, driver.FindElements(ResultEmailTextLacator).Count, "Текст с email не скрыт не исчезла после нажатия");
                Assert.AreEqual(string.Empty, driver.FindElement(EmailInputLocator).Text, "Поле для ввода почты не очистилось после нажатия на \"указать другой e-mail\"");
            });
        }

        [Test]
        public void PetNameGenerator_ClickFemaleRadio_FemaleResultTextIsShown()
        {
            driver.FindElement(GirlRadioLocator).Click();
            InputValidEmailAndSend();
            Assert.AreEqual("Хорошо, мы пришлём имя для вашей девочки на e-mail:",
                driver.FindElement(ResultTextLocator).Text, "Неверное сообщение если выбрать женский вариант имени");
        }

        [Test]
        public void PetNameGenerator_ClickMaleRadio_MaleResultTextIsShown()
        {
            driver.FindElement(BoyRadioLocator).Click();
            InputValidEmailAndSend();
            Assert.AreEqual("Хорошо, мы пришлём имя для вашего мальчика на e-mail:",
                driver.FindElement(ResultTextLocator).Text, "Неверное сообщение если выбрать мужской вариант имени");
        }

        [Test]
        public void PetNameGenerator_InputDifferentValidEmails_Success()
        {
            Assert.Multiple(() =>
            {
                foreach (var email in new[]
                {
                    "email@example.com",
                    "eMaiL@example.com",
                    "ачкак@example.com",
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
                })
                {
                    if (!driver.FindElement(SendButtonLocator).Displayed)
                        driver.FindElement(AnotherEmailLinkLocator).Click();
                    driver.FindElement(EmailInputLocator).Clear();
                    driver.FindElement(EmailInputLocator).SendKeys(email);
                    driver.FindElement(SendButtonLocator).Click();
                    Assert.IsFalse(driver.FindElement(SendButtonLocator).Displayed,
                        "Отклонён валидный email: " + email);
                }
            });
        }

        [Test]
        public void PetNameGenerator_InputDifferentInvalidEmails_Failure()
        {
            Assert.Multiple(() =>
            {
                foreach (var email in new[]
                {
                    "plainaddress",
                    "#@%^%#$@#$@#.com",
                    "@example.com",
                    "@.",
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
                    //720 symbols
                    "longEmailThatProbablySouldntBeAllowedqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq@email.com",
                })
                {
                    if (!driver.FindElement(SendButtonLocator).Displayed)
                        driver.FindElement(AnotherEmailLinkLocator).Click();
                    driver.FindElement(EmailInputLocator).Clear();
                    driver.FindElement(EmailInputLocator).SendKeys(email);
                    driver.FindElement(SendButtonLocator).Click();
                    Assert.IsTrue(driver.FindElement(SendButtonLocator).Displayed, "Принят невалидный email: " + email);
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