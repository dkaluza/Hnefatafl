using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Hnefatafln.Entities
{
    public class Pawn : Piece
    {
        public Pawn(int column, int row, Texture2D sprite, bool black) : base(column, row, sprite, black)
        {
        }

        public override void BeAttacked(Piece piece1, Piece piece2, Piece piece3, Piece piece4)
        {
            if (piece1 == null || piece2 == null)
                return;
            if(piece1.Black == piece2.Black && Black != piece1.Black)
            {
                Alive = false;
            }
        }

        protected override bool IsMoveOk(int column, int row)
        {
            // Restricted corners
            if (GameScreen.IsRestricted(column, row))
            {
                return false;
            }
            return base.IsMoveOk(column, row);
        }
    }
}
