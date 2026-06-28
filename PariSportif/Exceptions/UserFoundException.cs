using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PariSportif.Exceptions
{
    public class UserFoundException : Exception
    {
        public UserFoundException(string msg)
            : base(msg) { }
    }
}