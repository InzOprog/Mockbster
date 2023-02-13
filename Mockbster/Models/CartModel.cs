using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mockbster.Models;

public class CartModel
{  
    // Tuple<id, amount, Model>
    public List<Tuple<int, int, MovieModel>>? Movies { get; set; }
}
