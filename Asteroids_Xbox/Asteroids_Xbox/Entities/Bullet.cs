using Asteroids_Xbox.Manager;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Entities
{
    /// <summary>
    /// Bullet for a the player
    /// </summary>
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

        /// <summary>
        /// Load the content for the bullet
        /// </summary>
        /// <param name="content"></param>
        public override void Load(ContentManager content)
        {
            laserSound = content.Load<SoundEffect>("sound/laserFire");
            Texture2D texture = content.Load<Texture2D>(TextureName);
            Animation.Initialize(texture, Vector2.Zero, 5, 5, 1, 30, BackgroundColor, 1f, true);

            MaxSpeed = 2.0f;
            MoveSpeed = 2.0f;
            RotationSpeed = 0.0f;
            WrapScreen = false;
        }

        /// <summary>
        /// Update the bullet on the game loop
        /// </summary>
        /// <param name="inputManager"></param>
        /// <param name="gameTime"></param>
        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            Forward(speed.X, speed.Y);
            base.Update(inputManager, gameTime);
        }

        /// <summary>
        /// Handle collisions with other entities
        /// </summary>
        /// <param name="other"></param>
        public override void Touch(AnimatedEntity other)
        {
            if (other is Asteroid)
            {
                var asteroid = other as Asteroid;
                asteroid.Health = 0;
                asteroid.Killer = this;
                Kill();
            }
            else if (other is EnemyShip)
            {
                var enemyShip = other as EnemyShip;
                enemyShip.Kill(this);
                Kill();
            }

            base.Touch(other);
        }

        /// <summary>
        /// Destroy the bullet
        /// </summary>
        protected void Kill()
        {
            entityManager.Remove(this);
        }
    }
}
