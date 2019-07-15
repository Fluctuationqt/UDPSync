using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPObjectSync.UDPSync.UDP
{
    class UDPClient
    {
        private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
        private AsyncCallback recv = null;
        private const int bufSize = 8 * 1024;
        private State state = new State();

        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }

        public UDPClient(string address, int port)
        {
            _socket.Connect(IPAddress.Parse(address), port);
        }

        public void Send(byte[] data)
        {
            _socket.BeginSend(data, 0, data.Length, SocketFlags.None, SentCallback, state);
        }

        private void SentCallback(IAsyncResult ar)
        {
            State so = (State)ar.AsyncState;
            int bytes = _socket.EndSend(ar);
        }
    }
}
