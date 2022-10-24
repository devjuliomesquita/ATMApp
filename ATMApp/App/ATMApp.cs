using ATMApp.Domain.Entities;
using ATMApp.Domain.Enums;
using ATMApp.Domain.Interfaces;
using ATMApp.UI;
using ConsoleTables;
using System;
using static System.Console;

namespace ATMApp.App
{
    public class ATMApp : IUserLogin, IUserAccountActions, ITransaction
    {
        private List<UserAcconut> userAccountList;
        private UserAcconut selectedAccount;
        private List<Transaction> _listOfTrasactions;
        private const decimal minimumKeptAmount = 500;
        private readonly AppScreen screen;
        public ATMApp()
        {
            screen = new AppScreen();
        }

        //CÓDIGO QUE VERIFICA O NÚMERO DO CARTÃO
        public void CheckUserCardNumAndPassword()
        {
            bool isCorrectLogin = false;
            while(isCorrectLogin ==false)
            {
                UserAcconut inputAccount = AppScreen.UserLoginForm();
                AppScreen.LoginProgress();
                foreach(UserAcconut account in userAccountList)
                {
                    selectedAccount = account;
                    if(inputAccount.CardNumber.Equals(selectedAccount.CardNumber))
                    {
                        selectedAccount.TotalLogin++;

                        if(inputAccount.CardPin.Equals(selectedAccount.CardPin))
                        {
                            selectedAccount = account;
                            
                            if(selectedAccount.IsLoocked || selectedAccount.TotalLogin > 3)
                            {
                                //MANDAR MENSAGEM DE BLOQUEIO
                                AppScreen.PrintLockScreen();
                            }
                            else
                            {
                                selectedAccount.TotalLogin = 0;
                                isCorrectLogin = true;
                                break;
                            }
                        }
                    }   
                }
                if (isCorrectLogin == false)
                {
                    Utility.PrintMessage("\nNúmero do cartão ou PIN inválidos. ", false);
                    selectedAccount.IsLoocked = selectedAccount.TotalLogin == 3;
                    if (selectedAccount.IsLoocked)
                    {
                        AppScreen.PrintLockScreen();
                    }
                }
                Clear();
            }
            
        }

        private void ProcessMenuOption()
        {
            switch(Validator.Convert<int>("uma opção:"))
            {
                case (int)AppMenu.Saldo:
                    WriteLine("Checando o seu saldo em conta...");
                    Saldo();
                    break;
                case (int)AppMenu.Despósito:
                    WriteLine("Opções de Despósito selecionado...");
                    Despositar();
                    break;
                case (int)AppMenu.Sacar:
                    WriteLine("Opção de Saque selecionado...");
                    Sacar();
                    break;
                case (int)AppMenu.Transferencia:
                    WriteLine("Opções de Transferência selecionado...");
                    var internalTransfer = screen.InternalTransferForm();
                    ProcessInternalTransfer(internalTransfer);
                    break;
                case (int)AppMenu.Extrato:
                    WriteLine("Checando o extrato da conta...");
                    ViewTransaction();
                    break;
                case (int)AppMenu.Sair:
                    AppScreen.LogOutProgress();
                    Utility.PrintMessage("Logooff feito com sucesso. Porfavor retire o seu cartão...");
                    Run();
                    break;
                default:
                    Utility.PrintMessage("Ação inválida. \nFavor digitar um número referente a umas das ações acima.",false);
                    break;
                
            }
        }

        public void Run()
        {
            AppScreen.Welcome();
            CheckUserCardNumAndPassword();
            AppScreen.WelcomeCustomer(selectedAccount.FullName);
            while (true)
            {
                AppScreen.DisplayAppMenu();
                ProcessMenuOption();
            }
        } 
        

