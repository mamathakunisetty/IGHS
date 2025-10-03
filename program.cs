using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using Microsoft.VisualStudio.Coverage.Analysis;

namespace CodeCoverage
{
    class Program
    {
        static void Main(string[] args)
        {
            string coverageFilePath = string.Empty;
            List<XElement> elements = new List<XElement>();
            string coveragePath = string.Empty;
            string lookupPath = string.Empty;

#if DEBUG

           
            lookupPath = string.Format(@"{0}\out\", Directory.GetParent(Directory.GetParent(Directory.GetParent(coveragePath).ToString()).ToString()).ToString());
#else
           DirectoryInfo dirInfo = new DirectoryInfo(@"..\");
           coverageFilePath = (from fileInfo in dirInfo.GetFiles("data.coverage", SearchOption.AllDirectories)
                                       select fileInfo.FullName).FirstOrDefault();
           coveragePath = string.Format(@"{0}\In\{1}\data.coverage", Directory.GetParent(Directory.GetParent(Directory.GetParent(coverageFilePath).ToString()).ToString()).ToString(), Environment.MachineName);
            lookupPath = string.Format(@"{0}\out\", Directory.GetParent(Directory.GetParent(Directory.GetParent(coverageFilePath).ToString()).ToString()).ToString());
#endif

            if ((!string.IsNullOrWhiteSpace(coveragePath)) && (!string.IsNullOrWhiteSpace(lookupPath)))
            {
                IEnumerable<Header> header = Util.GetHeaders(coveragePath, lookupPath);
                IEnumerable<Category> parent = Util.GetParentCategories(header);
                IEnumerable<Category> children = Util.GetSubCategories(header);
                Util.Process(parent, children);
            }
            else
            {
                Console.WriteLine("Lookup path is not set");
            }
        }
    }
}
