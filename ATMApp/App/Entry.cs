using ATMApp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ATMApp.App
{
    class Entry
    {
        static void Main(string[] args)
        {
            
            ATMApp atmApp = new ATMApp();
            atmApp.InitializeData();
            atmApp.Run();
            

        }
    }
}
