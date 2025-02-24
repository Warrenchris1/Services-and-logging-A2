using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Tester
    {
        enum menuOptions // holds the different tests that can be performed
        {
            manualTest = 1,
            automaticTest = 2,
            abusePreventionTest = 3,
            exitApp = 4
        }

        private string ipAddress = ConfigurationManager.AppSettings["serverIP"];
        private int port = int.Parse(ConfigurationManager.AppSettings["serverPort"]);
        private string entriesFile = ConfigurationManager.AppSettings["entriesFile"];


        internal void Run()
        {
            while (true)
            { 
                menuOptions userChoice = showMenu();
                switch (userChoice)
                {
                    case menuOptions.manualTest:
                        manualTest();
                        break;

                    case menuOptions.automaticTest:
                        automaticTest();
                        break;

                    case menuOptions.abusePreventionTest:

                        break;
                    case menuOptions.exitApp:

                        break;
                    default:

                        break;
                }
            }

        }


        // this function sends the specified entry to the server
        private void sendEntry(string ipAddress, int portNumber, string entry)
        {
            // initialize connection with server
            TcpClient client = new TcpClient(ipAddress, portNumber);
            NetworkStream stream = client.GetStream();

            // send the entry to the server
            byte[] data = Encoding.UTF8.GetBytes(entry);
            stream.Write(data, 0, data.Length);

            //NOTE: should server send back anything??

            // close connection
            stream.Close();
            client.Close();
        }



        // this functions prompts the user to enter a entry and sends it to the server
        private void manualTest()
        {
            Console.WriteLine("\nEnter an entry to log:");
            string entry = Console.ReadLine();

            sendEntry(ipAddress, port, entry);
        }

        // this functions read the automatic entries file and sends each one to the server - testing each scenario
        private void automaticTest()
        {
            StreamReader reader;
            using (reader = new StreamReader(entriesFile))
            {
                string entry;
                while ((entry = reader.ReadLine()) != null)
                {
                    sendEntry(ipAddress, port, entry);
                }
            }

            reader.Close();
        }



        // this function displays the menu and gets the user to select an option
        private menuOptions showMenu()
        {
            /* show menu */
            Console.WriteLine("\nSelect a test to perform: \n\t 1.Manually configured entry. \n\t " +
                "2.Automated test (tests all message types). \n\t 3.Abuse prevention test.");

            /* loop until user selects a menu option */
            menuOptions selectedOption;
            while (true)
            {
                string input = Console.ReadKey(true).KeyChar.ToString();

                if (Enum.TryParse(input, out selectedOption) && Enum.IsDefined(typeof(menuOptions), selectedOption))
                {
                    break;
                }
            }

            return selectedOption;
        }










    }
}
