using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationFramework.Test
{
    [TestFixture]
    class clsFrameworkDDTest : clsWebBrowser
    {
        public bool blStop;

        [OneTimeSetUp]
        public void BeforeClass()
        {
            blStop = clsReportResult.fnExtentSetup();
            if (!blStop)
                AfterClass();
        }

        public void SetupTest(string pstrTestCase)
        {
            clsReportResult.objTest = clsReportResult.objExtent.CreateTest(pstrTestCase);
            fnOpenBrowser("Edge");
            Console.WriteLine("");

        }

        [Test]
        public void Test_TestCasesEntry()
        {
            for (int i = 0; i < clsDataDriven.intCountTests; i++)
            {
                SetupTest(clsDataDriven.objTestCases[i, 3].ToString());
                fnNavigateToUrl("https://www.google.com/?hl=es");
                CloseTest();
            }
        }

        public void CloseTest()
        {
            fnCloseBrowser();
            clsReportResult.fnExtentClose();
        }

        [OneTimeTearDown]
        public void AfterClass()
        {
            try
            {
                clsReportResult.objExtent.Flush();
            }
            catch (Exception objException)
            {
                Console.WriteLine(objException.Message);
            }
        }
    }
}
