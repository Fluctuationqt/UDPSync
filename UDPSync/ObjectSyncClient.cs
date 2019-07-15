using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UDPObjectSync.UDPSync.Serialization;
using UDPObjectSync.UDPSync.UDP;

namespace UDPObjectSync.UDPSync
{
    class ObjectSyncClient<T>
    {
        private T syncObject;
        private int syncRate = 100; // sync every x milliseconds
        private CancellationTokenSource cts;
        private UDPClient client;
        private Serializer serializer;

        public ObjectSyncClient(ref T syncObject, string address, int port, int rate)
        {
            this.syncObject = syncObject;
            client = new UDPClient(address, port);
            serializer = new Serializer();
            syncRate = rate;
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            StartSync(token);   
        }

        ~ObjectSyncClient()
        {
            cts.Cancel();
        }

        private async Task StartSync(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    SyncObject();
                    await Task.Delay(syncRate, token);
                    if (token.IsCancellationRequested)
                        break;
                }
            });
        }

        private void SyncObject()
        {
            Console.WriteLine("Sending:{0}", (syncObject as ExampleObject).data);
            client.Send(serializer.Serialize<T>(syncObject));
        }
    }
}
