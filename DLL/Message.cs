﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    [DataContract]
    public class Message
    {
        public Message(string text, string from, string to)
        {
            this.Time = DateTime.Now;
            this.From = from;
            this.To = to;
            this.Text = text;
            this.Attachemnts = null; // need to implement
        }
        [DataMember]
        public DateTime Time { get; set; }

        [DataMember]
        public string From { get; set; }

        [DataMember]
        public string To { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public byte[] Attachemnts { get; set; }

        public void setAttachment(byte[] attachment)
        {
            Attachemnts = attachment;
        }
    }
}
