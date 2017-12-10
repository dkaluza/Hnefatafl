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
    public class MainMenu : Screen
    {
        public GameState NextState { get; private set; }

        private SpriteFont menuFont;

        Button start;
        Button credits;
        Button quit;
        Button online;

        private Vector2 middle = new Vector2(Consts.ScreenWidth / 2, Consts.ScreenHeight / 2);

        public MainMenu(SpriteBatch sp) : base(sp)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            start.Draw(gameTime, spriteBatch);
            credits.Draw(gameTime, spriteBatch);
            quit.Draw(gameTime, spriteBatch);
            online.Draw(gameTime, spriteBatch);
        }

        public override void LoadContent(ContentManager content)
        {
            menuFont = content.Load<SpriteFont>("MenuFont");

            start = new Button((int)(middle.X), (int)(middle.Y), "New Game", menuFont);

            online = new Button((int)(middle.X), (int)(middle.Y + start.Height), "Online Game", menuFont);

            credits = new Button((int)(middle.X), (int)(middle.Y + start.Height + online.Height), "Credits", menuFont);

            quit = new Button((int)(middle.X), (int)(middle.Y + start.Height + credits.Height + online.Height), "Quit", menuFont);

        }

        public override void UnloadContent(ContentManager content)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handle button clicks and state switching
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {

            SwitchState = false;
            start.Update(gameTime);
            quit.Update(gameTime);
            credits.Update(gameTime);
            online.Update(gameTime);
            if (start.Clicked)
            {
                SwitchState = true;
                NextState = GameState.Gameplay;
            }
            if(quit.Clicked)
            {
                SwitchState = true;
                NextState = GameState.Exit;
            }
            if(credits.Clicked)
            {
                SwitchState = true;
                NextState = GameState.Credits;
            }
            if (online.Clicked)
            {
                SwitchState = true;
                NextState = GameState.Online;
            }
        }

    }
}
