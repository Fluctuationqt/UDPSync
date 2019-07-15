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
    class Position
    {
        public int x { get; set; }
        public int y { get; set; }
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Position()
        {
            x = y = 0;
        }
        public override string ToString()
        {
            return $"Position({x},{y})";
        }
    }

    class Program
    {
        private const int SYNC_RATE = 10; // Delay in milliseconds between each network update
        private const int GAME_RATE = 25; // Game FPS
        static void Main(string[] args)
        {
            Console.Write("Address: ");
            string address = Console.ReadLine();
            Console.Write("Port: ");
            int port = int.Parse(Console.ReadLine());
            
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Press 1 to start as server.");
                Console.WriteLine("2. Press 2 to start as client.");
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        ServerExample(address, port, SYNC_RATE);
                        break;
                    case ConsoleKey.D2:
                        ClientExample(address, port, SYNC_RATE);
                        break;
                    default:
                        Console.WriteLine(" <-- Wrong Key!");
                        break;
                }
                Thread.Sleep(500);
            }
        }

        static void ServerExample(string address, int port, int syncRate)
        {
            // Networked Object
            var hisPos = new Position();
            var hisPosRef = new SyncObjectRef<Position>(hisPos);
            var sserver = new ObjectSyncServer<Position>(hisPosRef, address, port, syncRate);
            
            // Server Game Loop
            while (true)
            {
                hisPos = hisPosRef.Value; // local pos = received pos (Interpolate positions here)
                Console.WriteLine("Server: " + hisPos.ToString());
                DrawGame(hisPos, GAME_RATE);
            }
        }

        static void ClientExample(string address, int port, int syncRate)
        {
            // Networked Object
            var myPos = new Position(0, 0);
            var sclient = new ObjectSyncClient<Position>(ref myPos, address, port, syncRate);
            
            // Client Game Loop
            while (true)
            {
                Console.WriteLine("Client: " + myPos.ToString());
                MovementHanlder(ref myPos); // Updates networked object
                DrawGame(myPos, GAME_RATE);
            }
        }

        static void MovementHanlder(ref Position p)
        {
            if (System.Console.KeyAvailable)
            {
                switch (System.Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow: p.y--; break;
                    case ConsoleKey.DownArrow: p.y++; break;
                    case ConsoleKey.LeftArrow: p.x--; break;
                    case ConsoleKey.RightArrow: p.x++; break;
                }
            }
        }

        static void DrawGame(Position p, int fps)
        {
            Console.CursorVisible = false;
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    char c = (p.x == x && p.y == y) ? '*' : ' ';
                    Console.Write(c);
                }
                Console.WriteLine();
            }
            Thread.Sleep(1000/fps);
            Console.Clear();
        }
    }
}