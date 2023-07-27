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
    class Order
    {
        public int OrderNo { get; set; }
        public DateTime OrderDateTime { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public List<Ticket> TicketList { get; set; } = new List<Ticket>();
        public Order() { }
        public Order(int orderNo, DateTime orderDateTime)
        {
            OrderNo = orderNo;
            OrderDateTime = orderDateTime;
        }
        public void AddTicket(Ticket t)
        {
            TicketList.Add(t);
        }
 
        public override string ToString()
        {
            return "OrderNo: " + OrderNo + "\tOrderDateTime: " + OrderDateTime + "\tAmount: " + Amount + "\tStatus: " + Status;
        }
    }
}
