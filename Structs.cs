using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace li.qubic.transfertool
{
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct SignedTransaction
    {
        public Transaction transaction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] signature;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct Transaction
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] sourcePublicKey;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] destinationPublicKey;
        public long amount;
        public uint tick;
        public ushort inputType;
        public ushort inputSize;
    }
}
