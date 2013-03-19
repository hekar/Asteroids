using Asteroids_Xbox.Manager;
using Microsoft.Xna.Framework;

namespace Asteroids_Xbox.Entities
{
    /// <summary>
    /// Extension of a bullet that may also kill the player.
    /// 
    /// Fired from the enemy ship
    /// </summary>
    class EnemyBullet : Bullet
    {
        public EnemyBullet(EntityManager entityManager,
            Vector2 position, Vector2 speed, float rotation) : base(entityManager, position, speed, rotation)
        {
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
            }

            base.Touch(other);
        }
    }
}
