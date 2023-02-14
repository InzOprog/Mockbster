using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mockbster.Models;

public class CartModel
{
    // Tuple<id, amount, Model>
    public List<Tuple<int, int, MovieModel>?> Movies { get; set; }
    public decimal Total { get; set; }

    public CartModel() 
    {
        Movies = new List<Tuple<int, int, MovieModel>?>();
        Total = 0;
    }

    public decimal ItemPrice(Tuple<int, int, MovieModel> item)
    {
        return item.Item2 * item.Item3.Price;
    }
}