        public void InitializeData()
        {
            userAccountList = new List<UserAcconut>
            {
                new UserAcconut
                {Id = 1
                ,FullName = "Júlio César M. Camilo Martins"
                ,AccountNumber = 123456
                ,CardNumber = 15071507
                ,CardPin = 123456
                ,AccountBalance = 50000.00m
                ,IsLoocked = false},

                new UserAcconut
                {Id = 2
                ,FullName = "Amanda Maria de Sousa Mesquita"
                ,AccountNumber = 456789
                ,CardNumber = 654654
                ,CardPin = 456456
                ,AccountBalance = 20000.00m
                ,IsLoocked = false},

                new UserAcconut
                {Id = 3
                ,FullName = "Célia Maria Pereira de Mesquita"
                ,AccountNumber = 741852
                ,CardNumber = 987987
                ,CardPin = 789789
                ,AccountBalance = 100000.00m
                ,IsLoocked = true},

                new UserAcconut
                {Id = 3
                ,FullName = "Célia Maria Pereira de Mesquita"
                ,AccountNumber = 741852
                ,CardNumber = 987987
                ,CardPin = 789789
                ,AccountBalance = 100000.00m
                ,IsLoocked = true},
            };
            _listOfTrasactions = new List<Transaction>();
        }

        public void Saldo()
        {
            Utility.PrintMessage($"Seu saldo é: {Utility.FormatAmount(selectedAccount.AccountBalance)}");
        }

        public void Despositar()
        {
            WriteLine("\nDepositos apenas com notas de R$ 50,00 ou R$ 100,00");
            var transaction_amt = Validator.Convert<int>($"valor {AppScreen.cur}");
            Utility.PrintDotAnimation();
            WriteLine("");

            if(transaction_amt <= 0)
            {
                Utility.PrintMessage("A quantidade necessária para o depósito deve ser maior que R$ 0,00 - zero.", false);
                return;
            }
            if(transaction_amt % 500 != 0)
            {
                Utility.PrintMessage("As notas para o depósito devem ser multiplas de 50 e 100.", false);
                return;
            }
            if(PreviewBankNotesCount(transaction_amt) == false)
            {
                Utility.PrintMessage("Ação cancelada.", false);
                return;
            }

            //DETALHES DA TRANSAÇÃO OBJETO
            InsertTransaction(selectedAccount.Id, TransactionType.Depositar, transaction_amt,"");

            //INCREMENTAR NA CONTA O VALOR
            selectedAccount.AccountBalance += transaction_amt;

            //MENSAGEM DE SUCESSO
            Utility.PrintMessage($"Trasação bem sucessida. Depósito de {Utility.FormatAmount(transaction_amt)}.", true);
        }

