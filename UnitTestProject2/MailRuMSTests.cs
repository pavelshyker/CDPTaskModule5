using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.Configuration;
using System.Drawing;
using System.Runtime.Remoting.Channels;
using System.Linq;

namespace TestWebProject
{
    [TestClass]
    public class Test
    {
        private IWebDriver driver;

        private string baseUrl;
        private string emailAddress = "litmarsd@mail.ru";
        private string emailSubject = "Subject";
        private string emailText = "EmailTestText";

        private By signInFrameLocator = By.ClassName("ag-popup__frame__layout__iframe");
        private By textFieldBodyFrameLocator = By.XPath("//iframe[contains(@id, 'toolkit')]");

        private By signUpLinkLocator = By.XPath("//a[@id='PH_authLink']");
        private By loginInputFieldLocator = By.XPath("//input[@name='Login']");
        private By passwordInputFieldLocator = By.XPath("//input[@name='Password']");
        private By toFieldLocator = By.XPath("//textarea[@data-original-name='To']");
        private By subjectFieldLocator = By.XPath("//input[@class='b-input']");
        private By emailBodyLocator = By.XPath("//div[contains(@id, 'BODY')]");
        private By toFieldElementLocator = By.XPath("//div[@data-blockid='head']//span[@data-text]");
        private By textFieldLocator = By.XPath(".//body[@id='tinymce']");

        private By submitButtonLocator = By.XPath("//button[@type='submit']");
        private By newEmailButtonLocator = By.XPath("//div[@class = 'b-sticky']//a[@data-name='compose']");
        private By newEmailButton2OptionsLocator = By.XPath("//div[@class = 'b-sticky']//a[@data-name='compose'] | //div[@class = 'b-sticky js-not-sticky']//a[@data-name='compose']");
        private By logoutButtonLocator = By.XPath("//a[@id='PH_logoutLink']");
        private By deleteButtonLocator = By.XPath("//div[@class = 'b-sticky js-not-sticky']//div[@data-cache-key='500001_undefined_false']//div[@data-shortcut-title='Del']");
        private By sendButtonLocator = By.XPath("//div[@class='b-sticky js-not-sticky']//*[@data-name='send']");
        private By sendButton2OptionsLocator = By.XPath("//div[@class='b-sticky js-not-sticky']//*[@data-name='send'] | //div[@class='b-sticky']//*[@data-name='send']");
        private By deleteAllOptionDraftLocator = By.XPath("//a[@data-name = 'all']");
        private By checkBoxSelectAllDraftLocator = By.XPath("//div[@class = 'b-sticky js-not-sticky']//div[@data-cache-key != '0_undefined_false']//div[contains(@class, 'selectAll')]//div[contains(@class, 'b-checkbox__box')]");
        private By checkBoxSave2OptionsLocator = By.XPath(".//div[@class = 'b-sticky']//div[@data-name = 'saveDraft'] | .//div[@class = 'b-sticky js-not-sticky']//div[@data-name = 'saveDraft']");
        //private By checkBoxSelectAllSendLocator1 = By.XPath("//div[@class = 'b-sticky']//div[@data-cache-key = '500000_undefined_false']//div[contains(@class, 'selectAll')]//div[contains(@class, 'b-checkbox__box')]");
        //private By deleteAllOptionSendLocator1 = By.XPath("//div[@class = 'b-sticky']//div[@data-cache-key='500000_undefined_false']//div[@data-shortcut-title='Del']");
        //private By checkBoxSelectAllSendLocator2 = By.XPath("//div[@class = 'b-sticky js-not-sticky']//div[@data-cache-key = '500000_undefined_false']//div[contains(@class, 'selectAll')]//div[contains(@class, 'b-checkbox__box')]");
        //private By deleteAllOptionSendLocator2 = By.XPath("//div[@class = 'b-sticky js-not-sticky']//div[@data-cache-key='500000_undefined_false']//div[@data-shortcut-title='Del']");
        private By checkBoxSelectAllSend2OptionsLocator = By.XPath("//div[@class = 'b-sticky']//div[@data-cache-key = '500000_undefined_false']//div[contains(@class, 'selectAll')]//div[contains(@class, 'b-checkbox__box')] | //div[@class = 'b-sticky js-not-sticky']//div[@data-cache-key = '500000_undefined_false']//div[contains(@class, 'selectAll')]//div[contains(@class, 'b-checkbox__box')]");
        private By deleteAllOptionSend2OptionsLocator = By.XPath("//div[@class = 'b-sticky']//div[@data-cache-key='500000_undefined_false']//div[@data-shortcut-title='Del'] | //div[@class = 'b-sticky js-not-sticky']//div[@data-cache-key='500000_undefined_false']//div[@data-shortcut-title='Del']");


