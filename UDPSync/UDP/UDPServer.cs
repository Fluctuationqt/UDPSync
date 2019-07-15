using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UDPObjectSync.UDPSync.UDP
{
    
    class UDPServer
    {
        private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
        private const int bufSize = 8 * 1024; // max serialized object byte size
        private AsyncCallback recv = null;
        private State state = new State();
        private byte[] receivedData = null;

        public byte[] ReceivedData
        {
            get
            {
                return receivedData;
            }
        }

        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }

        public UDPServer(string address, int port)
        {
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            
            Listen();
        }

        private void Listen()
        {
            recv = new AsyncCallback(ReceiveCallback);
            _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, state);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            // chunk of buffer size received store it in received data
            State state = (State)ar.AsyncState;
            int receivedBytes = _socket.EndReceiveFrom(ar, ref epFrom);
            receivedData = new byte[receivedBytes];
            Array.Copy(state.buffer, 0, receivedData, 0, receivedBytes);
            Console.WriteLine("Received {0} bytes of maximum {1} bytes.", receivedBytes, state.buffer.Length);
            
            // listen for another chunk
            _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, state);
        }
    }
}
