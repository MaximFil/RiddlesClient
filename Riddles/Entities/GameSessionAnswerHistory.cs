using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddles.Entities
{
    public class GameSessionAnswerHistory
    {
        public int Id { get; set; }

        public int GameSessionId { get; set; }

        public int? UserId { get; set; }

        public int? RiddleId { get; set; }

        public string Answer { get; set; }

        public bool Correct { get; set; }

        public DateTime AnswerDate { get; set; }
    }
}
