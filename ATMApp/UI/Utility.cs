using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ATMApp.UI
{
    public static class Utility
    {
        private static long tranId;
        private static CultureInfo culture = new CultureInfo("PT-BR");
        public static long GetTransactionId()
        {
            return ++tranId;
        }
        public static string GetSecretInput(string prompt)
        {
            bool isPrompt = true;
            string asterics = "";

            StringBuilder input = new StringBuilder();
            while(true)
            {
                if (isPrompt)
                    WriteLine(prompt);
                isPrompt = false;
                ConsoleKeyInfo inputKey = ReadKey(true);

                if (inputKey.Key == ConsoleKey.Enter)
                {
                    if (input.Length == 6)
                    {
                        break;
                    }
                    else
                    {
                        PrintMessage("\nPor favor digite 6 números.", false);
                        isPrompt = true;
                        input.Clear();
                        continue;
                    }
                }
                if(inputKey.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length-1, 1);
                }
                else if(inputKey.Key != ConsoleKey.Backspace)
                {
                    input.Append(inputKey.KeyChar);
                    Write(asterics + "*");
                }
            }
            return input.ToString();
        }
        public static void PrintMessage(string msg, bool success = true)
        {
            if(success)
            {
                ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                ForegroundColor = ConsoleColor.Red;
            }
            WriteLine(msg);
            ForegroundColor = ConsoleColor.White;
            PressEnterToContinue();
        }
        public static string GetUserInput(string prompt)
        {
            WriteLine($"\nDigite {prompt}");
            return ReadLine();
        }
        public static void PrintDotAnimation(int time = 10)
        {
            for (int i = 0; i < time; i++)
            {
                Write(".");
                Thread.Sleep(200);
            }
            Clear();
        }
        public static void PressEnterToContinue()
        {
            WriteLine("\n\nAperte ENTER para continuar...\n\n");
            ReadLine();
        }
        //MÉTODO PARA A CONVERSÃO DA MOEDA UTILIZANOD O OBJETO CULTURE CRIADO
        public static string FormatAmount(decimal amt)
        {
            return String.Format(culture, "{0:C2}", amt);
        }

    }
}
