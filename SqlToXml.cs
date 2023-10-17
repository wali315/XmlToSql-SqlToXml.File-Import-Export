using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Task2;

public class SqlToXml
{
    private string _connectionString;
    private Logger _logger;

    public SqlToXml(string connectionString, Logger logger)
    {
        _connectionString = connectionString;
        _logger = logger; // Use the provided logger
    }

    public void ExportDataToXml(string xmlFilePath)
    {
        MyConfiguration config = new MyConfiguration();
        // The logger is already provided via the constructor, no need to create a new one.
        try
        {
            Console.WriteLine("Data reading starts.");// Printing In Console.
            _logger.LogMessage("Data reading starts.");// Giving Message To LogFile.
            using (var context = new XmlDbContext())
            {
                var data = context.XmlModels.ToList();

                // Create an XML document
                XmlDocument xmlDoc = new XmlDocument();

                // Create a root element
                XmlElement rootElement = xmlDoc.CreateElement("XmlModels");
                xmlDoc.AppendChild(rootElement);

                // Serialize and add each data object to the XML
                foreach (var item in data)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(XmlModel));
                    using (var writer = new StringWriter())
                    {
                        serializer.Serialize(writer, item);
                        string xml = writer.ToString();
                        XmlDocument itemDoc = new XmlDocument();
                        itemDoc.LoadXml(xml);
                        rootElement.AppendChild(xmlDoc.ImportNode(itemDoc.DocumentElement, true));
                    }
                }

                // Save the XML document to a file
                xmlDoc.Save(xmlFilePath);

                Console.WriteLine("Data exported SQLToXML successfully.");// Printing In Console.
                _logger.LogMessage("Data exported From SqlToXml successfully.");// Giving Message To LogFile.

                // Calling Email By Method
                EmailSender emailSender = new EmailSender();
                emailSender.SendEmail("From SqlToXml", " Export Successful");//First Is Subject And Second Is Email Message You Can GEt this Message In You Gmail.
            }
            Console.WriteLine("Data reading completed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
           _logger.LogMessage("Error: " + ex.Message);
        }
    }
}
