using System;
using System.Xml;
using System.Configuration;
using Task2;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.IO;

public class XmlToSql
{
    private string _connectionString;
    private Logger _logger;

    public XmlToSql(string connectionString, Logger logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    public void ConvertXmlToSql(string xmlFilePath)
    {
        MyConfiguration config = new MyConfiguration();
        string logFilePath = config.GetLogPath();
        Logger logger = new Logger(logFilePath);

        try
        {
            Console.WriteLine("Data reading starts.");// Printing In Console.
            _logger.LogMessage("Data reading starts.");//Giving Message To LogFile.
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);

            using (var context = new XmlDbContext())
            {
                foreach (XmlNode node in xmlDoc.SelectNodes("XmlModels/XmlModel"))
                {
                    int id = int.Parse(node.SelectSingleNode("Id").InnerText);
                    XmlModel existingRecord = context.XmlModels.FirstOrDefault(x => x.Id == id);

                    if (existingRecord != null)
                    {
                        // Update the existing record
                        existingRecord.Name = node.SelectSingleNode("Name").InnerText;
                        existingRecord.Age = int.Parse(node.SelectSingleNode("Age").InnerText);
                        existingRecord.Country = node.SelectSingleNode("Country").InnerText;
                    }
                    else
                    {
                        // Insert a new record
                        XmlModel data = new XmlModel
                        {
                            Name = node.SelectSingleNode("Name").InnerText,
                            Age = int.Parse(node.SelectSingleNode("Age").InnerText),
                            Country = node.SelectSingleNode("Country").InnerText
                        };
                        Console.WriteLine("New Data Inserted");
                        context.XmlModels.Add(data);
                    }
                }
                context.SaveChanges();
            }

            Console.WriteLine("Data Imported From XmlToSql successfully.");// Printing In Console.
            _logger.LogMessage("Data Imported From XmlToSql successfully.");//Giving Message To LogFile.
            Console.WriteLine("Reading Ends");
            // Log the XML content processed
            _logger.LogMessage("XML Content Processed:");//Giving Message To LogFile.

            // Calling Email By Method
            EmailSender emailSender = new EmailSender();
            emailSender.SendEmail("From XmlToSql", " Import Successful");//First Is Subject And Second Is Email Message You Can GEt this Message In You Gmail.
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine("Database Update Error: " + ex.Message);
            _logger.LogMessage("Database Update Error: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            _logger.LogMessage("Error: " + ex.Message);
        }
    }
}
