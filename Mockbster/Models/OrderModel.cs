using System.ComponentModel.DataAnnotations;

namespace Mockbster.Models;

public class OrderModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MovieId { get; set; }
    [DataType(DataType.Date)]
    [Display(Name = "Rental begin date")]
    public DateTime OrderBegin { get; set; }
    [DataType(DataType.Date)]
    [Display(Name = "Rental end date")]
    public DateTime OrderEnd { get; set; }
    [Display(Name = "Rental price")]
    public double? PaidAmount { get; set; }
}