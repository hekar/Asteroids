using Asteroids_Xbox.Manager;
using Microsoft.Xna.Framework;

namespace Asteroids_Xbox.Entities
{
    class EnemyBullet : Bullet
    {
        public EnemyBullet(EntityManager entityManager,
            Vector2 position, Vector2 speed, float rotation) : base(entityManager, position, speed, rotation)
        {
        }
    }
}
