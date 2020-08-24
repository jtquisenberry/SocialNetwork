using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetworkGraph.Models
{
    public class SenderRecipientPair
    {
        public Int64 SenderEntityID = 0;
        public string SenderDisplay = "";
        public Int64 RecipientEntityID = 0;
        public string RecipientDisplay = "";
        public int Count = 0;

        public SenderRecipientPair()
        {

        }
    }
}