using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Hnefatafln.Entities;
using Microsoft.Xna.Framework.Input;

namespace Hnefatafln
{
    public class GameScreen : Screen
    {
        /// <summary>
        /// On board Pieces [column, row]
        /// </summary>
        protected Piece[,] Board = new Piece[Consts.NumberOfRowsColumns, Consts.NumberOfRowsColumns];

        King king;
        bool blackWin;
        bool whiteWin;

        Texture2D blackPawn;
        Texture2D whitePawn;
        Texture2D pieceBackground;
        Texture2D kingTexture;
        SpriteFont menuFont;

        protected Piece clickedPiece;
        protected bool blackTurn;

        ButtonState lastState = ButtonState.Released;

        public GameScreen(SpriteBatch sp) : base(sp)
        {
        }

        public virtual void StartGame()
        {
            for (int i = 0; i < Consts.NumberOfRowsColumns; i++)
            {
                for (int j = 0; j < Consts.NumberOfRowsColumns; j++)
                {
                    Board[i, j] = null;
                }
            }

            // Add white king
            king = new King(Consts.NumberOfRowsColumns / 2, Consts.NumberOfRowsColumns / 2, kingTexture);
            Board[Consts.NumberOfRowsColumns / 2, Consts.NumberOfRowsColumns / 2] = king;
            blackWin = false;
            whiteWin = false;
            clickedPiece = null;
            blackTurn = false;
            lastState = ButtonState.Released;

            // Add black Pawns
            for(int i = 3; i < Consts.NumberOfRowsColumns - 3; i++)
            {
                Board[i, 0] = new Pawn(i, 0, blackPawn, true);
                Board[i, Consts.NumberOfRowsColumns - 1] = new Pawn(i, Consts.NumberOfRowsColumns - 1, blackPawn, true);
                Board[0, i] = new Pawn(0, i, blackPawn, true);
                Board[Consts.NumberOfRowsColumns - 1, i] = new Pawn(Consts.NumberOfRowsColumns - 1, i, blackPawn, true);
            }
            Board[1, Consts.NumberOfRowsColumns/2] = new Pawn(1, Consts.NumberOfRowsColumns / 2, blackPawn, true);
            Board[Consts.NumberOfRowsColumns / 2, 1] = new Pawn(Consts.NumberOfRowsColumns / 2, 1, blackPawn, true);
            Board[Consts.NumberOfRowsColumns - 2, Consts.NumberOfRowsColumns / 2] = 
                new Pawn(Consts.NumberOfRowsColumns - 2, Consts.NumberOfRowsColumns / 2, blackPawn, true);
            Board[Consts.NumberOfRowsColumns / 2, Consts.NumberOfRowsColumns - 2] = 
                new Pawn(Consts.NumberOfRowsColumns / 2, Consts.NumberOfRowsColumns - 2, blackPawn, true);

            for(int i = 3; i < Consts.NumberOfRowsColumns - 3; i++)
            {
                for(int j = 3; j < Consts.NumberOfRowsColumns - 3; j++)
                {
                    if(Math.Abs(Consts.NumberOfRowsColumns / 2 - i) + Math.Abs(Consts.NumberOfRowsColumns / 2 - j) < 3 && Board[i,j] == null)
                        Board[i,j] = new Pawn(i, j, whitePawn, false);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach(Piece piece in Board)
            {
                if (piece != null)
                    piece.Draw(gameTime, spriteBatch, pieceBackground);
            }

            if (whiteWin)
            {
                string text = "White win, press Esc to exit";
                Vector2 pos = new Vector2(Consts.ScreenWidth / 2, Consts.ScreenHeight / 2) - menuFont.MeasureString(text) / 2;
                spriteBatch.DrawString(menuFont, text, pos, Color.Red);
            }
            if (blackWin)
            {
                string text = "Black win, press Esc to exit";
                Vector2 pos = new Vector2(Consts.ScreenWidth / 2, Consts.ScreenHeight / 2) - menuFont.MeasureString(text) / 2;
                spriteBatch.DrawString(menuFont, text, pos, Color.Red);
            }

            string turn;
            if (blackTurn)
                turn = "Black turn";
            else
                turn = "White turn";

            spriteBatch.DrawString(menuFont, turn, new Vector2(10, 10), Color.Red);

        }

        public override void LoadContent(ContentManager content)
        {
            blackPawn = content.Load<Texture2D>("BlackPawn");
            whitePawn = content.Load<Texture2D>("WhitePawn");
            pieceBackground = content.Load<Texture2D>("PieceBackground");
            Color[] whiteBackground = Enumerable.Range(0, pieceBackground.Width*pieceBackground.Height).Select(i => Color.White).ToArray();
            pieceBackground.SetData(whiteBackground);
            kingTexture = content.Load<Texture2D>("WhiteKing");
            menuFont = content.Load<SpriteFont>("MenuFont");
        }

        public override void UnloadContent(ContentManager content)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            SwitchState = false;
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                EndGame();
                SwitchState = true;
            }
            if (CheckSomebodyWins())
                return;
            RemoveDead();

            DoOpponentsTurn();

            var mouse = Mouse.GetState();
            if(mouse.X < Consts.ScreenWidth && mouse.X >= 0 && mouse.Y < Consts.ScreenHeight && mouse.Y >= 0)
            {
                if (mouse.LeftButton == ButtonState.Pressed && lastState == ButtonState.Released)
                {
                    // try to get a piece to move
                    if (clickedPiece == null)
                    {
                        int row = Piece.GetRow(mouse.Y);
                        int column = Piece.GetColumn(mouse.X);
                        clickedPiece = Board[column, row];
                        if (clickedPiece != null && clickedPiece.Black == blackTurn)
                        {
                            clickedPiece.Clicked = true;
                        }
                        else
                        {
                            clickedPiece = null;
                        }
                    }
                    else // try to move clicked piece
                    {
                        int row = Piece.GetRow(mouse.Y);
                        int column = Piece.GetColumn(mouse.X);
                        MoveClickedPiece(column, row);
                    }
                }

                if (mouse.RightButton == ButtonState.Pressed && clickedPiece != null)
                {
                    clickedPiece.Clicked = false;
                    clickedPiece = null;
                }

                lastState = mouse.LeftButton;
            }
        }

