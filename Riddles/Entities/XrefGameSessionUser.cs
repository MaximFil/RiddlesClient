using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddles.Entities
{
    public class XrefGameSessionUser
    {
        public int GameSessionID { get; set; }

        public int UserId { get; set; }

        public string TotalTime { get; set; }

        public int Points { get; set; }

        public string Result { get; set; }

        public bool Surrendered { get; set; }

        public bool Finished { get; set; }
    }
}
