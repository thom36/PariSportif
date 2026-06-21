using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PariSportif.Models
{
    public class Odd
    {
        public int Id {get; set;}
        public string? OddDescription {get; set;}
        public double Value {get; set;}
        public int MatchId {get; set;}
    }
}