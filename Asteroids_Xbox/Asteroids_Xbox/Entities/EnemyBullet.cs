using Asteroids_Xbox.Manager;
using Microsoft.Xna.Framework;
using Asteroids_Xbox.Types;

namespace Asteroids_Xbox.Entities
{
    /// <summary>
    /// Extension of a bullet that may also kill the player.
    /// 
    /// Fired from the enemy ship
    /// </summary>
    class EnemyBullet : Bullet
    {
        /// <summary>
        /// The ship that fired this bullet
        /// </summary>
        private EnemyShip ship;

        public EnemyBullet(EntityManager entityManager, EnemyShip ship,
            Vector2 position, Vector2 speed, float rotation) : 
            base(entityManager, position, speed, rotation)
        {
            this.ship = ship;
        }

        /// <summary>
        /// Change the touch event details slightly to also kill the player
        /// </summary>
        /// <param name="other"></param>
        public override void Touch(Types.AnimatedEntity other)
        {
            if (other is Player)
            {
                var player = other as Player;
                if (!player.UnderProtection)
                {
                    player.Kill();
                }

                Kill();
            }
            else if (other is Asteroid && ship.Size == Sizes.Large)
            {
                var asteroid = other as Asteroid;
                asteroid.Health = 0;
                asteroid.Killer = this;
                Kill();
            }
        }
    }
}
