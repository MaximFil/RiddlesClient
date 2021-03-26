using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Riddles.DAL
{
    public class Record
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int LevelId { get; set; }
        public Level Level { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int TotalTime { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
