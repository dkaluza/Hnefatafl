using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hnefatafln
{
    /// <summary>
    /// Screen base class.
    /// </summary>
    public abstract class Screen
    {
        public bool SwitchState { get; protected set; }
        protected SpriteBatch spriteBatch;

        public Screen(SpriteBatch sp)
        {
            spriteBatch = sp;
        }

        public abstract void LoadContent(ContentManager content);

        public abstract void UnloadContent(ContentManager content);

        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draws this screen and all its entities
        /// </summary>
        /// <param name="gameTime">Elapsed time for updates and animations in future</param>
        /// <param name="spriteBatch">Drawing batch</param>
        public abstract void Draw(GameTime gameTime);

    }
}
