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
        private readonly Color BackgroundColor = Color.GhostWhite;
        private readonly EntityManager entityManager;

        private Vector2 speed;
        private Vector2 origin;
        public SoundEffect laserSound;

        public Bullet(EntityManager entityManager,
            Vector2 position, Vector2 speed, float rotation)
        {
            this.entityManager = entityManager;
            this.speed = speed;
            this.Rotation = rotation;

            this.origin = position;
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
            if (WrapScreenCount > 0)
            {
                if (CurrentSpeed.Y < 0.0f)
                {
                    if (Position.Y < origin.Y)
                    {
                        Kill();
                    }
                }
                else
                {
                    if (Position.Y > origin.Y)
                    {
                        Kill();
                    }
                }
            }

            Forward(speed.X, speed.Y);
            base.Update(inputManager, gameTime);
        }

        public override void Touch(AnimatedEntity other)
        {
            if (other is Asteroid)
            {
                var asteroid = other as Asteroid;
                asteroid.Health = 0;
                Kill();
            }

            base.Touch(other);
        }

        private void Kill()
        {
            entityManager.Remove(this);
        }
    }
}
