using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework.Content;
using Asteroids_Xbox.Manager;
using Microsoft.Xna.Framework;

namespace Asteroids_Xbox.Entities
{
    class EnemyShip : AnimatedEntity
    {
        private readonly EntityManager entityManager;

        private const double bulletFireTime = 0.5;
        private double previousSeconds;
        private double lastRespawnTime;
        private Vector2 speed;

        public EnemyShip(EntityManager entityManager, Vector2 position, Vector2 speed)
        {
            this.entityManager = entityManager;
            Position = position;
            this.speed = speed;
        }

        public override void Load(ContentManager content)
        {
            MaxSpeed = 5.0f;
            MoveSpeed = 5.0f;
        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            Rotate(2.0f);
            Move(speed.X, speed.Y);
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
