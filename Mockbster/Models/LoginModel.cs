namespace Mockbster.Models;

public class LoginModel
{
    public UserModel UserData { get; set; } = new UserModel { 
        Firstname= "",
        Lastname= "",
        Username= "",
        Password= "",
        Email= ""
    };
    public string ErrorMessage { get; set; } = "";
}