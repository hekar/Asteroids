using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework.Content;
using Asteroids_Xbox.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Entities
{
    class EnemyShip : AnimatedEntity
    {
        private readonly EntityManager entityManager;
        private Texture2D enemyTexture;
        private const double bulletFireTime = 0.5;
        private double previousSeconds;
        private Vector2 speed;
        private readonly Color BackgroundColor = Color.White;
        public Sizes Size { get; set; }

        public EnemyShip(EntityManager entityManager, Vector2 position, Vector2 speed, Sizes newSize)
        {
            this.entityManager = entityManager;
            Position = position;
            this.Size = newSize;
            this.speed = speed;
        }

        public override void Load(ContentManager content)
        {
            switch (this.Size)
            {
                case Sizes.Small:
                    enemyTexture = content.Load<Texture2D>("Enemy_Small_Animated");
                    Animation.Initialize(enemyTexture, Vector2.Zero, 50, 20, 8, 45, BackgroundColor, 1f, true);
                    break;
                case Sizes.Large:
                    enemyTexture = content.Load<Texture2D>("Enemy_Large_Animated");
                    Animation.Initialize(enemyTexture, Vector2.Zero, 75, 30, 8, 45, BackgroundColor, 1f, true);
                    break;
                default:
                    enemyTexture = content.Load<Texture2D>("Enemy_Large_Animated");
                    Animation.Initialize(enemyTexture, Vector2.Zero, 75, 30, 8, 45, BackgroundColor, 1f, true);
                    break;
            }
            WrapScreen = true;
            MaxSpeed = 5.0f;
            MoveSpeed = 5.0f;
        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            Rotate(2.0f);
            Move(speed.X, speed.Y);
            base.Update(inputManager, gameTime);
        }

        private Bullet FireBullet(Vector2 speed)
        {
            // If someone is playing around midnight, they get a lucky shot that
            // has no delay
            var totalSeconds = DateTime.Now.TimeOfDay.TotalSeconds;
            var timeSinceLast = totalSeconds - previousSeconds;
            if (timeSinceLast > bulletFireTime)
            {
                Vector2 bulletPosition = new Vector2(Position.X + (Width / 2), Position.Y + (Height / 2));
                var bullet = new EnemyBullet(entityManager, bulletPosition, speed, Rotation);
                entityManager.Add(bullet);
                bullet.laserSound.Play();
                previousSeconds = totalSeconds;
                return bullet;
            }
            else
            {
                return null;
            }
        }
    }
}
