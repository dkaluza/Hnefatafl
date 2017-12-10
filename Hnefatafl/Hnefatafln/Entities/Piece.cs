using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hnefatafln.Entities
{
    public abstract class Piece
    {
        public bool Alive { get; protected set; }
        public int Row { get; private set; }
        public int Column { get; private set; }
        Rectangle drawRect;
        Texture2D sprite;
        public bool Black { get; }
        public bool Moved { get; set; }
        public bool Clicked { get; set; }

        public Piece(int column, int row, Texture2D sprite, bool black)
        {
            Row = row;
            Column = column;
            drawRect = new Rectangle(GetX(column), GetY(row), Consts.ColumnSize, Consts.RowSize);
            this.sprite = sprite;
            Alive = true;
            Black = black;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D background)
        {
            if (Clicked)
                spriteBatch.Draw(background, drawRect, Color.Blue);
            if(Black)
                spriteBatch.Draw(sprite, drawRect, Color.Black);
            else
                spriteBatch.Draw(sprite, drawRect, Color.White);

        }

        public void Move(int column, int row)
        {
            
            if(IsMoveOk(column, row))
            {
                Row = row;
                Column = column;
                drawRect.X = GetX(column);
                drawRect.Y = GetY(row);
                Moved = true;
                Clicked = false;
            }
        }

        public abstract void BeAttacked(Piece piece1, Piece piece2, Piece piece3, Piece piece4);

        protected virtual bool IsMoveOk(int column, int row)
        {
            return Row == row || Column == column;
        }
        public static int GetColumn(int x)
        {
            return x / Consts.ColumnSize;
        }

        public static int GetRow(int y)
        {
            return y / Consts.RowSize;
        }

        public static int GetX(int column)
        {
            return column * Consts.ColumnSize;
        }

        public static int GetY(int row)
        {
            return row * Consts.RowSize;
        }
    }
}