        protected virtual void EndGame()
        {
        }

        /// <summary>
        /// Will be filled in games through the network
        /// </summary>
        protected virtual void DoOpponentsTurn()
        {
        }

        protected void MoveClickedPiece(int column, int row)
        {
            if (Board[column, row] == null)
            {
                if (CheckPath(clickedPiece.Column, clickedPiece.Row, column, row))
                {
                    int previousColumn = clickedPiece.Column;
                    int previousRow = clickedPiece.Row;
                    clickedPiece.Move(column, row);
                    if (clickedPiece.Moved)
                    {
                        clickedPiece.Moved = false;
                        Board[previousColumn, previousRow] = null;
                        Board[column, row] = clickedPiece;
                        CheckSomeoneKilled(clickedPiece);
                        SendToOpponent(previousColumn, previousRow, column, row);
                        blackTurn = !blackTurn;
                        clickedPiece = null;
                    }
                }
            }
        }

        protected virtual void SendToOpponent(int previousColumn, int previousRow, int column, int row)
        {
        }

        private bool CheckSomebodyWins()
        {
            if (!king.Alive)
            {
                blackWin = true;
                return true;
            }
            if(IsRestricted(king.Column, king.Row) && king.Column != Consts.NumberOfRowsColumns/2)
            {
                whiteWin = true;
                return true;
            }

            return false;
        }

        void RemoveDead()
        {
            for (int i = 0; i < Consts.NumberOfRowsColumns; i++)
            {
                for (int j = 0; j < Consts.NumberOfRowsColumns; j++)
                {
                    if (Board[i, j] != null && !Board[i, j].Alive)
                    {
                        Board[i, j] = null;
                    }
                }
            }
        }

