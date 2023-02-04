using System.Runtime.InteropServices;
using System.Text;

namespace li.qubic.transfertool
{

    /// <summary>
    /// C# Wrapper for the C++ qubic.li Library
    /// </summary>
    public class QubicHelper
    {
        [DllImport(@"libQubicHelper", EntryPoint = "signTransferToBroadcastExported")]
        private static extern bool SignTransferToBroadcastDll(string seed, Transaction transfer, byte[] signature);
        [DllImport("libQubicHelper", EntryPoint = "GetIdentityFromSeedExported")]
        private static extern bool GetIdentityFromSeedDll(string seed, byte[] identity);
        [DllImport("libQubicHelper", EntryPoint = "GetPublicKeyFromIdentityExported")]
        static extern bool GetPublicKeyFromIdentity(string computor, byte[] publicKey);

        public string GetIdentityFromSeed(string seed)
        {
            byte[] identity = new byte[60];
            GetIdentityFromSeedDll(seed, identity);
            return Encoding.ASCII.GetString(identity);
        }

        public byte[]? GetPublicKeyFromIdentity(string identity)
        {
            byte[] publicKey = new byte[32];
            if (GetPublicKeyFromIdentity(identity, publicKey))
            {
                return publicKey;
            }
            return null;
        }

        public byte[] SignTransferToBroadcast(string seed, Transaction transfer)
        {
            byte[] signature = new byte[64];
            SignTransferToBroadcastDll(seed, transfer, signature);
            return signature;
        }

        public static T ByteToType<T>(BinaryReader reader)
        {
            var size = Marshal.SizeOf(typeof(T));
            byte[] bytes = reader.ReadBytes(size);

            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return theStructure;
        }
    }
}