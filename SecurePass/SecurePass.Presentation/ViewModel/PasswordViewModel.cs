using SecurePass.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurePass.Presentation.ViewModel
{
    public class PasswordViewModel
    {
        public PasswordModel Password { get; set; }
        public bool IsPasswordVisible { get; set; }
    }
}
