using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QKitSampleApp.Models
{
    public class MovieModel
    {
        public static List<MovieModel> CreateSampleData()
        {
            var data = new List<MovieModel>();

            data.Add(new MovieModel("Iron Man", "Action"));
            data.Add(new MovieModel("The Notebook", "Romance"));
            data.Add(new MovieModel("Terminator", "Action"));
            data.Add(new MovieModel("Friday the 13th", "Horror"));
            data.Add(new MovieModel("Shaun of the Dead", "Comedy"));
            data.Add(new MovieModel("The Conjuring", "Horror"));
            data.Add(new MovieModel("Zombieland", "Comedy"));
            data.Add(new MovieModel("Pretty Woman", "Romance"));
            data.Add(new MovieModel("21 Jump Street", "Comedy"));
            data.Add(new MovieModel("Batman Begins", "Action"));
            data.Add(new MovieModel("Titanic", "Drama"));
            data.Add(new MovieModel("Monty Python and the Holy Grail", "Comedy"));
            data.Add(new MovieModel("The Matrix", "Action"));
            data.Add(new MovieModel("Planes, Trains & Automobiles", "Comedy"));
            data.Add(new MovieModel("Scream", "Horror"));
            data.Add(new MovieModel("The Princess Bride", "Romance"));
            data.Add(new MovieModel("Transformers", "Action"));
            data.Add(new MovieModel("Saw", "Horror"));
            data.Add(new MovieModel("Back to the Future", "Action"));
            data.Add(new MovieModel("Casablanca", "Romance"));
            data.Add(new MovieModel("West Side Story", "Romance"));
            data.Add(new MovieModel("Texas Chainsaw Massacre", "Horror"));

            return data;
        }

        public MovieModel(string name, string category)
        {
            Name = name;
            Category = category;
        }

        public string Name { get; set; }

        public string Category { get; set; }
    }
}
