using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddles.Helpers
{
    public class HubHelper
    {
        public void SendInvite(string userName)
        {
            var inviteForm = new Invite(userName);
            inviteForm.Show();
        }
    }
}
