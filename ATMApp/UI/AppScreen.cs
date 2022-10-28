using ATMApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ATMApp.UI
{
    public class AppScreen
    {
        internal const string cur = "R$ ";
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

        internal static int SelectAmount()
        {
            WriteLine("");
            WriteLine(":1.{0}500           5.{0}10.000", cur);
            WriteLine(":2.{0}1000          6.{0}15.000", cur);
            WriteLine(":3.{0}2000          7.{0}20.000", cur);
            WriteLine(":4.{0}5000          8.{0}40.000", cur);
            WriteLine(":0.{0}Outro",cur);

            int selectedAmount = Validator.Convert<int>("opção:");
            switch(selectedAmount)
            {
                case 1: 
                    return 500;
                    break;
                case 2:
                    return 1000;
                    break;
                case 3:
                    return 2000;
                    break;
                case 4:
                    return 5000;
                    break;
                case 5:
                    return 10000;
                    break;
                case 6:
                    return 15000;
                    break;
                case 7:
                    return 20000;
                    break;
                case 8:
                    return 40000;
                    break;
                case 0:
                    return 0;
                    break;
                default:
                    Utility.PrintMessage("Entrada inválida. Tente novamente.", false);
                    SelectAmount();
                    return -1;
                    break;
            }

        }
        internal InternalTransfer InternalTransferForm()
        {
            var internalTransfer = new InternalTransfer();
            internalTransfer.ReciepeitBankAccountNumber = Validator.Convert<long>("número da conta do destinatário:");
            internalTransfer.TransferAmount = Validator.Convert<decimal>($"quantia {cur}");
            internalTransfer.RecienpeitBankAccountName = Utility.GetUserInput("nome do destinatário: ");
            return internalTransfer;
        }
    }
}
