using Asteroids_Xbox.Manager;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Entities
{
    class Bullet : AnimatedEntity
    {
        private const string TextureName = "bullet";

        private Vector2 speed;
        private readonly Color BackgroundColor = Color.GhostWhite;
        private readonly EntityManager entityManager;
        public SoundEffect laserSound;

        public Bullet(EntityManager entityManager,
            Vector2 position, Vector2 speed, float rotation)
        {
            this.entityManager = entityManager;
            this.speed = speed;
            this.Rotation = rotation;

            Position = position;
        }

        public override void Load(ContentManager content)
        {
            laserSound = content.Load<SoundEffect>("sound/laserFire");
            Texture2D texture = content.Load<Texture2D>(TextureName);
            Animation.Initialize(texture, Vector2.Zero, 5, 5, 1, 30, BackgroundColor, 1f, true);

            MaxSpeed = 2.0f;
            MoveSpeed = 2.0f;
            RotationSpeed = 0.0f;
            WrapScreen = true;
        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            Forward(speed.X, speed.Y);
            base.Update(inputManager, gameTime);
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
