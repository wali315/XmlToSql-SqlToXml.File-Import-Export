using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    internal class Program
    {
        static void Main(string[] args)
        {

            MyConfiguration config = new MyConfiguration();
            var FilePath = config.GetFilePath();
            string connectionString = config.GetConnectionString();
            string LogPath = config.GetLogPath();

            Logger logger = new Logger(LogPath);

            //Import Code From XmlToSql(calling This With An Instance)
            //XmlToSql xmlToSql = new XmlToSql(connectionString, logger);
            //xmlToSql.ConvertXmlToSql(FilePath);

            //Export Code From SqlToXml(calling This With An Instance)
            SqlToXml sqlToXml = new SqlToXml(connectionString, logger);
            sqlToXml.ExportDataToXml(FilePath);
        }
    }
}