        private By draftFolderLocator = By.XPath("//a[@data-mnemo= 'drafts']");
        private By sendFolderLocator = By.XPath("//i[contains(@class, 'ico ico_folder_send')]");
       
        private By draftEmailItemsLocator = By.XPath(".//a[contains(@href, 'drafts') and contains(@class, 'item')]//div[contains(@class, 'addr')]");
        private By sendEmailItemsLocator = By.XPath(" //div[@data-cache-key='500000_undefined_false']//div[@class='b-datalist__item__addr']");
        private By emptyBlockLocator = By.ClassName("b-datalist__empty__block");


        [TestInitialize]
        public void SetupTest()
        {
            this.driver = new ChromeDriver(@"C:\Users\Pavel_Shyker\Downloads\chromedriver_win32");
            this.baseUrl = "https://mail.ru";
            this.driver.Navigate().GoToUrl(this.baseUrl);
            this.driver.Manage().Window.Maximize();
        }

        [TestMethod, TestCategory("Login")]
        public void LoginTest()
        {
            Login();
            Assert.IsTrue(this.driver.FindElement(newEmailButtonLocator).Displayed);
        }

        [TestMethod, TestCategory("EmailCreating")]
        public void CreateDraftEmailTest()
        {
            Login();
            CreateADraft();
            this.driver.FindElement(draftFolderLocator).Click();
            Thread.Sleep(5000);
            var element = driver.FindElements(draftEmailItemsLocator);
            string emailAddressDraft = element.First().Text;
            Assert.AreEqual(emailAddress, emailAddressDraft);
        }

        [TestMethod, TestCategory("VerificationOfTheDraftContent")]
        public void CompareDraftEmailAddressTest()
        {
            Login();
            CreateADraft();
            this.driver.FindElement(draftFolderLocator).Click();
            IsElementVisible(draftEmailItemsLocator);
            var elements = this.driver.FindElements(draftEmailItemsLocator);
            elements.First().Click();
            IsElementVisible(toFieldElementLocator);
            string emailAttr = this.driver.FindElement(toFieldElementLocator).GetAttribute("data-text");
            Assert.AreEqual(emailAddress, emailAttr);
        }

        [TestMethod, TestCategory("VerificationOfTheDraftContent")]
        public void CompareDraftEmailSubjectTest()
        {
            Login();
            CreateADraft();
            this.driver.FindElement(draftFolderLocator).Click();
            IsElementVisible(draftEmailItemsLocator);
            var elements = this.driver.FindElements(draftEmailItemsLocator);
            elements.First().Click();
            IsElementVisible(toFieldElementLocator);
            string subject = this.driver.FindElement(subjectFieldLocator).GetAttribute("name");
            Assert.AreEqual(emailSubject, subject);
        }

        [TestMethod, TestCategory("VerificationOfTheDraftContent")]
        public void CompareDraftEmailTextTest()
        {
            Login();
            CreateADraft();
            this.driver.FindElement(draftFolderLocator).Click();
            IsElementVisible(draftEmailItemsLocator);
            var elements = this.driver.FindElements(draftEmailItemsLocator);
            elements.First().Click();
            IWebElement frameEmailBody = this.driver.FindElement(textFieldBodyFrameLocator);
            this.driver.SwitchTo().Frame(frameEmailBody);
            var element = this.driver.FindElement(emailBodyLocator).Text;
            Assert.IsTrue(element.Contains(emailText));
        }

        [TestMethod, TestCategory("EmailSending")]
        public void DraftFolderAfterSendingTest()
        {
            Login();
            DeleteAllDraft();
            CreateADraft();
            this.driver.FindElement(draftFolderLocator).Click();
            var elements = this.driver.FindElements(draftEmailItemsLocator);
            elements.First().Click();
            Thread.Sleep(2000);
            this.driver.FindElement(sendButton2OptionsLocator).Click();
            this.driver.FindElement(draftFolderLocator).Click();
            Thread.Sleep(5000);
            var elementsDraft = this.driver.FindElements(draftEmailItemsLocator);
            Thread.Sleep(5000);
            bool isAnyDraft = elementsDraft.Any();
            Thread.Sleep(5000);
            Assert.IsFalse(isAnyDraft);
        }

