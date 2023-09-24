using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public class User
    {
        public User()
        {
            UserID = Guid.NewGuid().ToString();
        }
        public string UserID { get; set; }
        public string UserName { get; set; }

    }
}
