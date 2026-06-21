using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PariSportif.Dto
{
    public class OddDto
    {
        public string OddDescription {get; set;} = string.Empty;
        public double Value {get; set;}
    }
}