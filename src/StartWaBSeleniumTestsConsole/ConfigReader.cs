using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#region XMLCode
using System.Xml;
#endregion

namespace StartWaBSeleniumTestsConsole
{
    class ConfigReader
    {
        public void Read(string configFile, ref string xmlDll, ref string xmlNUnit, ref string xmlXSL, ref string xmlOpt)
        {
            //Атрибут для идентификации начала данных в XML
            string fileheader = "/config";
            //Атрибут для идентификации пути к nunit
            string nunit = "path-nunit";
            //Атрибут для идентификации пути к xsl для преобразования xml
            string xsl = "path-xsl";
            //Атрибут для идентификации сборки dll для загрузки
            string dll = "path-dll";
            //Атрибут для дополнительных опций
            string opt = "option";
            
            try
            {
                XmlDocument rdr = new XmlDocument();
                rdr.Load(@configFile); // Загрузка XML

                XmlNode xmlData = rdr.SelectSingleNode(fileheader);

                xmlDll = xmlData[dll].InnerText;
                xmlNUnit = xmlData[nunit].InnerText;
                xmlXSL = xmlData[xsl].InnerText;
                xmlOpt = xmlData[opt].InnerText;
            }
            catch (Exception exp)
            {
                System.Console.WriteLine("Проблемы с парсингом файла " + @configFile + ".\n" + exp.Message.ToString());
            }
        }
    }
}
