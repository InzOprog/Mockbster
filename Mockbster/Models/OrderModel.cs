using System.ComponentModel.DataAnnotations;

namespace Mockbster.Models;

public class OrderModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MovieId { get; set; }
    [DataType(DataType.Date)]
    public DateTime OrderBegin { get; set; }
    [DataType(DataType.Date)]
    public DateTime OrderEnd { get; set; }
    [DataType(DataType.Date)]
    public DateTime? ProductReturn { get; set; }
}