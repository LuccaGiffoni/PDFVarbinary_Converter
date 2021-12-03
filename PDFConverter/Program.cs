using System;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net.Http;
using System.Threading.Tasks;

namespace PDFConverter
{
    class Program
    {
        private static string OriginalDataSource;
        private static string OriginalUserID;
        private static string OriginalPassword;
        private static string OriginalInitialCatalog;

        static void Main()
        {
            // Header
            Console.Clear();
            Console.WriteLine("Welcome to the PDF Converter!\nPlease, select what you wanna do:\n");
            Console.WriteLine("1. Save PDF file to database");
            Console.WriteLine("2. Delete PDF from database");
            Console.WriteLine("3. Close application\n");
            var response = Console.ReadLine();
            Console.WriteLine("____________________________________________________________________________________\n\n");

            if(response != "1" && response != "2" && response != "3")
            {
                // Header
                Console.Clear();
                Console.WriteLine("Welcome to the PDF Converter!\nPlease, select what you wanna do:\n");
                Console.WriteLine("1. Save PDF file to database\n");
                Console.WriteLine("2. Change connection string");
                response = Console.ReadLine();
                Console.WriteLine("____________________________________________________________________________________\n\n");
            }
            switch (response)
            {
                case "1":
                    SavePDF();
                    break;

                case "2":
                    DeletePDF();
                    break;

                case "3":
                    Environment.Exit(0);
                    break;
            }
        }

