using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UDPObjectSync.UDPSync;
using UDPObjectSync.UDPSync.Serialization;
using UDPObjectSync.UDPSync.UDP;

namespace UDPObjectSync
{
    [Serializable]
    class ExampleObject
    { 
        public string data { get; set; }
        public ExampleObject(string data)
        {
            this.data = data;
        }
        public ExampleObject()
        {
            this.data = null;
        }
    }

    class Program
    {
        private const int SYNC_RATE = 10;
        static void Main(string[] args)
        {
            Console.Write("Address: ");
            string address = Console.ReadLine();
            Console.Write("Port: ");
            int port = int.Parse(Console.ReadLine());
            
            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine("1. Press 1 to start as server.");
                Console.WriteLine("2. Press 2 to start as client.");
                key = Console.ReadKey().Key;
               
            } while (key != ConsoleKey.D1 && key != ConsoleKey.D2);


            if (key == ConsoleKey.D1)
            {
                Console.WriteLine("Starting Server");
                ServerExample(address, port, SYNC_RATE);
            }
            else if(key == ConsoleKey.D2)
            {
                Console.WriteLine("Starting Client");
                ClientExample(address, port, SYNC_RATE);
            }
        }

        static void ServerExample(string address, int port, int syncRate)
        {
            ExampleObject myObject
                   = new ExampleObject("Default");
            SyncObjectRef<ExampleObject> myObjectRef
                = new SyncObjectRef<ExampleObject>(myObject);
            ObjectSyncServer<ExampleObject> sserver
                = new ObjectSyncServer<ExampleObject>(myObjectRef, address, port, syncRate);

            while (true)
            {
                // set myObject in current thread to received object from network and print it 
                // TODO: (Interpolate between positions here)
                myObject = myObjectRef.Value;
                Console.WriteLine("Current Object: {0}", myObject.data);
            }
        }

        static void ClientExample(string address, int port, int syncRate)
        {
            ExampleObject myObject
                    = new ExampleObject("Whatever");
            ObjectSyncClient<ExampleObject> sclient
                = new ObjectSyncClient<ExampleObject>(ref myObject, address, port, syncRate);

            while (true)
            {
                // modify myObject's fields in current thread and print it constantly;
                myObject.data = DateTime.Now.ToString("HH:mm:ss.fff");
                Console.WriteLine("Modified Object to: {0}", myObject.data);
            }
        }
    }
}


/*
            ExampleObject localObject = new ExampleObject("Whatever");
            ExampleObject remoteObject = new ExampleObject();

            UDPClient client = new UDPClient("127.0.0.1", 5555);
            UDPServer server = new UDPServer("127.0.0.1", 5555);
            Serializer serializer = new Serializer();            

            while (true)
            {
                // Modify local object
                localObject.data = DateTime.Now.ToString("HH:mm:ss.fff");

                // Serialize and send local object
                client.Send(serializer.Serialize<ExampleObject>(localObject));

                // Receive and deserialize object
                if (server.ReceivedData != null)
                {
                    remoteObject = serializer.Deserialize<ExampleObject>(server.ReceivedData);
                    Console.WriteLine("Current Remote Object: {0}", remoteObject.data);
                }

                //System.Threading.Thread.Sleep(100);
                
            }
            */
