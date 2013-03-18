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

        private const string BulletTextureName = "Bullet";

        public Bullet(Player player, Vector2 speed, float rotation)
        {
            this.player = player;
            this.speed = speed;
            this.Rotation = rotation;
        }

        public override void Load(ContentManager content)
        {
            Texture2D bulletTexture = content.Load<Texture2D>(BulletTextureName);
            Animation.Initialize(bulletTexture, Vector2.Zero, 3, 3, 1, 30, BackgroundColor, 1f, true);

            MoveSpeed = this.speed.X;
            CurrentSpeed = speed;
            WrapScreen = true;

            Position = new Vector2
            (
                GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2,
                GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2
            );
        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            base.Update(inputManager, gameTime);
            // Move bullet
            Forward(speed.X, speed.Y);

            // TODO: Check collisions
        }

    }
}
