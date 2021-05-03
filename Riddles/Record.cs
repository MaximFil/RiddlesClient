using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddles
{
    public class Record
    {
        [DisplayName("Имя")]
        public string Name { get; set; }

        [DisplayName("Минуты")]
        public int Minutes { get; set; }

        [DisplayName("Секунды")]
        public int Seconds { get; set; }

        [DisplayName("Очки")]
        public int Points { get; set; }
    }
}
