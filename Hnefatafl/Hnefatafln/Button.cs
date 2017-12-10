using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hnefatafln
{
    public class Button
    {
        public bool Clicked { get; private set; }

        ButtonState oldState = ButtonState.Released;

        string text;
        SpriteFont font;
        Rectangle area;
        public int Height { get; }

        public Button(int x, int y, string text, SpriteFont font)
        {
            this.font = font;
            area = new Rectangle(x, y, (int)font.MeasureString(text).X, (int)font.MeasureString(text).Y);
            this.text = text;
            Height = (int)font.MeasureString(text).Y;
            area.X -= (int)font.MeasureString(text).X / 2;
            area.Y -= (int)font.MeasureString(text).Y / 2;

        }

        public void Update(GameTime gameTime)
        {
            Clicked = false;

            var mouse = Mouse.GetState();
            if(area.Contains(mouse.Position) && oldState == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed)
            {
                Clicked = true;
            }
            oldState = mouse.LeftButton;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(Clicked)
                spriteBatch.DrawString(font, text, area.Location.ToVector2(), Color.Purple);
            else
                spriteBatch.DrawString(font, text, area.Location.ToVector2(), Color.Red);
        }
    }
}
