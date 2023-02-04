using System.Runtime.InteropServices;

namespace li.qubic.transfertool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("qubic.li Simple Transfer Tool");
            Console.WriteLine("Syntax: transfertool.exe <SOURCESEED> <DESTINATIONID> <AMOUNT> [TICK]");
            Console.WriteLine("Example: transfertool.exe sdlkjljklskjdflksjdflksjfdsjdfsdf LKDJFLKJDFLKJDSFJKLJKDFKJ 100");
            Console.WriteLine("If Tick is not defined it will use current tick + 10 as target tick for transaction");

            Console.Write("\n\nSign in to qubic.li...");
            var apiHelper = new QubicApiClient("https://test.qubic.li");
            Console.Write("done!");

            string sourceSeed = "", targetId = "", amount = "", tick = "";

            if(args.Length < 3)
            {
                Console.Write("Please Enter Source Seed:");
                sourceSeed = Console.ReadLine();

                Console.Write("Please Enter Destination Id:");
                targetId = Console.ReadLine();

                Console.Write("Please Enter Amount:");
                amount = Console.ReadLine();

                Console.Write("Please Enter Tick (empty for autotick):");
                tick = Console.ReadLine();
            }else
            {
                sourceSeed = args[0];
                targetId = args[1];
                amount = args[2];
                if(args.Length > 3)
                {
                    tick = args[3];
                }
            }

            if(string.IsNullOrEmpty(sourceSeed) || sourceSeed.Length != 55)
            {
                Console.WriteLine("Invalid Source Seed");
                return;
            }

            if (string.IsNullOrEmpty(targetId) || targetId.Length != 60)
            {
                Console.WriteLine("Invalid Destination ID");
                return;
            }

            int energyTosend = 0;
            if (string.IsNullOrEmpty(amount) || !int.TryParse(amount, out energyTosend))
            {
                Console.WriteLine("Invalid Amount");
                return;
            }

            int targetTick = 0;
            if (string.IsNullOrEmpty(tick) || !int.TryParse(tick, out targetTick))
            {
                // try getting current Tick from API
                var apiResponse = apiHelper.GetFromApi<CurrentTickResponse>("public/currentTick").GetAwaiter().GetResult();
                if(apiResponse != null)
                {
                    targetTick = apiResponse.Tick + 10;
                }
                else
                {
                    Console.WriteLine("Invalid Destination ID");
                    return;
                }
            }

            Console.WriteLine($"\n\nPreparing Transaction of {energyTosend} quos to {targetId} for tick {targetTick}");

            var qubicHelper = new QubicHelper();

            var transfer = new SignedTransaction();
            transfer.transaction.amount = energyTosend;
            transfer.transaction.tick = (uint)targetTick;
            transfer.transaction.inputSize = 0;
            transfer.transaction.inputType = 0;
            transfer.transaction.sourcePublicKey = qubicHelper.GetPublicKeyFromIdentity(qubicHelper.GetIdentityFromSeed(sourceSeed));
            transfer.transaction.destinationPublicKey = qubicHelper.GetPublicKeyFromIdentity(targetId);
            transfer.signature = qubicHelper.SignTransferToBroadcast(sourceSeed, transfer.transaction);
            
            Console.Write($"\n\nSend transaction to qubic.li API...");

            var submitResponse = apiHelper.PostToApi<SubmitTransactionResponse, SubmitTransactionRequest>("public/submittransaction", new SubmitTransactionRequest()
            {
                SignedTransaction = Marshalling.Serialize(transfer)
            }).GetAwaiter().GetResult();
            Console.Write($"done!");

            if (submitResponse != null)
            {
                Console.WriteLine($"\n\nYour Transaction has been stored for propagation");
            }else
            {
                Console.WriteLine($"\n\nThere was an error storing your transaction for propagation. Try again later.");
            }

        }
    }

    public static class Marshalling
    {
        public static byte[] Serialize<T>(T s)
            where T : struct
        {
            var size = Marshal.SizeOf(typeof(T));
            var array = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(s, ptr, true);
            Marshal.Copy(ptr, array, 0, size);
            Marshal.FreeHGlobal(ptr);
            return array;
        }
    }
}
