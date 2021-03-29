using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddles.Entities
{
    public class GameSession
    {
        public int Id { get; set; }

        public int LevelId { get; set; }
        public Level Level { get; set; }

        public DateTime StartedDate { get; set; }

        public DateTime FinishedDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}
