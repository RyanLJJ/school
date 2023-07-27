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
    class Cinema
    {
        public string Name { get; set; }
        public int HallNo { get; set; }
        public int Capacity { get; set; }
        public Cinema() { }
        public Cinema(string name, int hallNo, int capacity)
        {
            Name = name;
            HallNo = hallNo;
            Capacity = capacity;
        }
        public override string ToString()
        {
            return "Name: " + Name + "\tHallNo: " + HallNo + "\tCapacity: " + Capacity;
        }
    }
}
