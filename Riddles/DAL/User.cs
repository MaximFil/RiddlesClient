using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddles.DAL
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Record> Records { get; set; }
        public List<Riddle> Riddles { get; set; }
        public User ()
        {
            Records = new List<Record>();
            Riddles = new List<Riddle>();
            
        }
    }
}
