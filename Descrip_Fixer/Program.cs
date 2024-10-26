using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Descrip_Fixer
{
    class Program
    {
        static void Main(string[] args)
        {
            string directoryPath = Directory.GetCurrentDirectory();
            var xmlFiles = Directory.EnumerateFiles(directoryPath, "*.xml");

            foreach (var file in xmlFiles)
            {
                XDocument doc = XDocument.Load(file);
                bool modified = false;

                // Handle <descrip> elements
                foreach (var descripElement in doc.Descendants("descrip"))
                {
                    string content = descripElement.Value;
                    int firstQuoteIndex = content.IndexOf('"');
                    int secondQuoteIndex = content.IndexOf('"', firstQuoteIndex + 1);

                    if (firstQuoteIndex != -1 && secondQuoteIndex != -1)
                    {
                        string newContent = content.Substring(firstQuoteIndex + 1, secondQuoteIndex - firstQuoteIndex - 1);
                        descripElement.Value = newContent;
                        modified = true;
                    }
                }

                // Handle <language> elements
                foreach (var languageElement in doc.Descendants("language"))
                {
                    string typeAttribute = (string)languageElement.Attribute("type");
                    string langAttribute = (string)languageElement.Attribute("lang");

                    if (typeAttribute == "Serbian (Latin, Serbia and Mont" && langAttribute == "SR-LATN-CS")
                    {
                        languageElement.SetAttributeValue("type", "Serbian (Latin, Serbia)");
                        languageElement.SetAttributeValue("lang", "SR-LATN-RS");
                        modified = true;
                        Console.WriteLine($"Language element modified in: {file}");
                    }
                }

                if (modified)
                {
                    doc.Save(file);
                    Console.WriteLine($"Modified and saved: {file}");
                }
                else
                {
                    Console.WriteLine($"No modifications needed for: {file}");
                }
            }

            Console.WriteLine("Processing complete.");
        }
    }
}
