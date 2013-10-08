#region C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
#endregion

namespace StartWaBSeleniumTestsConsole
{
    class Program
    {
        /// <summary>
        /// Перменная для маски покотоорой будет идти замена для пути к xml-файлу с результатами
        /// </summary>
        private static readonly string strV = @".*\\([^\\]+$)";

        static void Main(string[] args)
        {            
            try
            {
                string options = "";
                string resultName = "TestResult.xml";
                string resultNewName = "";
                string configFile = "config.xml";

                string xmlOpt = "";
                string xmlDll = "";
                string xmlNUnit = "";
                string xmlXSL = "";

                ReadConfigFile(ref xmlOpt, ref xmlDll, ref xmlNUnit, ref xmlXSL, configFile);

                string strXmlDll = "";
                strXmlDll = CopyResultFile(xmlDll, ref strXmlDll);

                options = MakeOptions(ref options, xmlOpt);

                //make path without name of file
                string strOld = "<!--This file represents the results of running a test suite-->";
                string strPattern = "<?xml-stylesheet type='text/xsl' href='" +
                    xmlXSL + "'?>" +
                    "\n" + strOld;

                DeleteOldResult(strXmlDll + resultName);

                RunNUnit(xmlNUnit, xmlDll, options);

                CreateResultFile(strXmlDll, strOld, strPattern, resultName, ref resultNewName);

                OpenResultFile(resultNewName);
            }
            catch (Exception err)
            {
                Console.WriteLine("\nВо время запуска тестов произошла ошибка.\n" 
                    + err.Message + "\n");
            }

            Console.WriteLine("\nНажмите любую клавишу для закрытия программы...");
            Console.ReadLine();
        }

        private static string CopyResultFile(string xmlDll, ref string strXmlDll)
        {
            //make path without name of file
            string filename = Regex.Match(xmlDll, strV).Groups[1].Value;
            strXmlDll = xmlDll;
            TestReplaceText text = new TestReplaceText();
            text.replaceTextTest(ref strXmlDll, filename, "");
            return strXmlDll;
        }

        private static string MakeOptions(ref string options, string xmlOpt)
        {
            //Make options to NUnit
            if (xmlOpt == "")
                options = "";
            else
                options = "" + "/run:" + xmlOpt;
            return options;
        }

        private static void ReadConfigFile(ref string xmlOpt, ref string xmlDll, ref string xmlNUnit, ref string xmlXSL, string configFile)
        {
            ConfigReader cr = new ConfigReader();
            cr.Read(configFile, ref xmlDll, ref xmlNUnit, ref xmlXSL, ref xmlOpt);
        }

        private static void DeleteOldResult(string resultName)
        {
            if (System.IO.File.Exists(@resultName))
            {
                try
                {
                    System.IO.File.Delete(@resultName);
                }
                catch (System.IO.IOException e)
                {
                    return;
                }
            }
        }

        private static void RunNUnit(string xmlNUnit, string xmlDll, string options)
        {
            //run Nunit
            //Make sure, that Nunit or nunit-console makes result files default
            TestExecute exec = new TestExecute();
            exec.runTest(xmlNUnit, xmlDll + " " + options, true);
        }

        private static void CreateResultFile(string strXmlDll, string strOld, string strPattern, 
            string resultName, ref string resultNewName)
        {
            //read xml all
            //input xsl string to xml
            TestReadWriteXMLString wr = new TestReadWriteXMLString();

            string pref = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() +
                "(" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + ")";
            wr.readwriteXmlToString(strXmlDll + resultName, strOld, strPattern, pref + resultName);
            resultNewName = pref + resultName;
        }

        private static void OpenResultFile(string resultNewName)
        {
            //open new result xml with xsl
            TestExecute execBrowser = new TestExecute();
            execBrowser.runTest(resultNewName, "", false);
        }
    }
}