        [TestMethod, TestCategory("EmailSending")]
        public void SendFolderAfterSendingTest()
        {
            Login();
            DeleteAllSent();
            CreateADraft();
            this.driver.FindElement(draftFolderLocator).Click();
            Thread.Sleep(2000);
            var elements = this.driver.FindElements(draftEmailItemsLocator);
            elements.First().Click();
            Thread.Sleep(2000);
            this.driver.FindElement(sendButton2OptionsLocator).Click();
            this.driver.FindElement(sendFolderLocator).Click();
            IsElementVisible(sendEmailItemsLocator);
            var elementsSent = this.driver.FindElements(sendEmailItemsLocator);
            bool isAnySend = elementsSent.Any();
            Thread.Sleep(2000);
            Assert.IsTrue(isAnySend);
        }

        [TestMethod, TestCategory("Logout")]
        public void LogoutTest()
        {
            Login();
            this.driver.FindElement(logoutButtonLocator).Click();
            Assert.IsTrue(this.driver.FindElement(signUpLinkLocator).Displayed);
        }

        public void IsElementVisible(By element, int timeoutSecs = 10)
        {
            var a = new WebDriverWait(this.driver, TimeSpan.FromSeconds(timeoutSecs)).Until(ExpectedConditions.ElementIsVisible(element));
        }

        public void Login()
        {
            this.driver.FindElement(signUpLinkLocator).Click();
            IWebElement frame1 = this.driver.FindElement(signInFrameLocator);
            this.driver.SwitchTo().Frame(frame1);
            Thread.Sleep(5000);
            this.driver.FindElement(loginInputFieldLocator).Click();
            this.driver.FindElement(loginInputFieldLocator).SendKeys("testuser.19");
            this.driver.FindElement(passwordInputFieldLocator).Click();
            this.driver.FindElement(passwordInputFieldLocator).SendKeys("testCDP123");
            this.driver.FindElement(submitButtonLocator).Click();
        }

        //[TestMethod, TestCategory("Deleting")]
        //public void DraftDeletingTest()
        //{
        //    Login();
        //    Thread.Sleep(2000);
        //    Assert.IsTrue(DeleteAllDraft());
        //}

        public bool DeleteAllDraft()
        {
            this.driver.FindElement(draftFolderLocator).Click();

            Thread.Sleep(2000);
            var elements = this.driver.FindElements(draftEmailItemsLocator);
            if (elements.Any())
            {         
               var elem = this.driver.FindElements(checkBoxSelectAllDraftLocator);
               elem.First().Click();
               this.driver.FindElement(deleteButtonLocator).Click();
               Thread.Sleep(2000);
            }
            // return this.driver.FindElements(draftEmailItemsLocator).Count().Equals(0);
            bool draftEmpty = this.driver.FindElement(emptyBlockLocator).Displayed;
            return draftEmpty;
        }

        public bool DeleteAllSent()
        {
            this.driver.FindElement(sendFolderLocator).Click();
            Thread.Sleep(2000);
            var elements = this.driver.FindElements(sendEmailItemsLocator);

            if (elements.Any())
            {
                this.driver.FindElement(checkBoxSelectAllSend2OptionsLocator).Click();
                this.driver.FindElement(deleteAllOptionSend2OptionsLocator).Click();
                Thread.Sleep(2000);
            }

            bool sentEmpty = this.driver.FindElement(emptyBlockLocator).Displayed;
            return sentEmpty;
        }

        public void CreateADraft()
        {
            Thread.Sleep(2000);
            this.driver.FindElement(newEmailButton2OptionsLocator).Click();
            this.driver.FindElement(toFieldLocator).SendKeys(emailAddress);
            this.driver.FindElement(subjectFieldLocator ).SendKeys(emailSubject);
            IWebElement frame2 = this.driver.FindElement(textFieldBodyFrameLocator);
            this.driver.SwitchTo().Frame(frame2);
            IWebElement messageFieldElement = this.driver.FindElement(textFieldLocator);
            messageFieldElement.SendKeys(emailText);
            this.driver.SwitchTo().DefaultContent();
            Thread.Sleep(2000);
            this.driver.FindElement(checkBoxSave2OptionsLocator).Click();
            Thread.Sleep(2000);
        }


        [TestCleanup]
        public void CleanUp()
        {
            this.driver.Close();
            this.driver.Quit();
        }
    }
}
