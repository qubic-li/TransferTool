using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace li.qubic.transfertool
{
    public class SubmitTransactionRequest
    {
        public byte[] SignedTransaction { get; set; }
    }
}
