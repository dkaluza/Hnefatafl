using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hnefatafln
{
    public class CreditsScreen : Screen
    {
        string credits = "Developed by Daniel Kaluza";
        string rules = "Rules by Adam Bartley (Norway),";
        string rules2 = "Tim Millar(UK) and Aage Nielsen(Denmark)";
        SpriteFont menuFont;
        Vector2 middle = new Vector2(Consts.ScreenWidth, Consts.ScreenHeight) / 2;

        Button back;

        public CreditsScreen(SpriteBatch sp) : base(sp)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            var position = middle - menuFont.MeasureString(credits) / 2;
            spriteBatch.DrawString(menuFont, credits, position, Color.Red);
            int offset = 10;
            position = middle - menuFont.MeasureString(rules) / 2 + new Vector2(0, menuFont.MeasureString(credits).Y + offset);
            spriteBatch.DrawString(menuFont, rules, position, Color.Red);
            position.Y += menuFont.MeasureString(rules).Y;
            position.X = middle.X - menuFont.MeasureString(rules2).X / 2;
            spriteBatch.DrawString(menuFont, rules2, position, Color.Red);

            back.Draw(gameTime, spriteBatch);

        }

        public override void LoadContent(ContentManager content)
        {
            menuFont = content.Load<SpriteFont>("MenuFont");

            back = new Button((int)middle.X, (int)(middle.Y + menuFont.MeasureString("Developed").Y * 3 + 20), "Back to main menu", menuFont);
        }

        public override void UnloadContent(ContentManager content)
        {
        }

        public override void Update(GameTime gameTime)
        {
            SwitchState = false;
            back.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                SwitchState = true;
            }

            if (back.Clicked)
            {
                SwitchState = true;
            }
        }
    }
}
