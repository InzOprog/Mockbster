using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mockbster.Data;

namespace Mockbster.Models;

public class dbSeedModel
{
    public static void Initialize(IServiceProvider iserviceProvider)
    {
        using (var context = new MockbsterContext(
            iserviceProvider.GetRequiredService<
                DbContextOptions<MockbsterContext>>()))
        {
            if (context.Movie.Any())
            {
                return;
            }
            context.Movie.AddRange(
                new MovieModel
                {
                    ImgName = "TheShawshankRedemption",
                    Title = "The Shawshank Redemption",
                    ReleaseDate = DateTime.Parse("1994-09-23"),
                    Genre = "Drama, Crime",
                    Rating = "9.3",
                    Price = 8.00M
                },
                new MovieModel
                {
                    ImgName = "PulpFiction",
                    Title = "Pulp Fiction",
                    ReleaseDate = DateTime.Parse("1994-10-14"),
                    Genre = "Crime, Drama",
                    Rating = "9.0",
                    Price = 5.50M
                },
                new MovieModel
                {
                    ImgName = "TheDarkKnight",
                    Title = "The Dark Knight",
                    ReleaseDate = DateTime.Parse("2008-07-16"),
                    Genre = "Action, Crime, Drama",
                    Rating = "9.0",
                    Price = 7.00M
                },
                new MovieModel
                {
                    ImgName = "12AngryMen",
                    Title = "12 Angry Men",
                    ReleaseDate = DateTime.Parse("1957-04-13"),
                    Genre = "Drama",
                    Rating = "8.9",
                    Price = 9.00M
                },
                new MovieModel
                {
                    ImgName = "TheLordOfTheRings",
                    Title = "The Lord of the Rings",
                    ReleaseDate = DateTime.Parse("2001-12-19"),
                    Genre = "Adventure, Drama, Fantasy",
                    Rating = "9.0",
                    Price = 11.50M
                },
                new MovieModel
                {
                    ImgName = "TheGoodTheBadAndTheUgly",
                    Title = "The Good, the Bad and the Ugly",
                    ReleaseDate = DateTime.Parse("1966-12-29"),
                    Genre = "Western",
                    Rating = "8.8",
                    Price = 10.50M
                },
                new MovieModel
                {
                    ImgName = "SchindlersList",
                    Title = "Schindler's List",
                    ReleaseDate = DateTime.Parse("1993-12-15"),
                    Genre = "Biography, Drama, History",
                    Rating = "8.9",
                    Price = 11.50M
                },
                new MovieModel
                {
                    ImgName = "TheLionKing",
                    Title = "The Lion King",
                    ReleaseDate = DateTime.Parse("1994-06-15"),
                    Genre = "Animation, Adventure, Drama",
                    Rating = "8.5",
                    Price = 8.50M
                },
                new MovieModel
                {
                    ImgName = "StarWars",
                    Title = "Star Wars",
                    ReleaseDate = DateTime.Parse("1977-05-25"),
                    Genre = "Action, Adventure, Fantasy",
                    Rating = "8.7",
                    Price = 6.00M
                },
                new MovieModel
                {
                    ImgName = "TheSilenceOfTheLambs",
                    Title = "The Silence of the Lambs",
                    ReleaseDate = DateTime.Parse("1991-02-14"),
                    Genre = "Crime, Drama, Thriller",
                    Rating = "8.6",
                    Price = 7.50M
                }
            );
            context.SaveChanges();
        }
    }
}