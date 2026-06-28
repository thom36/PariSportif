using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PariSportif.Exceptions
{
    public class WithdrawException : Exception
    {
        public WithdrawException(string msg)
            : base(msg) { }
    }
}