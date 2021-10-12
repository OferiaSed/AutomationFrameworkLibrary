﻿using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationFramework
{
    public class clsReportResult
    {
        public static ExtentReports objExtent;
        public static ExtentTest objTest;
        public static ExtentV3HtmlReporter objHtmlReporter;
        public static bool TC_Status;

        public static bool fnExtentSetup()
        {
            bool blSuccess;
            try
            {
                clsDataDriven clsDD = new clsDataDriven();
                blSuccess = clsDD.fnAutomationSettings();

                if (blSuccess)
                {
                    //To create report directory and add HTML report into it
                    objHtmlReporter = new ExtentV3HtmlReporter(clsDataDriven.strReportLocation + clsDataDriven.strReportName + @"\" + clsDataDriven.strReportName + ".html");

                    objHtmlReporter.Config.ReportName = clsDataDriven.strReportName;
                    objHtmlReporter.Config.DocumentTitle = clsDataDriven.strProjectName + " - " + clsDataDriven.strReportName;
                    objHtmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
                    objHtmlReporter.Config.Encoding = "utf-8";

                    objExtent = new ExtentReports();
                    objExtent.AddSystemInfo("Project", clsDataDriven.strProjectName);
                    objExtent.AddSystemInfo("Browser", clsDataDriven.strBrowser);
                    objExtent.AddSystemInfo("Env", clsDataDriven.strReportEnv);
                    objExtent.AttachReporter(objHtmlReporter);
                }
            }
            catch (Exception pobjException)
            {
                throw (pobjException);
            }
            return blSuccess;
        }

        public static void fnExtentClose()
        {
            try
            {
                var objStatus = TestContext.CurrentContext.Result.Outcome.Status;
                var objStacktrace = "" + TestContext.CurrentContext.Result.StackTrace + "";
                var objErrorMessage = TestContext.CurrentContext.Result.Message;
                Status objLogstatus;
                switch (objStatus)
                {
                    case TestStatus.Failed:
                        objLogstatus = Status.Fail;
                        objTest.Log(objLogstatus, "Test ended with " + objLogstatus + " – " + objErrorMessage);
                        break;
                    case TestStatus.Passed:
                        objLogstatus = Status.Pass;
                        break;
                    case TestStatus.Inconclusive:
                        objLogstatus = Status.Pass;
                        break;
                    default:
                        objLogstatus = Status.Warning;
                        Console.WriteLine("The status: " + objLogstatus + " is not supported.");
                        break;
                }
            }
            catch (Exception pobjException)
            {
                throw (pobjException);
            }

        }

        [Obsolete("New function replace pstrStatus parameter to receive Status.[your status]")]
        public static void fnLog(string pstrStepName, string pstrDescription, string pstrStatus, bool pblScreenShot, bool pblHardStop = false, string pstrHardStopMsg = "")
        {
            if (pblScreenShot)
            {
                string strSCLocation = fnGetScreenshot();
                switch (pstrStatus.ToUpper())
                {
                    case "PASS":
                        objTest.Log(Status.Pass, pstrDescription, MediaEntityBuilder.CreateScreenCaptureFromPath(strSCLocation).Build());
                        break;
                    case "FAIL":
                        TC_Status = false;
                        objTest.Log(Status.Fail, pstrDescription, MediaEntityBuilder.CreateScreenCaptureFromPath(strSCLocation).Build());
                        if (pblHardStop)
                            Assert.Fail(pstrHardStopMsg);
                        break;
                    case "INFO":
                        objTest.Log(Status.Info, pstrDescription, MediaEntityBuilder.CreateScreenCaptureFromPath(strSCLocation).Build());
                        break;
                    case "WARNING":
                        objTest.Log(Status.Warning, pstrDescription, MediaEntityBuilder.CreateScreenCaptureFromPath(strSCLocation).Build());
                        break;
                }
            }
            else
            {
                switch (pstrStatus.ToUpper())
                {
                    case "PASS":
                        objTest.Log(Status.Pass, pstrDescription);
                        break;
                    case "FAIL":
                        TC_Status = false;
                        objTest.Log(Status.Fail, pstrDescription);
                        if (pblHardStop) { Assert.Fail(pstrHardStopMsg); }
                        break;
                    case "INFO":
                        objTest.Log(Status.Info, pstrDescription);
                        break;
                    case "WARNING":
                        objTest.Log(Status.Info, pstrDescription);
                        break;
                }
            }
        }

        public static void fnLog(string pstrStepName, string pstrDescription, Status pstrStatus, bool pblScreenShot, bool pblHardStop = false, string pstrHardStopMsg = "")
        {
            MediaEntityModelProvider ss = null;
            if (pblScreenShot)
            {
                string strSCLocation = fnGetScreenshot();
                ss = MediaEntityBuilder.CreateScreenCaptureFromPath(strSCLocation).Build();
            }

            if (pstrStatus == Status.Fail)
            {
                TC_Status = false;
                if (pblHardStop) {
                    pstrHardStopMsg = $"Hardstop defined: {pstrHardStopMsg}";
                    objTest.Log(pstrStatus, pstrHardStopMsg, ss);
                    throw new Exception(pstrHardStopMsg);
                }
            }

            objTest.Log(pstrStatus, pstrDescription, ss);
        }

        public static string fnGetScreenshot()
        {
            string strSCName = "SC_" + clsDataDriven.strProjectName + "_" + DateTime.Now.ToString("MMddyyyy_hhmmss");

            //To take screenshot
            Screenshot objFile = ((ITakesScreenshot)clsWebBrowser.objDriver).GetScreenshot();

            string strFileLocation = @clsDataDriven.strReportLocation + @"\Screenshots\" + strSCName + ".jpg";
            //To save screenshot
            objFile.SaveAsFile(strFileLocation, ScreenshotImageFormat.Jpeg);

            return strFileLocation;
        }





    }
}
