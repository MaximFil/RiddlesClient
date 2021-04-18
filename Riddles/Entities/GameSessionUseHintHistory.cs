using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddles.Entities
{
    public class GameSessionUseHintHistory
    {
        public int Id { get; set; }

        public int GameSessionId { get; set; }

        public int? UserId { get; set; }

        public int? RiddleId { get; set; }

        public int? HintId { get; set; }

        public string OldAnswerValue { get; set; }

        public string NewAnswerValue { get; set; }

        public DateTime UseDate { get; set; }
    }
}
