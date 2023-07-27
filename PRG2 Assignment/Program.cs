//=====================================================================================
// Student Number: S10222186, S10221841
// Student Name: Low Jun Jie, Ryan, Javien Tan Jie En
// Module Group: P07
//=====================================================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace PRG2_Assignment
{
    class Program
    {
        static void Main(string[] args)
        {
            // For Recommended Movie Advanced Feature
            List<string> titleList = new List<string>();

            // For Load Movie and Cinema Data
            List<Movie> movieList = new List<Movie>();
            List<Cinema> cinemaList = new List<Cinema>();

            // For Load Screening Data
            List<Screening> screeningList = new List<Screening>();
            List<string> movTitleList = new List<string>();

            // For Ordering Tickets
            List<Order> orderList = new List<Order>();

            bool loadedMovieData = false;
            bool loadedScreeningData = false;

            while (true)
            {
                Console.WriteLine("\n------------ Main Menu -------------\n1. Load Movie and Cinema Data\n2. Load Screening Data\n3. List all movies\n4. List movie screenings\n5. Add a movie screening session\n6. Delete a movie screening session\n7. Order movie ticket/s\n8. Cancel order of ticket\n0. Exit");
                Console.Write("Please select an option: ");
                var input = Console.ReadLine();
                try
                {
                    int option = Convert.ToInt32(input);

                    if (option == 1) //Load Movie and Cinema Data
                    {
                        if (movieList.Count != 0)
                        {
                            Console.WriteLine("The movie and cinema data has already been loaded in.");
                        }
                        else
                        {
                            InitMovieList(movieList, titleList);
                            InitCinemaList(cinemaList);
                            loadedMovieData = true;
                        }
                    }

                    else if (loadedMovieData == false) // If the movie and cinema data has not been loaded in
                    {
                        Console.WriteLine("Please load in the movie and cinema data.");
                    }

                    else if (option == 2) //Load Screening Data
                    {
                        if (screeningList.Count != 0)
                        {
                            Console.WriteLine("The screening data has already been loaded in.");
                        }
                        else
                        {
                            InitScreeningList(screeningList, movieList, cinemaList, movTitleList);
                            loadedScreeningData = true;
                        }
                    }

                    else if (loadedScreeningData == false) // If the screening data has not been loaded in
                    {
                        Console.WriteLine("Please load in the screening data.");
                    }

                    else if (option == 3) // List all movies
                    {
                        ListAllMovies(movieList, orderList, titleList);
                    }

                    else if (option == 4) // List movie screenings
                    {
                        ListAllMovies(movieList, orderList, titleList);
                        ListAllScreenings(movieList);
                    }

                    else if (option == 5) // Add a movie screening session
                    {
                        ListAllMovies(movieList, orderList, titleList);
                        AddMovieScreeningSession(movieList, cinemaList, screeningList);
                    }

                    else if (option == 6) // Delete a movie screening session
                    {
                        DeleteMovieScreeningSession(screeningList);
                    }

                    else if (option == 7)
                    {
                        // List all movies
                        ListAllMovies(movieList, orderList, titleList);

                        // Order movie ticket/s
                        OrderTicket(movieList, orderList);
                    }

                    else if (option == 8) // Cancel order of ticket
                    {
                        RemoveOrderTicket(orderList);
                    }

                    else if (option == 0) // Exit
                    {
                        Console.WriteLine("\nGoodbye!");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Please select a valid option!");
                    }
                }
                catch
                {
                    Console.WriteLine("Please select a valid option!");
                }
            }
        }

        static void InitMovieList(List<Movie> movieList, List<string> titleList) // Reads Movie.csv and creates and populates the movie objects into a list.
        {
            string[] csvLines = File.ReadAllLines("Movie.csv");
            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] data = csvLines[i].Split(",");
                string movGenre = data[2];
                List<string> genreList = new List<string>();
                if (movGenre.Contains("/"))
                {
                    string[] multiGenre = movGenre.Split("/");
                    foreach (string g in multiGenre)
                    {
                        genreList.Add(g);
                    }
                }

                else
                {
                    genreList.Add(movGenre);
                }

                Movie newMovie = new Movie(data[0], Convert.ToInt32(data[1]), data[3], Convert.ToDateTime(data[4]), genreList);
                movieList.Add(newMovie);
            }

            // Adds titles of all movies into the list. (Advanced feature)
            foreach (Movie m in movieList) {
                titleList.Add(m.Title);
            }
        }

        static void InitCinemaList(List<Cinema> cinemaList) // Reads Cinema.csv and creates and populates the cinema objects into a list.
        {
            string[] csvLines = File.ReadAllLines("Cinema.csv");
            for (int i = 1; i < csvLines.Length; i++)
            {
                string[] data = csvLines[i].Split(",");
                Cinema newCinema = new Cinema(data[0], Convert.ToInt32(data[1]), Convert.ToInt32(data[2]));
                cinemaList.Add(newCinema);
            }
        }

        static void InitScreeningList(List<Screening> screeningList, List<Movie> movieList, List<Cinema> cinemaList, List<string> movTitleList) // Reads Screening.csv and creates and populates the cinema objects into a list.
        {
            string[] scLines = File.ReadAllLines("Screening.csv");

            for (int i = 1; i < scLines.Length; i++)
            {
                string[] data = scLines[i].Split(",");
                string scType = data[1];
                DateTime scDateTime = Convert.ToDateTime(data[0]);

                string scCinName = data[2];
                int scHallNo = Convert.ToInt32(data[3]);
                Cinema scCinema = SearchCinema(cinemaList, scCinName, scHallNo);

                string scMovTitle = data[4];
                Movie scMovie = SearchMovie(movieList, scMovTitle);

                movTitleList.Add(scMovTitle);

                int scNo = 1001;
                foreach (Screening s in screeningList) // Assigns a unique screening number to each screening based on its position in the screeningList.
                {
                    scNo++;
                }

                Screening newScreening = new Screening(scNo, scDateTime, scType, scCinema, scMovie);
                newScreening.SeatsRemaining = scCinema.Capacity;
                screeningList.Add(newScreening);
                scMovie.AddScreening(newScreening);
            }
        }

        static Cinema SearchCinema(List<Cinema> cinemaList, string a, int b) // Searches for the cinema object and returns it if found, null if not found.
        {

            foreach (Cinema c in cinemaList)
            {
                if (a == c.Name && b == c.HallNo)
                {
                    return c;
                }
            }
            return null;
        }

        static Movie SearchMovie(List<Movie> movieList, string s) // Searches for the movie object and returns it if found, null if not found.
        {

            foreach (Movie m in movieList)
            {
                if (s == m.Title)
                {
                    return m;
                }
            }
            return null;
        }

        static Screening SearchScreening(Movie m, int i) // Searches for the screening object and returns it if found, null if not found.
        {
            foreach (Screening s in m.ScreeningList)
            {
                if (s.ScreeningNo == i)
                {
                    return s;
                }
            }
            return null;
        }

        static Order SearchOrder(List<Order> orderList, int i) // Searches for the order object and returns it if found, null if not found.
        {

            foreach (Order o in orderList)
            {
                if (o.OrderNo == i)
                {
                    return o;
                }
            }
            return null;
        }

        static Screening SearchScreeningFromOrder(List<Order> orderList, int i)
        {
            foreach (Order o in orderList)
            {
                if (o.OrderNo == i)
                {
                    foreach (Ticket t in o.TicketList)
                    {
                        Screening orderScreening = t.Screening;
                        return orderScreening;
                    }
                }
            }
            return null;
        }

        static void ListAllMovies(List<Movie> movieList, List<Order> orderList, List<string> titleList)
        {
            // Advanced Feature for Recommended Movie Based on Tickets Sold

            List<int> bookingList = new List<int>();
            foreach (Movie m in movieList) // For each movie object in movieList, creates integer with value of 0 in bookingList. The index numbers of the integers in bookingList correspond with the index numbers of the titles in titleList.
            {
                int i = 0;
                bookingList.Add(i);     
            }

            if (orderList.Count != 0) // If orderList is not empty.
            {
                foreach (Order o in orderList) 
                {
                    int orderNo = o.OrderNo;
                    int ticketCount = 0;
                    foreach (Ticket t in o.TicketList) // Finds ticket count of the order.
                    {
                        ticketCount += 1;
                    }

                    Screening sc = SearchScreeningFromOrder(orderList, orderNo); // Finds the screening of the order.
                    string movTitle = sc.Movie.Title;
                    int indexNo = titleList.IndexOf(movTitle); // Finds the index number of the title in the title list.

                    bookingList[indexNo] += ticketCount; // Using indexNo, it adds the ticketCount to the integer with the corresponding index number in the bookingList.
                }

                int num = 0;

                foreach (int b in bookingList) // Searches for the largest integer in bookingList.
                {
                    if (num < b) 
                    {
                        num = b;
                    }
                }

                int titleIndex = bookingList.IndexOf(num); // titleIndex will become the index of "num" in bookingList.
                string recommendedMovie = titleList[titleIndex]; // Retrieves title from titleList using titleIndex.

                Console.WriteLine("\nRecommended movie: {0}", recommendedMovie);

            }

            // Listing the Movies

            Console.WriteLine("\n{0,-5}{1,-30}{2,-13}{3,-17}{4,-15}{5}", "No.", "Title", "Duration", "Classification", "Opening Date", "Genre(s)");
            for (int i = 0; i < movieList.Count; i++)
            {
                Movie m = movieList[i];
                string numString = Convert.ToString(i + 1) + ". ";

                string genres = "";
                foreach (var genre in m.GenreList)
                {
                    genres += genre;
                    genres += "/";
                }
                genres = genres.Remove(genres.Length - 1, 1);
                Console.WriteLine("{0,-5}{1,-30}{2,-13}{3,-17}{4,-15}{5}", numString, m.Title, m.Duration, m.Classification, m.OpeningDate.ToShortDateString(), genres);
            }
        }

        static void ListAllCinemas(List<Cinema> cinemaList)
        {
            int i = 1;
            Console.WriteLine("\n{0,-8}{1,-20}{2,-10}{3,-10}", "No.", "Name", "HallNo", "Capacity");
            foreach (var cinema in cinemaList)
            {
                Console.WriteLine("{0,-8}{1,-20}{2,-10}{3,-10}", i, cinema.Name, cinema.HallNo, cinema.Capacity);
                i++;
            }
        }

        static void ListAllScreenings(List<Movie> movieList)
        {
            
            // Selecting movie screening
            while (true)
            {
                try
                {
                    Console.Write("\nPlease select a movie (number): ");
                    int movieNum = Convert.ToInt32(Console.ReadLine()) - 1;
                    Movie movieSelected = movieList[movieNum];

                    List<Screening> sortedScreeningList = movieSelected.ScreeningList;
                    sortedScreeningList.Sort();

                    if (sortedScreeningList.Count == 0)
                    {
                        Console.WriteLine("\nThere are no screenings for this movie available.");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("\nList of screenings for: " + movieSelected.Title);
                        Console.WriteLine("\n{0,-26}{1,-17}{2,-22}{3,-17}{4,-17}{5}", "Screening Date & Time", "Cinema Name", "Cinema Hall Number", "Screening Type", "Screening No", "Seats Remaining");
                        foreach (Screening s in sortedScreeningList)
                        {
                            Console.WriteLine("{0,-26}{1,-17}{2,-22}{3,-17}{4,-17}{5}", s.ScreeningDateTime, s.Cinema.Name, s.Cinema.HallNo, s.ScreeningType, s.ScreeningNo, s.SeatsRemaining);
                        }
                        break;
                    }
                }

                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("You did not enter a valid number for any movie. Please try again.");
                }

                catch (FormatException)
                {
                    Console.WriteLine("You did not enter a number. Please try again.");
                }

            }

        }

        static bool CheckCinemaAvailability(Cinema desiredCinema, DateTime desiredStartDateTime, List<Screening> screeningList, Movie selectedMovie) // Checks if the cinema is available for a certain screening. Returns true if the cinema is available and false if not. 
        {
            int timeNeeded = selectedMovie.Duration + 30; // Duration of the whole movie + the 30 mins cleaning time.
            DateTime desiredEndDateTime = desiredStartDateTime.AddMinutes(timeNeeded); // Calculates the ending time of the desired screening.

            foreach (var screening in screeningList)
            {
                if (screening.Cinema == desiredCinema)
                {
                    DateTime startDateTime = screening.ScreeningDateTime;
                    Movie currentScreening = screening.Movie;
                    timeNeeded = currentScreening.Duration + 30;
                    DateTime endDateTime = startDateTime.AddMinutes(timeNeeded);
                    if ((DateTime.Compare(desiredStartDateTime, startDateTime) >= 0 && DateTime.Compare(desiredStartDateTime, endDateTime) < 0) || (DateTime.Compare(desiredEndDateTime, startDateTime) >= 0 && DateTime.Compare(desiredEndDateTime, endDateTime) < 0)) // Checks if the desired screening timeslot conflicts with any existing screenings.
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static void AddMovieScreeningSession(List<Movie> movieList, List<Cinema> cinemaList, List<Screening> screeningList)
        {
            try
            {
                Console.Write("Select movie (by number): ");
                int movieSelectedNo = Convert.ToInt32(Console.ReadLine());
                Movie movieSelected = SearchMovie(movieList, movieList[movieSelectedNo - 1].Title);

                if (movieSelected != null) // If the selected movie exists, prompt user to enter desired screening type.
                {
                    Console.Write("Enter a screening type (2D/3D): ");
                    string screeningType = Console.ReadLine();
                    string screeningTypeUpper = screeningType.ToUpper();

                    if (screeningTypeUpper == "2D" || screeningTypeUpper == "3D") // If the user inputs either 2D or 3D, prompt user to enter desired screening DateTime.
                    {
                        Console.Write("Enter a screening date and time (e.g. 03/02/2022 9:00PM): ");
                        DateTime dateTime = Convert.ToDateTime(Console.ReadLine());

                        if (DateTime.Compare(dateTime, movieSelected.OpeningDate) > 0) // If the desired DateTime is after the opening date of the movie, prompt user to enter desired cinema by number.
                        {
                            ListAllCinemas(cinemaList);
                            Console.Write("Select cinema (by number): ");
                            int selectedCinemaNumber = Convert.ToInt32(Console.ReadLine());
                            Cinema cinema = cinemaList[selectedCinemaNumber - 1];

                            if (CheckCinemaAvailability(cinema, dateTime, screeningList, movieSelected) == true) // Checks if cinema is available for the desired movie screening to take place at the chosen time.
                            {
                                // Creates screening object and adds it to the screening list.
                                int screeningNo = screeningList.Count + 1001;
                                Screening newScreening = new Screening(screeningNo, dateTime, screeningTypeUpper, cinema, movieSelected);
                                newScreening.SeatsRemaining = cinema.Capacity;
                                screeningList.Add(newScreening);
                                movieSelected.AddScreening(newScreening);
                                Console.WriteLine("Movie screening session creation successful.");
                            }
                            else
                            {
                                Console.WriteLine("That cinema hall is not available for the desired new movie screening session.");
                                Console.WriteLine("Movie screening session creation unsuccessful.");
                            }
                            
                        }
                        else
                        {
                            Console.WriteLine("Screening date and time must be after the opening date of the movie.");
                            Console.WriteLine("Movie screening session creation unsuccessful.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please choose either 2D or 3D.");
                        Console.WriteLine("Movie screening session creation unsuccessful.");
                    }
                }
                else
                {
                    Console.WriteLine("Selected movie number does not exist.");
                    Console.WriteLine("Movie screening session creation unsuccessful.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input!");
                Console.WriteLine("Movie screening session creation unsuccessful.");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Selected number does not exist!");
                Console.WriteLine("Movie screening session creation unsuccessful.");
            }
        }

        static void DeleteMovieScreeningSession(List<Screening> screeningList)
        {
            List<Screening> unsoldScreenings = new List<Screening>();

            // Checks each screening object to see if it has not sold any tickets. If no tickets are sold, that screening object will be added into unsoldScreenings list.
            foreach (var screening in screeningList)
            {
                if ((screening.Cinema.Capacity - screening.SeatsRemaining) == 0)
                {
                    unsoldScreenings.Add(screening);
                }
            }

            if (unsoldScreenings.Count == 0)
            {
                Console.WriteLine("There are no screenings that have not sold any tickets.");
                return;
            }
            // Prints all the screening objects that have not sold any tickets.
            Console.WriteLine("\nList of movie screening sessions that have not sold any tickets:");
            Console.WriteLine("{0,-4}{1,-26}{2,-17}{3,-22}{4,-17}{5,-17}{6}", " ", "Screening Date & Time", "Cinema Name", "Cinema Hall Number", "Screening Type", "Screening No", "Movie");
            int i = 1;
            foreach (var screening in unsoldScreenings)
            {
                Console.WriteLine("{0,-4}{1,-26}{2,-17}{3,-22}{4,-17}{5,-17}{6}", Convert.ToString(i) + ".", screening.ScreeningDateTime, screening.Cinema.Name, screening.Cinema.HallNo, screening.ScreeningType, screening.ScreeningNo, screening.Movie.Title);
                i++;
            }

            // Prompts user to enter the screening object that they want to delete. 
            try
            {
                Console.Write("Select a session number to delete: ");
                int selection = Convert.ToInt32(Console.ReadLine());
                Screening selectedScreening = screeningList[selection - 1];
                selectedScreening.Movie.ScreeningList.Remove(selectedScreening);
                screeningList.RemoveAt(selection - 1);
                Console.WriteLine("Removal of selected movie screening session successful.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Please input a session number!");
                Console.WriteLine("Removal of selected movie screening session unsuccessful.");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Please select a valid session number!");
                Console.WriteLine("Removal of selected movie screening session unsuccessful.");
            }
        }

        static void OrderTicket(List<Movie> movieList, List<Order> orderList)
        {
            Movie movieSelected; // Creates movie object to select movie

            while (true)
            {
                try
                {
                    Console.Write("\nPlease select a movie (number): "); // Prompts user to select a movie.
                    int movieNum = Convert.ToInt32(Console.ReadLine()) - 1;
                    movieSelected = movieList[movieNum];

                    List<Screening> movScreeningList = movieSelected.ScreeningList;

                    int seatsLeft = 0;

                    foreach (Screening s in movScreeningList) // Checks if there are any seats left for any screening of selected movie.
                    {
                        if (s.SeatsRemaining != 0) 
                        {
                            seatsLeft += 1;
                        }
                    }

                    if (movScreeningList.Count == 0) // If there are no more screenings left for the movie, breaks loop and returns to main menu.
                    {
                        Console.WriteLine("\nThere are no screenings for this movie available.");
                        return;
                    }

                    else if (seatsLeft == 0) // If there are no more seats left for any screening, breaks loop and returns to main menu.
                    {
                        Console.WriteLine("\nThere are no more seats left for any screenings for this movie.");
                        return;
                    }

                    else
                    {
                        Console.WriteLine("\nList of screenings for: " + movieSelected.Title);
                        Console.WriteLine("\n{0,-23}{1,-17}{2,-22}{3,-17}{4,-17}{5}", "Screening Date", "Cinema Name", "Cinema Hall Number", "Screening Type", "Screening No", "Seats Remaining");
                        foreach (Screening s in movScreeningList)
                        {
                            Console.WriteLine("{0,-23}{1,-17}{2,-22}{3,-17}{4,-17}{5}", s.ScreeningDateTime, s.Cinema.Name, s.Cinema.HallNo, s.ScreeningType, s.ScreeningNo, s.SeatsRemaining);
                        }
                        break;
                    }
                }


                catch (FormatException)
                {
                    Console.WriteLine("You did not enter a valid movie number. Please try again.");
                }

                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("The selected number does not exist for any movie. Please try again.");
                }
            }

            Screening screeningSelected; // Creates screening object to select screening

            while (true)
            {
                try
                {
                    Console.Write("\nSelect movie screening (number): ");
                    int screeningNo = Convert.ToInt32(Console.ReadLine());
                    screeningSelected = SearchScreening(movieSelected, screeningNo);
                    if (screeningSelected == null)
                    {
                        Console.WriteLine("The selected screening number does not exist for this movie. Please try again.");

                    }
                    else if (screeningSelected.SeatsRemaining == 0) 
                    {
                        Console.WriteLine("This screening has no more seats left. Please try another screening.");
                    }
                    else
                    {
                        break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("You did not enter a valid screening number. Please try again.");
                }

            }

            int ticketCount; 

            while (true)
            {
                try
                {
                    Console.Write("\nEnter total number of tickets to order: ");
                    ticketCount = Convert.ToInt32(Console.ReadLine());
                    int maxCapacity = screeningSelected.Cinema.Capacity;
                    string scCinema = screeningSelected.Cinema.Name;
                    int hallNo = screeningSelected.Cinema.HallNo;
                    if (ticketCount > screeningSelected.SeatsRemaining) // Checks if ticket count exceeds seats remaining.
                    {
                        Console.WriteLine("There are not enough seats in {0}, Hall {1}, for {2} tickets. Please purchase only up to {3} tickets for this cinema.", scCinema, hallNo, ticketCount, screeningSelected.SeatsRemaining);
                    }
                    else if (ticketCount <= 0)
                    {
                        Console.WriteLine("Please enter at least 1 ticket.");
                    }
                    else {
                        break;
                    }
              
                }
                catch (FormatException)
                {
                    Console.WriteLine("You did not enter a number. Please try again.");
                }
            }

            if (movieSelected.Classification != "G") // Checks if movie is rated G. If it is not, program will ask user if all ticket holders meet movie's age requirements.
            {
                while (true)
                {
                    try
                    {
                        Console.Write("\nMovie classification: {0}\nDo all ticket holders meet the movie classification requirements? (Yes/No): ", movieSelected.Classification);
                        string choice = Console.ReadLine().ToUpper();
                        if (choice == "NO")
                        {
                            Console.WriteLine("Not all people meet the movie classification requirements.");
                            return;
                        }
                        else if (choice == "YES")
                        {
                            break;
                        }

                        else
                        {
                            Console.WriteLine("You did not enter a valid option. Please try again.");
                        }
                    }

                    catch (FormatException)
                    {
                        Console.WriteLine("You did not enter a valid option. Please try again.");
                    }
                }

            }
            Order newOrder = new Order();

            newOrder.Status = "Unpaid";

            double price = 0;

            for (int i = 0; i < ticketCount; i++) // For number of tickets entered, program will loop the age prompt code that many times.
            {
                while (true) // Prompts user to enter their age group. 
                {
                    try 
                    {
                        int age;
                        Console.Write("\n1. Student\n2. Senior Citizen\n3. Adult\n\n0. Cancel Order\n\nSelect your age group (number): ");
                        int ageGroup = Convert.ToInt32(Console.ReadLine());

                        //Student

                        if (ageGroup == 1)
                        {
                            string educationLevel;

                            while (true)
                            {
                                try
                                {
                                    Console.Write(" \n1. Primary\n2. Secondary\n3. Tertiary\n\nPlease select your education level (number): "); // Finds the age of student to check if they meet age requirements.
                                    int eduOption = Convert.ToInt32(Console.ReadLine());

                                    if (eduOption == 1)
                                    {
                                        age = 12;
                                        educationLevel = "Primary";
                                        break;
                                    }

                                    else if (eduOption == 2)
                                    {
                                        educationLevel = "Secondary";

                                        Console.Write("\nPlease enter your age: ");
                                        age = Convert.ToInt32(Console.ReadLine());
                                        if (age > 17 || age < 13)
                                        {
                                            Console.WriteLine("You have entered an invalid secondary age."); // Prompts for education level again.
                                        }
                                        else
                                        {
                                            break;
                                        }

                                    }

                                    else if (eduOption == 3)
                                    {
                                        educationLevel = "Tertiary";
                                        Console.Write("\nPlease enter your age: ");
                                        age = Convert.ToInt32(Console.ReadLine());
                                        if (age > 30 || age < 12)
                                        {
                                            Console.WriteLine("You have entered an invalid tertiary age."); // Prompts for education level again.
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    else
                                    {
                                        Console.WriteLine("You have entered an invalid input. Please try again.");
                                    }
                                }

                                catch (FormatException)
                                {
                                    Console.WriteLine("You have entered an invalid input. Please try again.");
                                }
                            }

                            // Classification eligibility to check if the students meet the age requirements.

                            if (screeningSelected.Movie.Classification == "PG13")
                            {
                                if (age < 13)
                                {
                                    Console.WriteLine("\nYou are not eligible to watch this movie.");
                                    return;
                                }
                            }

                            else if (screeningSelected.Movie.Classification == "NC16")
                            {
                                if (age < 16)
                                {
                                    Console.WriteLine("\nYou are not eligible to watch this movie.");
                                    return;
                                }
                            }

                            else if (screeningSelected.Movie.Classification == "M18")
                            {
                                if (age < 18)
                                {
                                    Console.WriteLine("\nYou are not eligible to watch this movie.");
                                    return;
                                }
                            }

                            else if (screeningSelected.Movie.Classification == "R21")
                            {
                                if (age < 21)
                                {
                                    Console.WriteLine("\nYou are not eligible to watch this movie.");
                                    return;
                                }
                            }

                            Ticket studentTicket = new Student(screeningSelected, educationLevel); // After verifying if they are eligible, a ticket is made for the student with the selected screening and education level.
                            newOrder.AddTicket(studentTicket); // Adds ticket to current object.
                            screeningSelected.SeatsRemaining -= 1; // Deducts seats remaining for the screening.
                            price += studentTicket.CalculatePrice(); // Calculates price for the Student ticket.
                            break;

                        }

                        //Senior Citizen

                        else if (ageGroup == 2)
                        {
                            while (true)
                            {
                                try
                                {
                                    Console.Write("\nPlease enter your year of birth: ");
                                    int DOB = Convert.ToInt32(Console.ReadLine());
                                    int today = DateTime.Now.Year;
                                    age = today - DOB; // Calculates age

                                    Console.WriteLine("\nYour age is {0}.", age);

                                    if (age >= 55 && age < 125) // If senior citizen's age is eligible
                                    {
                                        Ticket seniorTicket = new SeniorCitizen(screeningSelected, age); // Ticket is for SeniorCitizen.
                                        newOrder.AddTicket(seniorTicket); // Adds ticket to current object.
                                        screeningSelected.SeatsRemaining -= 1; // Deducts seats remaining for the screening.
                                        price += seniorTicket.CalculatePrice(); // Calculates price for the Senior Citizen ticket.
                                        break;
                                    }
                                    else if (age < 55 && age > 10) // If senior citizen's age is not eligible
                                    {
                                        Console.WriteLine("You are not eligible for a senior citizen ticket, only persons aged 55 and above are eligible. \nYou will be charged the standard adult fees.");
                                        Ticket adultTicket = new Adult(screeningSelected, false); // Ticket is for Adult instead of SeniorCitizen.
                                        while (true)
                                        {
                                            try
                                            {
                                                Console.Write("You are eligible for the popcorn offer. Do you want to add popcorn at a discounted rate of $3? (Yes/No): "); // Popcorn offer since it is an adult ticket.
                                                string popcorn = Console.ReadLine().ToUpper();

                                                if (popcorn == "YES")
                                                {
                                                    adultTicket = new Adult(screeningSelected, true);
                                                    break;

                                                }
                                                else if (popcorn == "NO")
                                                {
                                                    adultTicket = new Adult(screeningSelected, false);
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("You have entered an invalid option. Please try again.");
                                                }

                                            }

                                            catch (FormatException)
                                            {
                                                Console.WriteLine("You have entered an invalid option. Please try again.");
                                            }

                                        }

                                        newOrder.AddTicket(adultTicket); // Adds ticket to current object.
                                        screeningSelected.SeatsRemaining -= 1; // Deducts seats remaining for the screening.
                                        price += adultTicket.CalculatePrice(); // Calculates price for the Senior Citizen (Adult) ticket.
                                        break;
                                    }

                                    else if (age < 10 || age > 125) // Unrealistic ages
                                    {
                                        Console.WriteLine("You have entered an unrealistic age. Please try again."); // Prompts for year of birth again.
                                    }

                                    else
                                    {
                                        Console.WriteLine("You have entered an invalid option. Please try again.");
                                    }

                                }

                                catch (FormatException)
                                {
                                    Console.WriteLine("You have entered an invalid year. Please try again.");
                                }
                            }
                            break;

                        }

                        //Adult

                        else if (ageGroup == 3)
                        {
                            Ticket adultTicket = new Adult(screeningSelected, false);
                            while (true)
                            {
                                try
                                {
                                    Console.Write("You are eligible for the popcorn offer. Do you want to add popcorn at a discounted rate of $3? (Yes/No): "); // Popcorn offer
                                    string popcorn = Console.ReadLine().ToUpper();

                                    if (popcorn == "YES")
                                    {
                                        adultTicket = new Adult(screeningSelected, true);
                                        break;

                                    }
                                    else if (popcorn == "NO")
                                    {
                                        adultTicket = new Adult(screeningSelected, false);
                                        break;
                                    }

                                    else
                                    {
                                        Console.WriteLine("You have entered an invalid option. Please try again.");
                                    }

                                }

                                catch (FormatException)
                                {
                                    Console.WriteLine("You have entered an invalid option. Please try again.");
                                }

                            }

                            newOrder.AddTicket(adultTicket); // Adds ticket to current object.
                            screeningSelected.SeatsRemaining -= 1; // Deducts seats remaining for the screening.
                            price += adultTicket.CalculatePrice(); // Calculates price for the Adult ticket.
                            break;
                        }

                        //Cancels ticket order

                        else if (ageGroup == 0)
                        {
                            while (true)
                            {
                                try
                                {
                                    Console.Write("Are you sure you want to cancel this order? (Yes/No): ");
                                    string choice = Console.ReadLine().ToUpper();

                                    if (choice == "YES")
                                    {
                                        return;

                                    }
                                    else if (choice == "NO")
                                    {
                                        break;
                                    }

                                    else
                                    {
                                        Console.WriteLine("You have entered an invalid option. Please try again.");
                                    }

                                }

                                catch (FormatException)
                                {
                                    Console.WriteLine("You have entered an invalid option. Please try again.");
                                }

                            }
                        }

                        else
                        {
                            Console.WriteLine("You did not enter a valid option. Please try again.");
                        }
                    }

                    catch (FormatException) 
                    {
                        Console.WriteLine("You did not enter a valid option. Please try again.");
                    }
                }

            }


            Console.WriteLine("\nAmount payable: ${0}\n", price);

            Console.Write("Press any key to make payment.");
            string randomStr = Convert.ToString(Console.ReadLine());

            newOrder.Amount = price; // Changes Amount of the order.

            newOrder.Status = "Paid"; // Changes Status of the order.

            int orderNo = (orderList.Count) + 1; // Assigns order number based on number of order objects in the order list.

            orderList.Add(newOrder); // Adds order object to order list.

            newOrder.OrderNo = orderNo; // Changes OrderNo of the order.

            Console.WriteLine("\nPayment successful. Your order number is {0}. Have a nice day!", newOrder.OrderNo);

        }

        static void RemoveOrderTicket(List<Order> orderList)
        {
            int statusCheck = 0;

            foreach (Order o in orderList) // Checks if all orders in order list have been cancelled. If all orders have been cancelled, statusCheck will be 0.
            {
                if (o.Status != "Cancelled")
                {
                    statusCheck += 1;
                }

            }

            if (statusCheck == 0 || orderList.Count == 0)
            {
                Console.WriteLine("\nThere are no orders to cancel.");
                return;
            }

            while (true)
            {
                try
                {
                    Console.Write("\nPlease enter your order number: ");
                    int orderNo = Convert.ToInt32(Console.ReadLine());

                    Order selectedOrder = SearchOrder(orderList, orderNo);

                    if (selectedOrder != null) { // Checks if such an order with the entered order number exists. 
                        Screening orderScreening = SearchScreeningFromOrder(orderList, orderNo);

                        DateTime today = DateTime.Now;

                        string movieTitle = orderScreening.Movie.Title;

                        if (orderScreening.ScreeningDateTime < today) // If screening has already taken place.
                        {
                            Console.WriteLine("Cancellation unsuccessful. The order for screening {0} of {1} cannot be cancelled as it has already taken place.", orderScreening.ScreeningNo, movieTitle);
                            return;
                        }

                        else if (selectedOrder.Status == "Cancelled") // If order is already cancelled.
                        {
                            Console.WriteLine("Cancellation unsuccessful. The order you were trying to cancel has already been cancelled.");
                            return;
                        }

                        else
                        {
                            int seatsAdd = 0;
                            foreach (Ticket t in selectedOrder.TicketList) // Finds number of seats remaining based on number of tickets purchased in the order.
                            {
                                seatsAdd += 1;

                            }

                            orderScreening.SeatsRemaining += seatsAdd; // Adds back seats to screening.

                            selectedOrder.Status = "Cancelled"; // Changes Status of the order to Cancelled.

                            double orderAmt = selectedOrder.Amount;


                            Console.WriteLine("\nYour order (Order No. {0}) of ${1:0.00} for {2} has successfully been cancelled. Have a nice day!", orderNo, orderAmt, movieTitle);
                            break;
                        }

                    }

                    else
                    {
                        Console.WriteLine("You have entered an invalid order number.");
                        return;
                    }

                }
                
                catch (FormatException) {
                    Console.WriteLine("You have entered an invalid input. Please try again.");
                } 
                
            }
                

        }
        

    }
}
