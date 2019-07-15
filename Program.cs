using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        static void Main(string[] args)
        {
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
        }
    }
}
