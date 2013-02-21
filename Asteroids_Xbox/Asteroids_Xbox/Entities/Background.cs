using System;
using Asteroids_Xbox.Manager;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Entities
{
    class Background : Entity
    {
        private Texture2D background;
        private GraphicsDevice graphicsDevice;

        private readonly string[] BackgroundContentNames = new string[]
        {
            "space1",
            "space2"
        };

        private readonly Color BackgroundColor = Color.Black;

        public override void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;

            var rand = new Random();
            var background = BackgroundContentNames[rand.Next(0, BackgroundContentNames.Length)];
            this.background = content.Load<Texture2D>(background);
        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            // Nothing to update, just draw
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, graphicsDevice.Viewport.Bounds, BackgroundColor);
        }
    }
}
