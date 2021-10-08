
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationFramework
{
    public class clsWebBrowser : clsWebElements
    {
        public static IWebDriver objDriver;
        public static WebDriverWait wait;

        public IWebDriver fnOpenBrowser(string pstrBrowser, bool pblScreenShot = false)
        {
            switch (pstrBrowser.ToUpper())
            {
                case "CHROME":

                    ChromeOptions optionsChrome = new ChromeOptions();
                    optionsChrome.AddArgument("no-sandbox");
                    objDriver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), optionsChrome, TimeSpan.FromMinutes(3));
                    objDriver.Manage().Timeouts().PageLoad.Add(System.TimeSpan.FromSeconds(10));
                    wait = new WebDriverWait(objDriver, TimeSpan.FromSeconds(10));
                    objDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    objDriver.Manage().Window.Maximize();

                    //objDriver = new ChromeDriver();
                    //wait = new WebDriverWait(objDriver, TimeSpan.FromSeconds(10));
                    //objDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    //objDriver.Manage().Window.Maximize();
                    clsReportResult.fnLog("OpenBrowser", "Step - Open Chrome Browser", "Info", false);
                    clsReportResult.fnLog("OpenBrowserPass", "Browser is openned correctly", "Pass", pblScreenShot);
                    break;
                case "EDGE":
                    var strDriverPath = fnGetProjectPath();
                    var optionsEdge = new OpenQA.Selenium.Edge.EdgeOptions();
                    optionsEdge.AddAdditionalCapability("UseChromium", true);
                    objDriver = new OpenQA.Selenium.Edge.EdgeDriver(OpenQA.Selenium.Edge.EdgeDriverService.CreateDefaultService(strDriverPath), optionsEdge, TimeSpan.FromMinutes(3));
                    objDriver.Manage().Timeouts().PageLoad.Add(System.TimeSpan.FromSeconds(10));
                    wait = new WebDriverWait(objDriver, TimeSpan.FromSeconds(10));
                    objDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    objDriver.Manage().Window.Maximize();
                    break;
                default:
                    break;
            }
            return objDriver;
        }

        public void fnCloseBrowser()
        {
            clsReportResult.fnLog("CloseBrowser", "Step - Browser is Closed", "Info", false);
            objDriver.Close();
            objDriver.Quit();
        }

        public void fnNavigateToUrl(string pstrUrl, bool pblScreenShot = true)
        {
            clsReportResult.fnLog("NavigateURL", "Step - Navigated to the URL: " + pstrUrl, "Info", false);
            objDriver.Navigate().GoToUrl(pstrUrl);
            clsReportResult.fnLog("NavigateURLPass", "Navigated to the URL succesfully", "Pass", pblScreenShot);
        }

        private string fnGetProjectPath()
        {
            var strProjectPath = new Uri(clsVariables.strEdgeDriverPath).LocalPath;
            /*
            var strPath = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            var strActualPath = "";
            var strProjectPath = "";
            if (!strPath.ToUpper().Contains("AGENT"))
            {
                strActualPath = strPath.Substring(0, strPath.LastIndexOf("bin"));
                strProjectPath = new Uri(strActualPath).LocalPath;
            }
            else 
            {

                //var TempPath = new Uri(strPath).LocalPath;
                //var TempPath = new Uri(strPath).LocalPath;
                //var arrArr = TempPath.Split('\\');
                //var NewPath = new Uri($"{arrArr[0]}\\{arrArr[1]}\\{arrArr[2]}\\").LocalPath;
                strProjectPath = new Uri(clsVariables.strEdgeDriverPath).LocalPath;
            }
            */
            return strProjectPath;
        }
    }
}
