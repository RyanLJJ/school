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
    class Screening : IComparable<Screening>
    {
        public int ScreeningNo { get; set; }
        public DateTime ScreeningDateTime { get; set; }
        public string ScreeningType { get; set; }
        public int SeatsRemaining { get; set; }
        public Cinema Cinema { get; set; }
        public Movie Movie { get; set; }
        public Screening() { }
        public Screening(int screeningNo, DateTime screeningDateTime, string screeningType, Cinema cinema, Movie movie)
        {
            ScreeningNo = screeningNo;
            ScreeningDateTime = screeningDateTime;
            ScreeningType = screeningType;
            Cinema = cinema;
            Movie = movie;
        }
        public int CompareTo(Screening s) // Sort by SeatsRemaining
        {
            if (SeatsRemaining < s.SeatsRemaining)
            {
                return 1;
            }
            else if (SeatsRemaining == s.SeatsRemaining)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
        public override string ToString()
        {
            return "ScreeningNo: " + ScreeningNo + "\tScreeningDateTime: " + ScreeningDateTime + "\tSeatsRemaining: " + SeatsRemaining + "\tScreeningType: " + ScreeningType + "\tSeatsRemaining: " + SeatsRemaining + "\tCinema: " + Cinema + "\tMovie: " + Movie;
        }
    }
}
