using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddles.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public bool IsPlaying { get; set; }

        public User(bool isActive = false, bool isPlaying = false)
        {
            this.IsActive = isActive;
            this.IsPlaying = isPlaying;
        }
    }
}
