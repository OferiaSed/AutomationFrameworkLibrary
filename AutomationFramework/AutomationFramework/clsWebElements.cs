using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutomationFramework
{
    public class clsWebElements
    {
        public static DefaultWait<IWebDriver> objFluentWait;
        public static WebDriverWait objExplicitWait;
        public static string strAction = "";


        public IList<IWebElement> fnGetWeList(string pstrLocator)
        {
            try
            {
                IList<IWebElement> pobjElement = clsWebBrowser.objDriver.FindElements(By.XPath(pstrLocator));
                return pobjElement;
            }
            catch (Exception pobjException)
            {
                fnExceptionHandling(pobjException, "Ilist<WebElement>: " + pstrLocator + " doesn't exist", true);
                return null;
            }
        }

        public IWebElement fnGetWe(string pstrLocator)
        {
            try
            {
                IWebElement pobjElement = clsWebBrowser.objDriver.FindElement(By.XPath(pstrLocator));
                return pobjElement;
            }
            catch (Exception pobjException)
            {
                fnExceptionHandling(pobjException, "WebElement: " + pstrLocator + " doesn't exist", true);
                return null;
            }
        }

        public object fnGetFluentWait(IWebElement pobjWebElement, string pstrAction, string pstrTextEnter = "")
        {
            objFluentWait = new DefaultWait<IWebDriver>(clsWebBrowser.objDriver);
            objFluentWait.Timeout = TimeSpan.FromSeconds(10);

            objFluentWait.PollingInterval = TimeSpan.FromMilliseconds(250);
            objFluentWait.IgnoreExceptionTypes(typeof(WebDriverTimeoutException), typeof(SuccessException));

            switch (pstrAction)
            {
                case "Displayed":
                    objFluentWait.Until(x => pobjWebElement.Displayed);
                    break;
                case "Click":
                    objFluentWait.Until(x => pobjWebElement).Click();
                    break;
                case "SendKeys":
                    objFluentWait.Until(x => pobjWebElement).SendKeys(pstrTextEnter);
                    break;
                case "Clear":
                    objFluentWait.Until(x => pobjWebElement).Clear();
                    break;
            }
            return objFluentWait;
        }

        public bool fnGetExplicitWait(string pstrLocator, string pstrOption)
        {
            bool pblStatus = false;
            IWebElement objWebElement;
            try
            {
                objExplicitWait = new WebDriverWait(clsWebBrowser.objDriver, TimeSpan.FromSeconds(10));
                objExplicitWait.IgnoreExceptionTypes(typeof(WebDriverTimeoutException));

                switch (pstrOption)
                {
                    case "ElementExists":
                        objWebElement = objExplicitWait.Until(ExpectedConditions.ElementExists(By.XPath(pstrLocator)));
                        pblStatus = true;
                        break;
                    case "Elementvisible":
                        objWebElement = objExplicitWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(pstrLocator)));
                        pblStatus = true;
                        break;
                }
                return pblStatus;

            }
            catch (Exception pobjException)
            {
                fnExceptionHandling(pobjException);
                return pblStatus;
            }
        }

        public bool fnPageLoad(IWebElement pobjWebElement, string pstrPage, bool pblScreenShot = true, bool pblHardStop = false, string pstrHardStopMsg = "PageLoad Failed and HardStop defined")
        {
            clsReportResult clsRR = new clsReportResult();

            bool blResult = false;
            try
            {
                //TestContext.Progress.WriteLine($"Step - PageLoad in Page: {pstrPage} - Info");
                clsReportResult.fnLog("PageLoad", "Step - PageLoad in Page: " + pstrPage, Status.Info, false);

                strAction = "Displayed";
                IJavaScriptExecutor objJS = (IJavaScriptExecutor)clsWebBrowser.objDriver;
                clsWebBrowser.wait.Until(wd => objJS.ExecuteScript("return document.readyState").ToString() == "complete");

                fnGetFluentWait(pobjWebElement, strAction);
                //TestContext.Progress.WriteLine($"The Page is loaded for the Page: {pstrPage} - Pass");
                clsReportResult.fnLog("PageLoadPass", "The Page is loaded for the Page: " + pstrPage, Status.Pass, pblScreenShot);
                blResult = true;
            }
            catch (Exception pobjException)
            {
                //TestContext.Progress.WriteLine($"The Page is not loaded for the Page: : {pstrPage} - Fail");
                clsReportResult.fnLog("PageLoadFail", "The Page is not loaded for the Page: " + pstrPage, Status.Fail, true, pblHardStop, pstrHardStopMsg);
                fnExceptionHandling(pobjException);
            }
            return blResult;
        }

        public bool fnSendKeys(IWebElement pobjWebElement, string pstrField, string pstrTextEnter, bool pblScreenShot = true, bool pblHardStop = false, string pstrHardStopMsg = "SendKeys Failed and HardStop defined")
        {
            clsReportResult clsRR = new clsReportResult();

            bool blResult = false;

            try
            {
                clsReportResult.fnLog("SendKeys", "Step - Sendkeys: " + pstrTextEnter + " to field: " + pstrField, Status.Info, false);
                strAction = "SendKeys";
                fnGetFluentWait(pobjWebElement, strAction, pstrTextEnter);
                clsReportResult.fnLog("SendKeysPass", "The SendKeys for: " + pstrField + " with value: " + pstrTextEnter + " was done successfully.", Status.Pass, pblScreenShot, pblHardStop);
                blResult = true;
            }
            catch (Exception pobjException)
            {
                clsReportResult.fnLog("SendKeysFail", "The SendKeys for: " + pstrField + " with value: " + pstrTextEnter + " has failed.", Status.Fail, true, pblHardStop, pstrHardStopMsg);
                fnExceptionHandling(pobjException);
            }
            return blResult;
        }

        public bool fnClick(IWebElement pobjWebElement, string pstrElement, bool pblScreenShot = true, bool pblHardStop = false, string pstrHardStopMsg = "Click Failed and HardStop defined")
        {
            bool blResult = false;
            try
            {
                //TestContext.Progress.WriteLine($"Step - Click on: {pstrElement} - Info");
                clsReportResult.fnLog("Click", "Step - Click on " + pstrElement, Status.Info, false);

                strAction = "Click";
                fnGetFluentWait(pobjWebElement, strAction);
                //TestContext.Progress.WriteLine($"The click to the element is not working for: {pstrElement} - Pass");
                clsReportResult.fnLog("ClickPass", "Click on " + pstrElement + " was done successfully.", Status.Pass, pblScreenShot);
                blResult = true;
            }
            catch (Exception pobjException)
            {
                //TestContext.Progress.WriteLine($"The click to the element is not working for: {pstrElement} - Fail");
                clsReportResult.fnLog("ClickFail", "The click to the element is not working for: " + pstrElement, Status.Fail, true, pblHardStop, pstrHardStopMsg);
                fnExceptionHandling(pobjException);
            }
            return blResult;
        }

        public bool fnDoubleClick(IWebElement pobjWebElement, string pstrElement, bool pblScreenShot = true, bool pblHardStop = false, string pstrHardStopMsg = "DoubleClick Failed and HardStop defined")
        {
            bool blResult = false;
            try
            {
                //TestContext.Progress.WriteLine($"Step - Double Clic on {pstrElement} - Info");
                clsReportResult.fnLog("DoubleClick", "Step - Double Clic on " + pstrElement, Status.Info, false);

                strAction = "Displayed";
                fnGetFluentWait(pobjWebElement, strAction);
                Actions actions = new Actions(clsWebBrowser.objDriver);
                actions.DoubleClick(pobjWebElement).Perform();
                //TestContext.Progress.WriteLine($"Double Click on {pstrElement} was done successfully - Pass");
                clsReportResult.fnLog("DoubleClickPass", "Double Click on " + pstrElement + " was done successfully.", Status.Pass, pblScreenShot);
                blResult = true;
            }
            catch (Exception pobjException)
            {
                //TestContext.Progress.WriteLine($"Couldn't Double Click on {pstrElement} - Fail");
                clsReportResult.fnLog("DoubleClickPass", "Couldn't Double Click on " + pstrElement, Status.Fail, true, pblHardStop, pstrHardStopMsg);
                fnExceptionHandling(pobjException);
            }
            return blResult;

        }

        public string fnGetAttribute(IWebElement pobjWebElement, string pstrElement, string pstrAttName, bool pblScreenShot = true, bool pblHardStop = false, string pstrHardStopMsg = "GetAttribute Failed and HardStop defined")
        {
            string strAttributeContent = "";
            try
            {
                //TestContext.Progress.WriteLine($"Step - Get Attribue {pstrAttName} from {pstrElement} - Info");
                clsReportResult.fnLog("GetAttribute", "Step - Get Attribue " + pstrAttName + " from " + pstrElement, Status.Info, false);

                strAttributeContent = pobjWebElement.GetAttribute(pstrAttName);
                //TestContext.Progress.WriteLine($"Get Attribute {pstrAttName} from Element: {pstrElement} was done successfully - Pass");
                clsReportResult.fnLog("GetAttributePass", "Get Attribute " + pstrAttName + " from Element: " + pstrElement + "  was done successfully.", Status.Pass, pblScreenShot);

            }
            catch (Exception pobjException)
            {
                //TestContext.Progress.WriteLine($"Get Attribute {pstrAttName} from Element: {pstrElement} was not done. - Fail");
                clsReportResult.fnLog("GetAttributeFail", "Get Attribute " + pstrAttName + " from Element: " + pstrElement + " was not done.", Status.Fail, false);
                fnExceptionHandling(pobjException);
            }
            return strAttributeContent;
        }

        public bool fnClear(IWebElement pobjWebElement, string pstrField, bool pblScreenShot = true, bool pblHardStop = false, string pstrHardStopMsg = "Clear Failed and HardStop defined")
        {
            bool blResult = false;
            try
            {
                //TestContext.Progress.WriteLine($"Step - Clear to field: {pstrField} - Info");
                clsReportResult.fnLog("Clear", "Step - Clear to field: " + pstrField, Status.Info, false);

                strAction = "Clear";
                fnGetFluentWait(pobjWebElement, strAction);
                Thread.Sleep(3000);
                string testTXT = pobjWebElement.Text;
                if (pobjWebElement.Text.Equals("") || pobjWebElement.Text.Equals(null))
                {
                    //TestContext.Progress.WriteLine($"Clear to field {pstrField} was done successfully. - Pass");
                    clsReportResult.fnLog("ClearPass", "Clear to field" + pstrField + " was done successfully.", Status.Pass, pblScreenShot);
                    blResult = true;
                }
                else
                    throw new ArgumentException("ClearFail");
            }
            catch (Exception pobjException)
            {
                fnExceptionHandling(pobjException);
            }
            return blResult;
        }

        public bool fnContainsText(string pstrStepName, string pstrParentString, string pstrSubString, bool pblScreenShot = true, bool pblHardStop = false, string pstrHardStopMsg = "Contains Failed and HardStop defined")
        {
            bool blResult = false;
            try
            {
                //TestContext.Progress.WriteLine($"Step - Contains: {pstrSubString} in the string: {pstrParentString} - Info");
                clsReportResult.fnLog("Contains", "Step - Contains: " + pstrSubString + " in the string: " + pstrParentString, Status.Info, false);

                if (pstrParentString.Contains(pstrSubString))
                {
                    //TestContext.Progress.WriteLine($"Contains: {pstrStepName} - Pass");
                    clsReportResult.fnLog("ContainsTextPass", pstrStepName, Status.Pass, pblScreenShot);
                    blResult = true;
                }
                else
                    throw new ArgumentException("ContainsTextFail");
            }
            catch (Exception pobjException)
            {
                fnExceptionHandling(pobjException);
            }
            return blResult;
        }

        public bool fnElementExist(string pstrStepName, string pstrLocator, bool pblScreenShot = true, bool pblHardStop = false, string pstrHardStopMsg = "Element Exist Failed and HardStop defined")
        {
            bool blResult = false;
            try
            {
                bool blElementExist = fnGetExplicitWait(pstrLocator, "ElementExists");

                if (blElementExist)
                {
                    //TestContext.Progress.WriteLine($"The element {pstrStepName} exist in the page - Pass");
                    clsReportResult.fnLog("ElementExistPass", "The element " + pstrStepName + " exist in the page", Status.Pass, pblScreenShot);
                    blResult = true;
                }
                else
                    throw new ArgumentException("ElementExistFail");
            }
            catch (Exception pobjException)
            {
                fnExceptionHandling(pobjException);
            }
            return blResult;
        }

        public bool fnElementNotExist(string pstrStepName, string pstrLocator, bool pblScreenShot = true, bool pblHardStop = false, string pstrHardStopMsg = "Element Not Exist Failed and HardStop defined")
        {
            bool blResult = false;
            try
            {

                bool blElementExist = fnGetExplicitWait(pstrLocator, "ElementExists");

                if (!blElementExist)
                {
                    //TestContext.Progress.WriteLine($"The element {pstrStepName} not exist in the page - Pass");
                    clsReportResult.fnLog("ElementNotExistPass", "Element " + pstrStepName + " not exist in the page", Status.Pass, pblScreenShot);
                    blResult = true;
                }
                else
                    throw new ArgumentException("ElementNotExistFail");

            }
            catch (Exception pobjException)
            {
                fnExceptionHandling(pobjException);
            }
            return blResult;
        }

        public bool fnElementExistNoReport(string pstrStepName, string pstrLocator, bool pblScreenShot = true, bool pblHardStop = false, string pstrHardStopMsg = "Element Exist Failed and HardStop defined")
        {
            bool blResult = false;
            try
            {
                bool blElementExist = fnGetExplicitWait(pstrLocator, "ElementExists");

                if (blElementExist)
                {
                    //clsReportResult.fnLog("ElementExistPass", "Element " + pstrStepName + " exist in the page", "Pass", pblScreenShot);
                    blResult = true;
                }
                else
                    blResult = false;
                //throw new ArgumentException("ElementExistFail");
            }
            catch (Exception pobjException)
            {
                blResult = false;
                //fnExceptionHandling(pobjException);
            }
            return blResult;
        }


        public bool fnVerifyText(string pstrStepName, string pstrExpectedString, string pstrActualString, bool pblScreenShot = true, bool pblHardStop = false, string pstrHardStopMsg = "Verify Text Failed and HardStop defined")
        {
            bool blResult = false;
            try
            {
                //TestContext.Progress.WriteLine($"Step - Verify Text, Expected: {pstrExpectedString}, Actual: {pstrActualString} - Info");
                clsReportResult.fnLog("VerifyText", "Step - Verify Text, Expected: " + pstrExpectedString + ", Actual: " + pstrActualString, Status.Info, false);

                if (pstrExpectedString.Equals(pstrActualString))
                {
                    //TestContext.Progress.WriteLine($"{pstrStepName} - Pass");
                    clsReportResult.fnLog("VerifyTextPass", pstrStepName, Status.Pass, pblScreenShot);
                    blResult = true;
                }
                else
                    throw new ArgumentException("VerifyTextFail");
            }
            catch (Exception pobjException)
            {
                fnExceptionHandling(pobjException);
            }
            return blResult;
        }

        public bool fnSelectList(string pstrStepName, IWebElement pobjWebElement, string pstrValue, string pstrValueType = "value", bool pblScreenShot = true, bool pblHardStop = false, string pstrHardStopMsg = "Select List Failed and HardStop defined")
        {
            bool blResult = false;
            try
            {
                //TestContext.Progress.WriteLine($"Step - Select Dropdown: {pstrStepName} With Value: {pstrValue} - Info");
                clsReportResult.fnLog("SelectDropdown", "Step - Select Dropdown: " + pstrStepName + " With Value: " + pstrValue, Status.Info, false);

                var selectElement = new SelectElement(pobjWebElement);
                switch (pstrValueType.ToUpper())
                {
                    case "VALUE":
                        selectElement.SelectByValue(pstrValue);
                        break;
                    case "TEXT":
                        selectElement.SelectByText(pstrValue);
                        break;
                    case "INDEX":
                        selectElement.SelectByIndex(Int32.Parse(pstrValue));
                        break;
                }
                //TestContext.Progress.WriteLine($"{pstrStepName} - Pass");
                clsReportResult.fnLog("SelectListPass", pstrStepName, Status.Pass, pblScreenShot);
                blResult = true;

            }
            catch (Exception pobjException)
            {
                //TestContext.Progress.WriteLine($"{pstrStepName} - Fail");
                clsReportResult.fnLog("SelectListFail", pstrStepName, Status.Fail, true, pblHardStop, pstrHardStopMsg);
                fnExceptionHandling(pobjException);
            }
            return blResult;
        }

        public bool fnVerifySelectedItem(string pstrStepName, IWebElement pobjWebElement, string pstrValue, bool pblScreenShot = true, bool pblHardStop = false, string pstrHardStopMsg = "Selected List Failed and HardStop defined")
        {
            bool blResult = false;
            try
            {
                //TestContext.Progress.WriteLine($"Step - Selected Dropdown: {pstrStepName} With Value: {pstrValue} - Info");
                clsReportResult.fnLog("VerifySelectedDropdown", "Step - Selected Dropdown: " + pstrStepName + " With Value: " + pstrValue, Status.Info, false);
                string pstrSelectItem = new SelectElement(pobjWebElement).SelectedOption.GetAttribute("value");
                if (pstrValue == pstrSelectItem)
                {
                    //TestContext.Progress.WriteLine($"{pstrStepName} - Pass");
                    clsReportResult.fnLog("SelectedItemPass", pstrStepName, Status.Pass, pblScreenShot);
                    blResult = true;
                }
                else
                    throw new ArgumentException("VerifySelectedItemFail");
            }
            catch (Exception pobjException)
            {
                fnExceptionHandling(pobjException, pstrStepName, pblHardStop, pstrHardStopMsg);
            }
            return blResult;
        }

        public bool fnSelectCheckBox(string pstrStepName, IWebElement pobjWebElement, bool pblScreenShot = true, bool pblHardStop = false, string pstrHardStopMsg = "Select CheckBox Failed and HardStop defined")
        {
            bool blResult = false;
            try
            {
                //TestContext.Progress.WriteLine($"Step - {pstrStepName} - Info");
                clsReportResult.fnLog("SelectCheckBox", "Step - " + pstrStepName, Status.Info, false);
                strAction = "Click";
                fnGetFluentWait(pobjWebElement, strAction);
                Thread.Sleep(5000);
                //TestContext.Progress.WriteLine($"{pstrStepName} - Pass");
                clsReportResult.fnLog("SelectCheckBoxPass", pstrStepName, Status.Pass, pblScreenShot);
                blResult = true;
            }
            catch (Exception pobjException)
            {
                //TestContext.Progress.WriteLine($"{pstrStepName} - Fail");
                clsReportResult.fnLog("SelectCheckBoxFail", pstrStepName, Status.Fail, true, pblHardStop, pstrHardStopMsg);
                fnExceptionHandling(pobjException);
            }
            return blResult;

        }

        public bool fnSelectRadioBtn(string pstrStepName, IWebElement pobjWebElement, bool pblScreenShot = true, bool pblHardStop = false, string pstrHardStopMsg = "Select Radio Button Failed and HardStop defined")
        {
            bool blResult = false;
            try
            {
                //TestContext.Progress.WriteLine($"Step - {pstrStepName} - Info");
                clsReportResult.fnLog("SelectRadioBtn", "Step - " + pstrStepName, Status.Info, false);
                strAction = "Click";
                fnGetFluentWait(pobjWebElement, strAction);
                Thread.Sleep(5000);

                if (pobjWebElement.Selected)
                {
                    //TestContext.Progress.WriteLine($"{pstrStepName} - Pass");
                    clsReportResult.fnLog("SelectRadioBtnPass", pstrStepName, Status.Pass, pblScreenShot);
                    blResult = true;
                }
                else
                    throw new ArgumentException("SelectRadioBtnFail");

            }
            catch (Exception pobjException)
            {
                fnExceptionHandling(pobjException, pstrStepName, pblHardStop, pstrHardStopMsg);
            }
            return blResult;
        }

        public bool fnVerifyList(string pstrStepName, IWebElement pobjWebElement, string[] pstrListValues, string pstrListType = "option", bool pblScreenShot = true, bool pblHardStop = false, string pstrHardStopMsg = "Verify List Failed and HardStop defined")
        {
            bool blResult = false;
            try
            {
                bool blElementsFound = true;
                //TestContext.Progress.WriteLine($"Step - {pstrStepName} - Info");
                clsReportResult.fnLog("VerifyList", "Step - " + pstrStepName, Status.Info, false);
                IList<IWebElement> objListWE = pobjWebElement.FindElements(By.TagName(pstrListType));

                for (int i = 0; i < pstrListValues.Count(); i++)
                {
                    bool blElementFound = false;
                    foreach (IWebElement objWE in objListWE)
                    {
                        if (pstrListValues[i] == objWE.Text)
                        {
                            blElementFound = true;
                        }
                    }

                    if (blElementFound)
                    {
                        //TestContext.Progress.WriteLine($"Element from the List: {pstrListValues[i]} is Displayed - Info");
                        clsReportResult.fnLog("VerifyListItems", "Element from the List: " + pstrListValues[i] + " is Displayed", Status.Info, false);
                    }
                    else
                    {
                        //TestContext.Progress.WriteLine($"Element from the List: {pstrListValues[i]} is not Displayed - Info");
                        clsReportResult.fnLog("VerifyListItems", "Element from the List: " + pstrListValues[i] + " is not Displayed", Status.Info, false);
                        blElementsFound = false;
                    }
                }

                if (blElementsFound)
                {
                    //TestContext.Progress.WriteLine($"All Elements from the List are Displayed - Pass");
                    clsReportResult.fnLog("VerifyListPass", "All Elements from the List are Displayed", Status.Pass, pblScreenShot, pblHardStop);
                    blResult = true;
                }
                else
                    throw new ArgumentException("VerifyListFail");

            }
            catch (Exception pobjException)
            {
                //TestContext.Progress.WriteLine($"Some Elements from the List are not Displayed - Fail");
                clsReportResult.fnLog("VerifyListPass", "Some Elements from the List are not Displayed", Status.Fail, pblScreenShot, pblHardStop);
                fnExceptionHandling(pobjException, pstrStepName, pblHardStop, pstrHardStopMsg);
            }
            return blResult;
        }

        public virtual void fnExceptionHandling(Exception pobjException, string pstrStepName = "", bool pblHardStop = false, string pstrHardStopMsg = "Failed Step and HardStop defined")
        {
            clsReportResult clsRR = new clsReportResult();
            switch (pobjException.Message.ToString())
            {
                case "SendKeysFail":
                    clsReportResult.fnLog("SendKeysFail", "SendKeys action Fail", Status.Fail, true, pblHardStop, pstrHardStopMsg);
                    break;
                case "ClearFail":
                    clsReportResult.fnLog("ClearFail", "Clear action Fail", Status.Fail, true, pblHardStop, pstrHardStopMsg);
                    break;
                case "ElementExistFail":
                    clsReportResult.fnLog("ElementExistFail", "Element exist verification failed", Status.Fail, true, pblHardStop, pstrHardStopMsg);
                    break;
                case "ElementNotExistFail":
                    clsReportResult.fnLog("ElementNotExistFail", "Element not exist verification failed", Status.Fail, true, pblHardStop, pstrHardStopMsg);
                    break;
                case "ContainsTextFail":
                    clsReportResult.fnLog("ContainsTextFail", "Contains text verification failed", Status.Fail, true, pblHardStop, pstrHardStopMsg);
                    break;
                case "VerifyTextFail":
                    clsReportResult.fnLog("VerifyTextFail", "Verify text verification failed", Status.Fail, true, pblHardStop, pstrHardStopMsg);
                    break;
                case "VerifySelectedItemFail":
                    clsReportResult.fnLog("SelectedItemFail", "Coverage selected verification failed", Status.Fail, true, pblHardStop, pstrHardStopMsg);
                    break;
                case "SelectRadioBtnFail":
                    clsReportResult.fnLog("SelectRadioBtnFail", "Select Radio Button verification failed", Status.Fail, true, pblHardStop, pstrHardStopMsg);
                    break;
                case "Timed out after 10 seconds":
                    if (pobjException.InnerException.ToString().Contains("no such element: Unable to locate element"))
                    {
                        clsReportResult.fnLog("NoSuchElement", "WebElement doesn't exist or is incorrect", Status.Info, false);
                    }
                    break;
                default:
                    clsReportResult.fnLog("Exception", $"{pstrStepName}, Exception => Message({pobjException.Message.ToString()}), Stack Trace({pobjException.StackTrace.ToString()})", Status.Fail, true, pblHardStop, pstrHardStopMsg);
                    //clsReportResult.fnLog("Exception", $"Exception: Message({pobjException.Message.ToString()}), Stack Trace({pobjException.StackTrace.ToString()})", Status.Fail, true, pblHardStop, pstrHardStopMsg);
                    //Console.WriteLine($"Exception: Message({pobjException.Message.ToString()}), Stack Trace({pobjException.StackTrace.ToString()})");
                    break;
            }
        }

        /// <summary>
        /// Created to scroll in pages as needed and make a specific element visible
        /// </summary>
        /// <param name="driver">The WebDriver</param>
        /// <param name="element">The elelemt to scroll to</param>
        public void fnScrollToV2(IWebElement pobjWebElement, string pstrField, bool pblScreenShot = true, bool pblHardStop = false, string pstrHardStopMsg = "Scroll To Failed and HardStop defined")
        {
            try
            {
                clsReportResult.fnLog("ScrollTo", "Step - Scroll to element: " + pstrField, Status.Info, false);
                Thread.Sleep(TimeSpan.FromSeconds(2));
                new Actions(clsWebBrowser.objDriver)
                    .MoveToElement(pobjWebElement)
                    .Build()
                    .Perform();
                clsReportResult.fnLog("ScrollToPass", "Scrolled to element: " + pstrField, Status.Pass, pblScreenShot);
            }
            catch (Exception pobjException)
            {
                clsReportResult.fnLog("ScrollToFailed", "Failed Scroll to element: " + pstrField, Status.Fail, true, pblHardStop, pstrHardStopMsg);
                fnExceptionHandling(pobjException);
            }
        }

        [Obsolete("Use fnScrollToV2() or fnJsScrollTo() instead")]
        public void fnScrollTo(IWebElement pobjWebElement, string pstrField, bool pblScreenShot = true, bool pblHardStop = false, string pstrHardStopMsg = "Scroll To Failed and HardStop defined")
        {
            clsReportResult clsRR = new clsReportResult();
            try
            {
                clsReportResult.fnLog("ScrollTo", "Step - Scroll to element: " + pstrField, Status.Info, false);
                clsWebBrowser.objDriver.ExecuteJavaScript("arguments[0].scrollIntoView(true);", pobjWebElement);
                Thread.Sleep(TimeSpan.FromSeconds(2));
                clsReportResult.fnLog("ScrollToPass", "Scrolled to element: " + pstrField, Status.Pass, pblScreenShot);
            }
            catch (Exception pobjException)
            {
                clsReportResult.fnLog("ScrollToFailed", "Failed Scroll to element: " + pstrField, Status.Fail, true, pblHardStop, pstrHardStopMsg);
                fnExceptionHandling(pobjException);
            }
        }

        /// <summary>
        /// Created to scroll in pages as needed and make a specific element visible, Javascript version
        /// </summary>
        /// <param name="driver">The WebDriver</param>
        /// <param name="element">The elelemt to scroll to</param>
        public void fnJsScrollTo(IWebElement pobjWebElement, string pstrField, bool pblScreenShot = true, bool pblHardStop = false, string pstrHardStopMsg = "Scroll To Failed and HardStop defined", bool alignToTop = false)
        {
            clsReportResult clsRR = new clsReportResult();
            try
            {
                clsReportResult.fnLog("ScrollTo", "Step - Scroll to element: " + pstrField, Status.Info, false);
                ((IJavaScriptExecutor)clsWebBrowser.objDriver).ExecuteScript($"arguments[0].scrollIntoView({alignToTop.ToString()});", pobjWebElement);
                clsReportResult.fnLog("ScrollToPass", "Scrolled to element: " + pstrField, Status.Pass, pblScreenShot);
            }
            catch (Exception pobjException)
            {
                clsReportResult.fnLog("ScrollToFailed", "Failed Scroll to element: " + pstrField, Status.Fail, true, pblHardStop, pstrHardStopMsg);
                fnExceptionHandling(pobjException);
            }
        }
    }
}
