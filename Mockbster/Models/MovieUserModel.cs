using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mockbster.Models;

public class MovieUserModel
{
    public List<MovieModel>? Movies { get; set; }
    public SelectList? Genres { get; set; }
    public string? MovieGenre { get; set; }
    public string? movieTitle { get; set; }
    public string? maxPrice { get; set; }
}