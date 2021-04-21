using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Riddles
{
    public static class UserProfile
    {
        public static int Id { get; set; }

        public static string Login { get; set; }

        public static string Password { get; set; }

        public static Entities.Level Level { get; set; }

        public static int GamaSessionId { get; set; }

        public static string RivalName { get; set; }

        public static ICloseble CurrentForm { get; set; } 
    }
}
