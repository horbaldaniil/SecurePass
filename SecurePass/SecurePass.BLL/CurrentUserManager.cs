using SecurePass.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
