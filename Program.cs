using System;
using NLog.Web;
using System.IO;
using System.Linq;

namespace MediaLibrarywSearch
{
    class Program
    {

        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");

            string movieFilePath = Directory.GetCurrentDirectory() + "\\movies.csv";

            MovieFile movieFile = new MovieFile(movieFilePath);

            string choice = "";
            do
            {
                // display choices to user
                Console.WriteLine("1) Add Movie");
                Console.WriteLine("2) Display All Movies");
                Console.WriteLine("3) Find Movie");
                Console.WriteLine("Enter to quit");
                // input selection
                choice = Console.ReadLine();
                logger.Info("User choice: {Choice}", choice);

//make a choice to find a movie - display both the 
//results as well as the number of matches (can probably use the "count" thing in linq)

                if (choice == "1")
                {
                    // Add movie
                    //Media movie = new Media.Movie();
                    //Movie movie1 = new Movie();
                    var movie = new Media.Movie();
                    // ask user to input movie title
                    Console.WriteLine("Enter movie title");
                    // input title
                    movie.title = Console.ReadLine();
                    // verify title is unique
                    if (movieFile.isUniqueTitle(movie.title)){
                        // input genres
                        string input;
                        do
                        {
                            // ask user to enter genre
                            Console.WriteLine("Enter genre (or done to quit)");
                            // input genre
                            input = Console.ReadLine();
                            // if user enters "done"
                            // or does not enter a genre do not add it to list
                            if (input != "done" && input.Length > 0)
                            {
                                movie.genres.Add(input);
                            }
                        } while (input != "done");
                        // specify if no genres are entered
                        if (movie.genres.Count == 0)
                        {
                            movie.genres.Add("(no genres listed)");
                        }

                        Console.WriteLine("Enter movie director");
                        movie.director = Console.ReadLine();

                        Console.WriteLine("Enter running time (h:m:s)");
                        string runTime = Console.ReadLine();
                        movie.runningTime = TimeSpan.Parse(runTime);
                        //movie.rTime = runTime;
                        

                        // add movie
                        movieFile.AddMovie(movie);
                        
                    }
                } else if (choice == "2")
                {
                    string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");
                    logger.Info(scrubbedFile);
                    // Display All Movies
                    foreach(Media.Movie m in movieFile.Movies)
                    {
                        Console.WriteLine(m.Display());
                    }
                }
                else if (choice == "3")
                {
                    Console.WriteLine("Enter the movie title in which you want to search");
                    string movieSearched = Console.ReadLine();
                    var Movies = movieFile.Movies.Where(m => m.title.Contains(movieSearched));
                    Console.WriteLine("");

                    foreach(Media.Movie m in Movies)
                    {
                        Console.WriteLine($"  {m.title}");
                    }



                    Console.WriteLine($"\nThere are {Movies.Count()} movies with the title {movieSearched}\n");


                }

            } while (choice == "1" || choice == "2" || choice == "3");


            logger.Info("Program ended");
        }
    }
}
