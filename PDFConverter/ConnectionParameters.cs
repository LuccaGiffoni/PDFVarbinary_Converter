using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFConverter
{
    class ConnectionParameters
    {
        public static string DataSource = "goit.database.windows.net";
        public static string UserID = "lucca";
        public static string Password = "JavaScript2002.";
        public static string InitialCatalog = "biomedical2";

        // SQL Builder
        public static SqlConnectionStringBuilder connectionBuilder = new SqlConnectionStringBuilder
        {
            UserID = UserID,
            Password = Password,
            InitialCatalog = InitialCatalog,
            DataSource = DataSource
        };

        public static string InsertPDF = "INSERT INTO archives VALUES (@fileContent, @fileName)";
        public static string UpdatePDF = "UPDATE OperationSteps SET step_pdf = @fileContent WHERE step_code = @stepcode";
    }
}
