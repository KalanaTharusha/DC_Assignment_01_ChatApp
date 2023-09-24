using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public class Message
    {
        public Message()
        {

        }
        public DateTime Time { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Text { get; set; }
        public string Attachemnts { get; set; }
    }
}
