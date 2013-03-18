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

            Position = position;
            MaxSpeed = 2.0f;
            MoveSpeed = 2.0f;
            RotationSpeed = 0.0f;
            WrapScreen = true;
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

            if (Offscreen)
            {
                entityManager.Remove(this);
            }
        }

        public override void Touch(AnimatedEntity other)
        {
            if (other is Asteroid)
            {
                var asteroid = other as Asteroid;
                asteroid.Health = 0;
                entityManager.Remove(this);
            }

            base.Touch(other);
        }
    }
}
