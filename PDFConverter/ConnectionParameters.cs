using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFConverter
{
    class ConnectionParameters
    {
        // Change this fields using your paramaters to connect
        public static string DataSource = "your data source, from your database";
        public static string UserID = "the user to access the database";
        public static string Password = "the password to validate the access";
        public static string InitialCatalog = "the name of the database you're trying to access";

        // If you're using my script to create the table, let this string as it is.
        // But, if you're using your own table, change this to INSERT data correctly
        public static string SqlString = "INSERT INTO archives VALUES (@fileContent, @fileName)";
    }
}
