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
    class Adult : Ticket
    {
        public bool PopcornOffer { get; set; }
        public Adult() { }
        public Adult(Screening s, bool popcornOffer) : base(s)
        {
            PopcornOffer = popcornOffer;
        }
        public override double CalculatePrice()
        {
            double price;
            string day = Convert.ToString(Screening.ScreeningDateTime.DayOfWeek);
            string movieType = Screening.ScreeningType;

            if (movieType == "3D") {

                if (day == "Friday" || day == "Saturday" || day == "Sunday")
                {
                    price = 14;

                }

                else {
                    price = 11;

                }
            }
            else { 

                if (day == "Friday" || day == "Saturday" || day == "Sunday")
                {
                    price = 12.50;
                }

                else
                {
                    price = 8.50;
                }
            }

            if (PopcornOffer == true)
            {

                price += 3;

            }
            
            return price;
        }
        public override string ToString()
        {
            return base.ToString() + "\tPopcornOffer: " + PopcornOffer;
        }
    }
}
