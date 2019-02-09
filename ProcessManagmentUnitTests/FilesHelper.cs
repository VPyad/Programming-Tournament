using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProcessManagmentUnitTests
{
    internal class FilesHelper
    {
        private const string JUNK_EXE = "junk.exe";
        private const string JUNK_TXT = "junk.txt";
        private const string JUNK_PDF = "junk.pdf";
        private const string JUNK_XML = "junk.xml";
        private const string JUNK_PNG = "junk.png";

        internal static void CreateJunkFiles(string dir)
        {
            string exeFile = Path.Combine(dir, JUNK_EXE);
            string textFile = Path.Combine(dir, JUNK_TXT);
            string pdfFile = Path.Combine(dir, JUNK_PDF);
            string xmlFile = Path.Combine(dir, JUNK_XML);
            string pngFile = Path.Combine(dir, JUNK_PNG);

            File.Create(exeFile).Close();
            File.Create(textFile).Close();
            File.Create(pdfFile).Close();
            File.Create(xmlFile).Close();
            File.Create(pngFile).Close();
        }

        internal static bool JunkFilesDeleted(string dir)
        {
            string exeFile = Path.Combine(dir, JUNK_EXE);
            string textFile = Path.Combine(dir, JUNK_TXT);
            string pdfFile = Path.Combine(dir, JUNK_PDF);
            string xmlFile = Path.Combine(dir, JUNK_XML);
            string pngFile = Path.Combine(dir, JUNK_PNG);

            List<bool> deleted = new List<bool>
            {
                File.Exists(exeFile),
                File.Exists(textFile),
                File.Exists(pdfFile),
                File.Exists(xmlFile),
                File.Exists(pngFile),
            };

            return deleted.All(x => x == false);
        }
    }
}
