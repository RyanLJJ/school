//=====================================================================================
// Student Number: S10222186, S10221841
// Student Name: Low Jun Jie, Ryan, Javien Tan Jie En
// Module Group: P07
//=====================================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    class Movie 
    {

        public string Title { get; set; }
        public int Duration { get; set; }
        public string Classification { get; set; }
        public DateTime OpeningDate { get; set; }
        public List<string> GenreList { get; set; } = new List<string>();
        public List<Screening> ScreeningList { get; set; } = new List<Screening>();
        public Movie() { }
        public Movie(string title, int duration, string classification, DateTime openingDate, List<string> genreList)
        {
            Title = title;
            Duration = duration;
            Classification = classification;
            OpeningDate = openingDate;
            GenreList = genreList;
        }
  
        public void AddScreening(Screening s)
        {
            ScreeningList.Add(s);
        }

        public void AddGenre(string s) 
        {
            GenreList.Add(s);
        }
        public override string ToString()
        {
            return "Title: " + Title + "\tDuration: " + Duration + "\tClassification: " + Classification + "\tOpeningDate: " + OpeningDate;
        }
    }
}
