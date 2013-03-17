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
    class Bullet : AnimatedEntity
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

        public override void Load(ContentManager content)
        {
        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            base.Update(inputManager, gameTime);
            // Move bullet
            Forward(speed.X, speed.Y);
        }

    }
}
