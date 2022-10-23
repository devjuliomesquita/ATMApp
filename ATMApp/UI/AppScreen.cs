using ATMApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ATMApp.UI
{
    public static class AppScreen
    {
        internal const string cur = "N ";
        internal static void Welcome()
        {
            //Apresentação, Cabeçalho e introdução do BANK
            Clear();
            Title = "ATM BANK App";
            ForegroundColor = ConsoleColor.White;

            WriteLine("\n\n-----------Bem vindo ao aplicativo do ATM BANK-----------\n\n");
            WriteLine("Por favor insira o seu cartão ATM BANK.");
            Utility.PressEnterToContinue();
        }
        internal static UserAcconut UserLoginForm()
        {
            UserAcconut tempUserAccount = new UserAcconut();

            //UTILIZANDO O OBJETO CRIADO
            tempUserAccount.CardNumber = Validator.Convert<long>("seu número do cartão.");
            tempUserAccount.CardPin = Convert.ToInt32(Utility.GetSecretInput("Entre com o PIN do seu cartão."));
            return tempUserAccount;
        }
        internal static void LoginProgress()
        {
            //VERIFICAÇÃO E ATUALIZAÇÃO DE STATUS...
            WriteLine("\nVerificando o número do seu cartão e PIN...");
            Utility.PrintDotAnimation();
        }
        internal static void PrintLockScreen()
        {
            Clear();
            Utility.PrintMessage("Sua conta está bloqueada. " +
                "Procure uma agência ATM BANK mais próxima de võcê. Obrigado.", true);
            Utility.PressEnterToContinue();
            Environment.Exit(1);
        }
        internal static void WelcomeCustomer(string fullName)
        {
            WriteLine($"Bem vindo novamente, {fullName}");
            Utility.PressEnterToContinue();
        }

        internal static void DisplayAppMenu()
        {
            Clear();
            WriteLine("----------Meu ATM BANK----------");
            WriteLine(":                              :");
            WriteLine("1. Saldo                       :");
            WriteLine("2. Depositar                   :");
            WriteLine("3. Sacar Dinheiro              :");
            WriteLine("4. Transferir                  :");
            WriteLine("5. Extrato                     :");
            WriteLine("6. Sair                        :");
            
        }
        internal static void LogOutProgress()
        {
            WriteLine("Obrigado por usar o ATM BANK");
            Utility.PrintDotAnimation();
            Clear();
        }
    }
}
