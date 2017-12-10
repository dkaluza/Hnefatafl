using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hnefatafln
{
    public class Server
    {
        static IPAddress localaddr;
        TcpListener tcp;

        public Server(string address)
        {
            localaddr = IPAddress.Parse(address);
            tcp = new TcpListener(localaddr, 10011);
        }

        public void RunServer()
        {
            tcp.Start();
            while (true)
            {
                var player1 = tcp.AcceptSocket();
                var player2 = tcp.AcceptSocket();
                var t = new Thread(() => HandleGame(player1, player2));
                t.Start();
            }
        }

        public void HandleGame(Socket player1, Socket player2)
        {
            Byte[] buffer = new Byte[4];
            buffer[0] = 1;
            player1.Send(buffer, 1, SocketFlags.None);
            buffer[0] = 0;
            player2.Send(buffer, 1, SocketFlags.None);
            try
            {
                while (player1.Connected && player2.Connected)
                {
                    player2.Receive(buffer);
                    player1.Send(buffer);
                    player1.Receive(buffer);
                    player2.Send(buffer);
                }
            }
            catch (Exception e)
            {

            }
        }

        static void Main()
        {
            Server s = new Server("127.0.0.1");
            s.RunServer();
        }
    }
}
