using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddles.Entities
{
    public class Riddle
    {
        public int Id { get; set; }

        public int LevelId { get; set; }
        public Level Level { get; set; }

        public string Text { get; set; }

        public string Answer { get; set; }
    }
}
