using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Asteroids_Xbox.Manager;

namespace Asteroids_Xbox.Entities
{
    /// <summary>
    /// Display and process the titlescreen
    /// </summary>
    class Titlescreen : Entity
    {
        // Image used to display the static background
        //Texture2D mainBackground;       //To implement
        //Texture2D gameOverBackground;   //To implement
        //Texture2D mainMenuBackground;   //To implement

        public override void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            // Need to draw up some background images for these.
            //mainBackground = Content.Load<Texture2D>("mainbackground");
            //gameOverBackground = Content.Load<Texture2D>("endMenu");
            //mainMenuBackground = Content.Load<Texture2D>("mainMenu");

        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
