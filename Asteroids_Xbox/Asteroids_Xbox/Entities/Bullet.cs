using System;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Asteroids_Xbox.Manager;

namespace Asteroids_Xbox.Entities
{
    /// <summary>
    /// TODO: Implement
    /// </summary>
    class Bullet : Entity
    {
        private Vector2 speed;
        private Player player;
        private Color BackgroundColor = Color.GhostWhite;

        public Bullet(Player player, Vector2 speed, float rotation)
        {
            this.player = player;
            this.speed = speed;
            this.Rotation = rotation;
        }

        public override void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            throw new NotImplementedException();
        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            // Move bullet
            Forward(speed.X, speed.Y);

            // TODO: Check collisions
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(null, Position, BackgroundColor);
        }
    }
}
