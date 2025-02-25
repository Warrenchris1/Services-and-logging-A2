/*
*   FILE          : Program.cs
*   PROJECT       : Logging Client - A3
*   PROGRAMMER    : Ahmed, Warren
*   FIRST VERSION : 02/12/2025
*   DESCRIPTION   :
*      This is the main entry point for the logging client. It initializes and starts the Tester class.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static void Main()
        {
            // run tester
            Tester serverTester = new Tester();
            serverTester.Run();
        }
    }
}
