﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.Domain.Interfaces
{
    public interface IUserAccountActions
    {
        void Saldo();
        void Despositar();
        void Sacar();

    }
}
