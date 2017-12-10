using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Hnefatafln
{
    public class NetGame : GameScreen
    {
        TcpClient client = new TcpClient();
        Stream stream;
        bool playerBlack;

        public NetGame(SpriteBatch sp) : base(sp)
        {
        }

        public override void StartGame()
        {
            base.StartGame();
            client.Connect("localhost", 10011);

            stream = client.GetStream();
            Byte[] buffer = new Byte[1];
            stream.Read(buffer, 0, 1);
            if (buffer[0] != 0)
            {
                playerBlack = true;
            }
            else
            {
                playerBlack = false;
            }
        }
        protected override void DoOpponentsTurn()
        {
            if(playerBlack != blackTurn)
            {
                Byte[] buffer = new Byte[4];
                stream.Read(buffer, 0, 4);
                int fromColumn = buffer[0];
                int fromRow = buffer[1];
                int toColumn = buffer[2];
                int toRow = buffer[3];
                clickedPiece = Board[fromColumn, fromRow];
                MoveClickedPiece(toColumn, toRow);
            }
        }
        protected override void SendToOpponent(int previousColumn, int previousRow, int column, int row)
        {
            if(playerBlack == blackTurn)
            {
                Byte[] buffer = new Byte[4];
                buffer[0] = (Byte) previousColumn;
                buffer[1] = (Byte)previousRow;
                buffer[2] = (Byte)column;
                buffer[3] = (Byte)row;
                stream.Write(buffer, 0, 4);
            }
        }

        protected override void EndGame()
        {
            client.Close();
            client = new TcpClient();
        }
    }
}
