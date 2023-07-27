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
    class SeniorCitizen : Ticket
    {
        public int YearOfBirth { get; set; }
        public SeniorCitizen() { }
        public SeniorCitizen(Screening s, int yearOfBirth) : base(s)
        {
            YearOfBirth = yearOfBirth;
        }
        public override double CalculatePrice()
        {
            double price;
            string day = Convert.ToString(Screening.ScreeningDateTime.DayOfWeek);
            string movieType = Screening.ScreeningType;
            DateTime openingDate = Screening.Movie.OpeningDate;
            DateTime oneWeekLater = openingDate.AddDays(7);

            if (movieType == "3D")
            { 

                if (day == "Friday" || day == "Saturday" || day == "Sunday")
                {
                    price = 14;
                }

                else
                {
                    if (Screening.ScreeningDateTime < oneWeekLater)
                    {
                        price = 11;
                    }
                    else {
                        price = 6;
                    }

                }
            }

            else
            {

                if (day == "Friday" || day == "Saturday" || day == "Sunday")
                {
                    price = 12.50;
                }

                else
                {
                    if (Screening.ScreeningDateTime < oneWeekLater)
                    {
                        price = 8.50;
                    }

                    else {
                        price = 5;
                    }

                }
            }

            return price;
        }
        public override string ToString()
        {
            return base.ToString() + "\tYearOfBirth: " + YearOfBirth;
        }
    }
}
