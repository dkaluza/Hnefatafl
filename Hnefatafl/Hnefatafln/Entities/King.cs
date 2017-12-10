using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Hnefatafln.Entities
{
    public class King : Piece
    {
        public King(int column, int row, Texture2D sprite) : base(column, row, sprite, false)
        {
        }

        public override void BeAttacked(Piece piece1, Piece piece2, Piece piece3, Piece piece4)
        {
            if (piece1 == null || piece2 == null || piece3 == null || piece4 == null)
                return;
            if(piece1.Black == piece2.Black && piece2.Black == piece3.Black && piece3.Black == piece4.Black && Black != piece4.Black)
            {
                Alive = false;
            }
        }
    }
}
