using System;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Asteroids_Xbox.Manager;
using Microsoft.Xna.Framework.Audio;

namespace Asteroids_Xbox.Entities
{
    class Bullet : AnimatedEntity
    {
        private const string TextureName = "bullet";

        private Vector2 speed;
        private readonly Player player;
        private readonly Color BackgroundColor = Color.GhostWhite;
        private readonly EntityManager entityManager;
        public SoundEffect laserSound;

        public Bullet(EntityManager entityManager, Player player, 
            Vector2 position, Vector2 speed, float rotation)
        {
            this.entityManager = entityManager;
            this.player = player;
            this.speed = speed;
            this.Rotation = rotation;

            WrapScreen = true;

            Position = position;
            MaxSpeed = 5.0f;
            MoveSpeed = 5.0f;
            RotationSpeed = 5.0f;
        }

        public override void Load(ContentManager content)
        {
            laserSound = content.Load<SoundEffect>("sound/laserFire");
            Texture2D texture = content.Load<Texture2D>(TextureName);
            Animation.Initialize(texture, Vector2.Zero, 5, 5, 1, 30, BackgroundColor, 1f, true);
        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            // Move bullet
            Forward(speed.X, speed.Y);
            base.Update(inputManager, gameTime);
            // TODO: Remove bullet when it flies off the screen
            //GraphicsDevice.Viewport.y
        }

    }
}
