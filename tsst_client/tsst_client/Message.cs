using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tsst_client
{
    [Serializable]
    public class Message
    {
        public string s;
        public int inti;
      
        public Message(string s, int inti)
        {
            this.s = s;
            this.inti = inti;
        }

        public Message()
        {
        }

    }
}
