using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace Riddles.DAL
{
    public class Context : DbContext
    {
        public Context() : base("Connection")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Riddle> Riddles { get; set; }
        public DbSet<Record> Records { get; set; }
        
    }
}
