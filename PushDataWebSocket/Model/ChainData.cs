using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushDataWebSocket.Model
{
    //todo: wait front end data format
    public struct ChainData
    {
        public long Timestamp { get; set; }

        public int TransactionNumber { get; set; }
    }
}
