using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerClient.Models
{
    public class Message
    {
        public int MessageID { get; set; }

        public int ChatroomID { get; set; }

        public int SenderID { get; set; }

        public DateTime SentDate { get; set; }

        public string Text { get; set; }

        public string Image { get; set; }
    }
}
