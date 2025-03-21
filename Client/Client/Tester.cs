﻿/*
*   FILE          : Tester.cs
*   PROJECT       : Logging Client - A3
*   PROGRAMMER    : Ahmed, Warren
*   FIRST VERSION : 02/22/2025
*   DESCRIPTION   :
*      This file implements a TCP client for sending log messages to the logging service.
*      It provides manual, automatic, and abuse-prevention test modes.
*/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Tester
    {
        private enum menuOptions //the different types of tests that can be performed
        {
            manualTest = 1,
            automaticTest = 2,
            abusePreventionTest = 3,
            exitApp = 4
        }

        private string ipAddress = ConfigurationManager.AppSettings["serverIP"];
        private int port = int.Parse(ConfigurationManager.AppSettings["serverPort"]);
        private string entriesFile = ConfigurationManager.AppSettings["entriesFile"]; //automatic test entries' file
        private int abuseEntriesCount = 30; //the number of entries to send when testing the abuse-prevention mechanism
        private string clientID = "TestingClient"; //the identifier of this tester

        /*
        *  Function  : Run()
        *  Summary   : This function runs in a loop that allows the user to continuously select a test to perform or quit the program
        *  Params    : None.
        *  Return    : None.
        */
        
        internal void Run()
        {
            /* loop until user quits program */
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
                        abusePreventionTest();
                        break;

                    case menuOptions.exitApp:
                        return;

                    default:
                        automaticTest();
                        break;
                }
            }
        }

        /*
        *  Function  : sendEntry()
        *  Summary   : this function sends a specified message to the server
        *  Params    :
        *     ipAddress
        *     portNumber 
        *     entry 
        *  Return    : None.
        */
        
        private void sendEntry(string ipAddress, int portNumber, string entry)
        {
            // initialize connection with server
            TcpClient client = new TcpClient(ipAddress, portNumber);
            NetworkStream stream = client.GetStream();

            // send message to the server
            byte[] data = Encoding.UTF8.GetBytes(entry);
            stream.Write(data, 0, data.Length);

            //NOTE: should server send back anything??
            //NOTE: how will server's abuse prevention work??

            // close connection
            stream.Close();
            client.Close();
        }


        /*
        *  Function  : manualTest()
        *  Summary   : this functions prompts the user to enter an entry and sends it to the server
        *  Params    : None.
        *  Return    : None.
        */
       
        private void manualTest()
        {
            Console.WriteLine("\nEnter an entry to log:");
            string entry = Console.ReadLine();
            sendEntry(ipAddress, port, entry);
        }

        /*
        *  Function  : automaticTest()
        *  Summary   : this functions read the automatic entries' file and sends each entry to the server. i.e. tests all possible scenario
        *  Params    : None.
        *  Return    : None.
        */
        
        private void automaticTest()
        {
            /* read each line in the automatic entries file and send it to server */
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

        /*
        *  Function  : abusePreventionTest()
        *  Summary   : this function sends a large volume of entries to the server in order to test its abuse-prevention mechanism
        *  Params    : None.
        *  Return    : None.
        */
        
        private void abusePreventionTest()
        {
            for (int c = 0; c < abuseEntriesCount; c++)
            {
                string entry = $"{clientID}|Abuse|This message is abusive.";
                sendEntry(ipAddress, port, entry);
            }
        }

        /*
        *  Function  : showMenu()
        *  Summary   : this function displays the menu and gets the user to select a test to perform
        *  Params    : None.
        *  Return    : 
        *     menuOptions : The test mode selected by the user.
        */
        
        private menuOptions showMenu()
        {
            /* show menu */
            Console.WriteLine("\nSelect a test to perform: \n\t 1.Manually configured entry. \n\t " +
                "2.Automated test (tests all message types). \n\t 3.Abuse prevention test. \n\t 4.Quit program.");

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

        /*
        *  Function  : getUTCTime()
        *  Summary   : this functions returns the current time in UTC format
        *  Params    : None.
        *  Return    : 
        *     string : The current UTC timestamp.
        */
        
        private string getUTCTime()
        {
            DateTime currentTime = DateTime.UtcNow;
            string formattedTime = currentTime.ToString("yyyy-MM-ddTHH:mm:ss");
            return formattedTime;
        }
    }
}
