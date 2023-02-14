namespace Mockbster.Models;

public class UserPageModel
{
    public UserModel? UserData { get; set; }
    public List<Tuple<string, OrderModel>?> UserHistory { get; set; }

    public UserPageModel() 
    {
        UserHistory = new List<Tuple<string, OrderModel>?>();
    }
}