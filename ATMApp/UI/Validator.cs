using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
namespace ATMApp.UI
{
    public static class Validator
    {
        //MÉTODOS PARA VALIDAÇÃO
        public static T Convert<T>(string prompt)
        {
            bool valid = false;
            string userInput;

            while(!valid)
            {
                userInput = Utility.GetUserInput(prompt);

                try
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));

                    if(converter != null)
                    {
                        return (T) converter.ConvertFromString(userInput);
                    }
                    else
                    {
                        return default;
                    }
                }
                catch
                {
                    Utility.PrintMessage("Entrada Inválida. Tente novamente.", false);
                }
            }
            return default;
        }
    }
}
