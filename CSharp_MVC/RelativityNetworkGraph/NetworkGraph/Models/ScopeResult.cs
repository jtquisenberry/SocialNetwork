using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetworkGraph.Models
{
    
    public class ScopeResult
    {
        public List<Recipient> Recipients = new List<Recipient>();
        public List<Sender> Senders = new List<Sender>();
        public List<SenderRecipientPair> Pairs = new List<SenderRecipientPair>();

        public ScopeResult()
        {

        }

    }
}