        private static void ChangeSettings()
        {
            // Saving original settings
            OriginalDataSource = ConnectionParameters.DataSource;
            OriginalUserID = ConnectionParameters.UserID;
            OriginalPassword = ConnectionParameters.Password;
            OriginalInitialCatalog = ConnectionParameters.InitialCatalog;

            // Header
            Console.Clear();
            Console.WriteLine("Welcome to the Settings!\nFollow the instruction bellow to change parameters:\n");
            Console.WriteLine("____________________________________________________________________________________\n\n");

            // Display actual variables
            Console.WriteLine("Actual parameters:\n");
            Console.WriteLine("Data Source: " + ConnectionParameters.DataSource);
            Console.WriteLine("User ID: " + ConnectionParameters.UserID);
            Console.WriteLine("Password: " + ConnectionParameters.Password);
            Console.WriteLine("Initial Catalog: " + ConnectionParameters.InitialCatalog);

            Console.WriteLine("\nIf you really wanna change something press enter or press any key to go back to main menu:");
            var readKey = Console.ReadKey();

            if(readKey.Key != ConsoleKey.Enter)
            {
                Main();
            }

            while(readKey.Key == ConsoleKey.Enter)
            {
                // Display actual variables
                Console.Clear();
                Console.WriteLine("\nActual parameters:\n");
                Console.WriteLine("Data Source: " + ConnectionParameters.DataSource);
                Console.WriteLine("User ID: " + ConnectionParameters.UserID);
                Console.WriteLine("Password: " + ConnectionParameters.Password);
                Console.WriteLine("Initial Catalog: " + ConnectionParameters.InitialCatalog);

                // Editing parameters
                Console.WriteLine("\nChanging parameters:\n");
                Console.WriteLine("Data Source:");
                    string datasource = Console.ReadLine();
                Console.WriteLine("\nUser ID:");
                    string userid = Console.ReadLine();
                Console.WriteLine("\nPassword:");
                    string password = Console.ReadLine();
                Console.WriteLine("\nInitial Catalog:");
                    string initialcatalog = Console.ReadLine();

                // Verifying response
                Console.WriteLine("\nNew parameters:\n");
                Console.WriteLine("Data Source: " + ConnectionParameters.DataSource);
                Console.WriteLine("User ID: " + ConnectionParameters.UserID);
                Console.WriteLine("Password: " + ConnectionParameters.Password);
                Console.WriteLine("Initial Catalog: " + ConnectionParameters.InitialCatalog);

                Console.WriteLine("\nTesting connection with new parameters...\n");
                TestConnection(datasource, userid, password, initialcatalog);

                Console.WriteLine("What you wanna do now: ");
                Console.WriteLine("\n[ENTER] Change parameters again");
                Console.WriteLine("[TAB] Reset to the very first original parameters");
                Console.WriteLine("[SPACE] Go back to main menu");
                Console.WriteLine("[BACKSPACE] Close application");

                readKey = Console.ReadKey();
            }

            if(readKey.Key == ConsoleKey.Tab)
            {
                // Reseting to the original settings
                ConnectionParameters.DataSource = OriginalDataSource;
                ConnectionParameters.UserID = OriginalUserID;
                ConnectionParameters.Password = OriginalPassword;
                ConnectionParameters.InitialCatalog = OriginalInitialCatalog;

                Console.WriteLine("Parameters are already set to original values!");
                Console.WriteLine("\nTo go back to main menu press enter or any key to close app:");
                var key = Console.ReadKey();

                if(key.Key == ConsoleKey.Enter)
                {
                    Main();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            else if(readKey.Key == ConsoleKey.Spacebar)
            {
                Main();
            }
            else if(readKey.Key == ConsoleKey.Backspace)
            {
                Environment.Exit(0);
            }
        }

        private static void TestConnection(string datasource, string userid, string password, string initialcatalog)
        {
            // Estabilishing Connection
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = datasource;
            builder.UserID = userid;
            builder.Password = password;
            builder.InitialCatalog = initialcatalog;

            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    // Opening connection
                    connection.Open();

                    if (connection.State.ToString() == "Open")
                    {
                        ConnectionParameters.DataSource = datasource;
                        ConnectionParameters.UserID = userid;
                        ConnectionParameters.Password = password;
                        ConnectionParameters.InitialCatalog = initialcatalog;

                        Console.WriteLine("\nThe connection was successfully estabilished and the parameters were change!");
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);

                ConnectionParameters.DataSource = datasource;
                ConnectionParameters.UserID = userid;
                ConnectionParameters.Password = password;
                ConnectionParameters.InitialCatalog = initialcatalog;

                Console.WriteLine("\nThe connection failed but the parameters were change");
            }
        }

        private static void SavePDF()
        {
            // Header
            Console.Clear();
            Console.WriteLine("Welcome to the PDF Converter!\nPlease, follow the instructions above:\n");

            // Get filename
            Console.WriteLine("Please, enter the name of the file: ");
            string pdfname = Console.ReadLine();

            // Test if the file termination is ok
            while (!pdfname.Contains(".pdf"))
            {
                Console.WriteLine("\nPlease, enter the name again using the pdf file termination (.pdf): ");
                pdfname = Console.ReadLine();
            }

            // Get filepath
            Console.WriteLine("\nPlease, enter the filepath: ");
            string filepath = Console.ReadLine();

            // Start process
            Console.WriteLine("\n\nPress any key to convert the PDF to varbinary and save to database");
            Console.ReadKey();

            // Estabilish connection with database
            OpenConnection(pdfname, filepath);
        }

        private static void DeletePDF()
        {
            // Header
            Console.Clear();
            Console.WriteLine("Welcome to the PDF Converter!\nPlease, follow the instructions above:\n");

            using (SqlConnection connection = new SqlConnection(ConnectionParameters.connectionBuilder.ConnectionString))
            {
                 // Opening connection
                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                // Getting info from user
                string DeletePDF = "DELETE FROM archives WHERE pdfname = @pdfname";
                string PDFName = "";

                Console.WriteLine("\nInsert the name of the PDF you wanna delete from database: ");
                PDFName = Console.ReadLine();

                try
                {
                    using (SqlCommand cmd = new SqlCommand(DeletePDF, connection))
                    {
                        cmd.Parameters.AddWithValue("@fileName", PDFName);
                        cmd.ExecuteNonQuery();

                        Console.WriteLine("The PDF with name " + PDFName + " was deleted with success");
                        Console.WriteLine("\nTo go back to main menu, press enter or press any other key to close application:");
                        var readKey = Console.ReadKey();

                        if (readKey.Key == ConsoleKey.Enter)
                        {
                            Main();
                        }
                        else
                        {
                            Environment.Exit(0);
                        }
                    }
                }
                catch (Exception error)
                {
                    Console.WriteLine(error.Message);

                    Console.WriteLine("\nAn error ocurred, to go back to main menu and try again, press enter or any other key to close application:");
                    var readKey = Console.ReadKey();

                    if(readKey.Key == ConsoleKey.Enter)
                    {
                        Main();
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }
            }
        }

        // Function to retrieve user's IP
        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private static void OpenConnection(string pdfname, string filepath)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionParameters.connectionBuilder.ConnectionString))
            {
                // Opening connection
                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                // Setting the file and the filename to be saved
                FileStream fStream = File.OpenRead(filepath);

                // Writing the doc as a stream
                byte[] contents = new byte[fStream.Length];
                fStream.Read(contents, 0, (int)fStream.Length);
                fStream.Close();

                try
                {
                    //// Insert the converted PDF to database
                    ///// THAT'S TEH LINE YOU NEED TO CHANGE TO USE IT
                    using (SqlCommand cmd = new SqlCommand(ConnectionParameters.InsertPDF, connection))
                    {
                        cmd.Parameters.AddWithValue("@fileName", pdfname);
                        cmd.Parameters.AddWithValue("@fileContent", contents);
                        cmd.ExecuteNonQuery();

                        Console.WriteLine("The PDF was saved with success on database");
                        Console.WriteLine("\nTo go back to main menu, press enter or press any other key to close application:");
                        var readKey = Console.ReadKey();

                        if (readKey.Key == ConsoleKey.Enter)
                        {
                            Main();
                        }
                        else
                        {
                            Environment.Exit(0);
                        }
                    }
                }
                catch (Exception error)
                {
                    Console.WriteLine(error.Message);

                    Console.WriteLine("\nAn error ocurred, to go back to main menu and try again, press enter or any other key to close application:");
                    var readKey = Console.ReadKey();

                    if(readKey.Key == ConsoleKey.Enter)
                    {
                        Main();
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }
            }
        }
    }
}
