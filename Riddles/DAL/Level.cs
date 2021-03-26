﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddles.DAL
{
    public class Level
    {
        public int Id { get; set; }
        public string LevelName { get; set; }
        public List<Riddle> Riddles { get; set; }
        public List<Record> Records { get; set; }
        public Level()
        {
            Riddles = new List<Riddle>();
            Records = new List<Record>();
        }
    }
   
}
