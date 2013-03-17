using System;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Asteroids_Xbox.Manager;

namespace Asteroids_Xbox.Entities
{
    class Bullet : AnimatedEntity
    {
        private const string TextureName = "bullet";

        private Vector2 speed;
        private Player player;
        private Color BackgroundColor = Color.GhostWhite;

        public Bullet(Player player, Vector2 position, Vector2 speed, float rotation)
        {
            this.player = player;
            this.speed = speed;
            this.Rotation = rotation;

            Position = position;
            MaxSpeed = 5.0f;
            MoveSpeed = 5.0f;
            RotationSpeed = 5.0f;
        }

        public override void Load(ContentManager content)
        {
            Texture2D texture = content.Load<Texture2D>(TextureName);
            Animation.Initialize(texture, Vector2.Zero, 5, 5, 1, 30, BackgroundColor, 1f, true);
        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            // Move bullet
            Forward(speed.X, speed.Y);

            base.Update(inputManager, gameTime);
        }

    }
}