        public static bool IsRestricted(int column, int row)
        {
            if ((column == 0 || column == Consts.NumberOfRowsColumns - 1) && (row == 0 || row == Consts.NumberOfRowsColumns - 1))
            {
                return true;
            }
            if (column == Consts.NumberOfRowsColumns / 2 && row == Consts.NumberOfRowsColumns / 2)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns if position is inside the board
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static bool IsInside(int column, int row)
        {
            return (column < Consts.NumberOfRowsColumns && column >= 0 && row < Consts.NumberOfRowsColumns && row >= 0);
        }

        void CheckSomeoneKilled(Piece piece)
        {
            CheckNormalCapture(piece, piece.Column + 1, piece.Row, piece.Column + 2, piece.Row, piece.Column + 1, piece.Row + 1, piece.Column + 1, piece.Row - 1);
            CheckNormalCapture(piece, piece.Column - 1, piece.Row, piece.Column - 2, piece.Row, piece.Column - 1, piece.Row + 1, piece.Column - 1, piece.Row - 1);
            CheckNormalCapture(piece, piece.Column, piece.Row + 1, piece.Column, piece.Row + 2, piece.Column + 1, piece.Row + 1, piece.Column - 1, piece.Row + 1);
            CheckNormalCapture(piece, piece.Column, piece.Row - 1, piece.Column, piece.Row - 2, piece.Column + 1, piece.Row - 1, piece.Column - 1, piece.Row - 1);

        }

        /// <summary>
        /// Checks if an unit is captured and if yes kills the unit
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="preyColumn"></param>
        /// <param name="preyRow"></param>
        /// <param name="otherSideColumn"></param>
        /// <param name="otherSideRow"></param>
        /// <param name="col1">Column of third position next to prey</param>
        /// <param name="row1">Row of third position next to prey</param>
        /// <param name="col2">Column of 4th position next to prey </param>
        /// <param name="row2">Row of 4th position next to prey</param>
        void CheckNormalCapture(Piece piece, int preyColumn, int preyRow, int otherSideColumn, int otherSideRow, int col1, int row1, int col2, int row2)
        {
            if(IsInside(otherSideColumn, otherSideRow))
            {
                Piece posibleAttacker = Board[otherSideColumn, otherSideRow];
                if(posibleAttacker == null)
                {
                    if(IsRestricted(otherSideColumn, otherSideRow))
                    {
                        posibleAttacker = piece;
                    }
                    else
                    {
                        return;
                    }
                }
                Piece prey = Board[preyColumn, preyRow];
                if (prey != null)
                {
                    Piece piece1;
                    Piece piece2;
                    piece1 = IsInside(col1, row1) ? Board[col1, row1] : null;
                    piece2 = IsInside(col2, row2) ? Board[col2, row2] : null;
                    piece1 = piece1 == null && IsRestricted(col1, row1) ? piece : piece1;
                    piece2 = piece2 == null && IsRestricted(col2, row2) ? piece : piece2;
                    prey.BeAttacked(piece, posibleAttacker, piece1, piece2);
                }
            }
        }

        /// <summary>
        /// Returns if there is any other piece on the moving path
        /// </summary>
        /// <param name="fromColumn">Moving piece actual column</param>
        /// <param name="fromRow">Moving piece actual row</param>
        /// <param name="toColumn">Moving piece destination column</param>
        /// <param name="toRow">Moving piece destination row</param>
        /// <returns>If there isnt any piece on the path returns true</returns>
        private bool CheckPath(int fromColumn, int fromRow, int toColumn, int toRow)
        {
            int modifier;
            if (fromColumn == toColumn)
            {
                if (fromRow > toRow)
                    modifier = -1;
                else
                    modifier = 1;
                for (int i = fromRow + modifier; i != toRow; i += modifier)
                {
                    if (Board[fromColumn, i] != null)
                        return false;
                }
                return true;
            }
            else if (fromRow == toRow)
            {
                if (fromColumn > toColumn)
                    modifier = -1;
                else
                    modifier = 1;
                for (int i = fromColumn + modifier; i != toColumn; i += modifier)
                {
                    if (Board[i, fromRow] != null)
                        return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
