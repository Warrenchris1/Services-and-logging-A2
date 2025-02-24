using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Tester
    {
        enum menuOptions
        {
            manualTest = 1,
            automaticTest = 2,
            abusePreventionTest = 3,
            exitApp = 4
        }

        internal void Run()
        {
            string ipaddress = ConfigurationManager.AppSettings["serverIP"];


            showMenu();
        }



        // this function displays the menu options and gets the user to choose a test
        private menuOptions showMenu()
        {
            Console.WriteLine("Select a test to perform: \n\t 1.Manually configured entry. \n\t " +
                "2.Automated test. \n\t 3.Abuse prevention test.");

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
