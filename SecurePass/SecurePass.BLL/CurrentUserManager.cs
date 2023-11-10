using SecurePass.DAL.Model;

namespace SecurePass.BLL;

public class CurrentUserManager
{
    public static UserModel? CurrentUser { get; private set; }

    public static void SetCurrentUser(UserModel user)
    {
        CurrentUser = user;
    }

    public static void ClearCurrentUser()
    {
        CurrentUser = null;
    }

}
