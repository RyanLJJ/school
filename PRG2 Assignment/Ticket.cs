//=====================================================================================
// Student Number: S10222186, S10221841
// Student Name: Low Jun Jie, Ryan, Javien Tan Jie En
// Module Group: P07
//=====================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    abstract class Ticket
    {
        public Screening Screening { get; set; }
        public Ticket() { }
        public Ticket(Screening s)
        {
            Screening = s;
        }
        public abstract double CalculatePrice();
        public override string ToString()
        {
            return "Screening: " + Screening;
        }
    }
}
