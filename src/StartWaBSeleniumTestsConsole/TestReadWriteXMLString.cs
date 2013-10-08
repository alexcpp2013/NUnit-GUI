using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StartWaBSeleniumTestsConsole
{
    class TestReadWriteXMLString
    {
        public void readwriteXmlToString(string filename, string old, string pattern, string newname)
        {
            string source = string.Empty;
            using (System.IO.StreamReader reader = System.IO.File.OpenText(@filename))
            {
                source = reader.ReadToEnd();
            }
            source = source.Replace(old, pattern);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@newname))
            {
                file.Write(source);
            }
        }
    }
}
