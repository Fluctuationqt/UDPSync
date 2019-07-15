using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UDPObjectSync.UDPSync.Serialization;

namespace UDPObjectSync.UDPSync.UDP
{
    class ObjectSyncServer<T>
    { 
        private int syncRate = 100; // sync every x milliseconds
        private CancellationTokenSource cts;
        private UDPServer server;
        private Serializer serializer;
        private SyncObjectRef<T> syncObject;

        public ObjectSyncServer(SyncObjectRef<T> syncObject, string address, int port, int rate)
        {
            this.syncObject = syncObject;

            server = new UDPServer(address, port);
            serializer = new Serializer();
            syncRate = rate;
            
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            StartSync(token);
        }

        ~ObjectSyncServer()
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
            if (server.ReceivedData != null)
            {
                syncObject.Value = serializer.Deserialize<T>(server.ReceivedData);
            }
        }
    }
}