        public void Sacar()
        {
            var transaction_amt = 0;
            int selectedAmount = AppScreen.SelectAmount();

            if(selectedAmount == -1)
            {
                Sacar();
                return;
            }
            else if(selectedAmount != 0)
            {
                transaction_amt = selectedAmount;
            }
            else
            {
                transaction_amt = Validator.Convert<int>($"quantia {AppScreen.cur}");
            }

            //VALIDAÇÃO DAS ENTRADAS
            if(transaction_amt <= 0)
            {
                Utility.PrintMessage("Para o saque é necessário que a quantida seja maior que zero. Tente novamente.", false);
                return;
            }
            if(transaction_amt % 500 != 0)
            {
                Utility.PrintMessage("Você pode sacar apenas valores multiplos de 500 e 1000. Tente novamente.", false);
                return;
            }

            //VALIDAÇÃO DA LÓGICA DE NEGÓCIOS
            if(transaction_amt > selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Saque bloqueado. Seu saldo é inferior a quantia solicitada. " +
                    $"{Utility.FormatAmount(transaction_amt)}", false);
                return;
            }
            if((selectedAccount.AccountBalance - transaction_amt) < minimumKeptAmount)
            {
                Utility.PrintMessage($"Saque bloqueado.É necessário que fique em saldo o mínimo de {Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }
            //ALIMENTANDO O OBJETO DE DETALHES DA TRANSAÇÃO
            InsertTransaction(selectedAccount.Id, TransactionType.Sacar, -transaction_amt, "");
            //UPDATE NO SALDO
            selectedAccount.AccountBalance -= transaction_amt;
            //MSG DE SUCESSOS
            Utility.PrintMessage($"Saque realizado com sucesso {Utility.FormatAmount(transaction_amt)}.", true);
        }
        private bool PreviewBankNotesCount(int amount)
        {
            int thousandNotesCount = amount / 1000;
            int fiveHundredNotesCount = (amount % 1000) / 500;

            WriteLine("\nResumo");
            WriteLine("-------");
            WriteLine($"{AppScreen.cur}1000 X {thousandNotesCount} = {thousandNotesCount*1000}");
            WriteLine($"{AppScreen.cur}500 X {fiveHundredNotesCount} = {fiveHundredNotesCount * 500}");
            WriteLine($"Quantia total {Utility.FormatAmount(amount)}\n\n");

            int opt = Validator.Convert<int>("1 para confirmar...");
            return opt.Equals(1);
        }

        public void InsertTransaction(long _UserBankAccountId, TransactionType _transactionType, decimal _trasactionAmount, string _desc)
        {
            //CRIAR UM NOVO OBJETO DE TRASAÇÃO
            var trasaction = new Transaction()
            {
                TransactionId = Utility.GetTransactionId(),
                UserBankAcconuntId = _UserBankAccountId,
                TrasactionDate = DateTime.Now,
                TransactionType = _transactionType,
                TrasactionAmount = _trasactionAmount,
                Description = _desc
            };

            //OBJETO DE TRASAÇÃO PARA A LISTA
            _listOfTrasactions.Add(trasaction);
        }

        public void ViewTransaction()
        {
            var filteredTrasactionList = _listOfTrasactions.Where(t => t.UserBankAcconuntId == selectedAccount.Id).ToList();
            if(filteredTrasactionList.Count <= 0)
            {
                Utility.PrintMessage("Você não tem transações.", true);
            }
            else
            {
                var table = new ConsoleTable("Id","Transaction Data", "Type", "Description","Amount" + AppScreen.cur);
                foreach(var trans in filteredTrasactionList)
                {
                    table.AddRow(trans.TransactionId
                                , trans.TrasactionDate
                                , trans.TransactionType
                                , trans.Description
                                , trans.TrasactionAmount);
                }
                table.Options.EnableCount = false;
                table.Write();
                Utility.PrintMessage($"Você tem essas trasações {filteredTrasactionList.Count}", true);
            }
        }
        private void ProcessInternalTransfer(InternalTransfer internalTransfer)
        {
            if(internalTransfer.TransferAmount <= 0)
            {
                Utility.PrintMessage("Quantia necessária deve ser maior que zero. Tente novamente.", false);
                return;
            }
            if(internalTransfer.TransferAmount > selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Transferência bloqueada. Você não tem saldo sufuciente para a transação " +
                    $"{Utility.FormatAmount(internalTransfer.TransferAmount)}");
                return;
            }
            if((selectedAccount.AccountBalance - internalTransfer.TransferAmount) < minimumKeptAmount)
            {
                Utility.PrintMessage($"Tranferência bloqueada. Sua conta precisa ter um mínimo de " +
                    $"{Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }

            var selectedBankAccountReceiver = (from userAcc in userAccountList
                                               where userAcc.AccountNumber == internalTransfer.ReciepeitBankAccountNumber
                                               select userAcc).FirstOrDefault();
            if(selectedBankAccountReceiver ==null)
            {
                Utility.PrintMessage("Transferência bloqueada. Conta do usuário inválida.", false);
                return ;
            }
            if(selectedBankAccountReceiver.FullName != internalTransfer.RecienpeitBankAccountName)
            {
                Utility.PrintMessage("Transferência bloqueada. ", false);
                return;
            }

            //ADICIONAR A TRANSAÇÃO NA GRAVAÇÃO
            InsertTransaction(selectedAccount.Id, TransactionType.Transferir, -internalTransfer.TransferAmount, "Transferência " +
                $"para {selectedBankAccountReceiver.AccountNumber} ({selectedBankAccountReceiver.FullName})");
            //UPDATE NO VALOR DA CONTA DO TRANSFERIDOR
            selectedAccount.AccountBalance -= internalTransfer.TransferAmount;
            //ADICIONAR NA GRAVAÇÃO DE RECEBIMENTO
            InsertTransaction(selectedBankAccountReceiver.Id, TransactionType.Transferir, internalTransfer.TransferAmount,"Transferido de " +
                $"{selectedAccount.AccountNumber}({selectedAccount.FullName}).");
            //UPDATE NO VALOR DA CONTA DO RECEBEDOR
            selectedBankAccountReceiver.AccountBalance += internalTransfer.TransferAmount;
            //PRINT MSG DE SUCESSO
            Utility.PrintMessage($"Tranferência concluída {Utility.FormatAmount(internalTransfer.TransferAmount)} para " +
                $"{internalTransfer.RecienpeitBankAccountName}", true);

        }
    }
}