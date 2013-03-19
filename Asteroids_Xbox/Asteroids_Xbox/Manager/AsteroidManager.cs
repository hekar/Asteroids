using System;
using System.Collections.Generic;
using Asteroids_Xbox.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Asteroids_Xbox.Types;

namespace Asteroids_Xbox.Manager
{
    /// <summary>
    /// 
    /// </summary>
    class AsteroidManager
    {
        private readonly EntityManager entityManager;
        private readonly Player player;
        private readonly Random random;

        private const int DefaultAsteroidCount = 3;

        /// <summary>
        /// List of all asteroids
        /// </summary>
        private readonly List<Asteroid> asteroids = new List<Asteroid>();

        /// <summary>
        /// Newly created large asteroids. Created by the game timer (not by breaking larger asteroids into smaller ones)
        /// </summary>
        private readonly List<Asteroid> freshAsteroids = new List<Asteroid>();

        /// <summary>
        /// The rate at which asteroids appear
        /// </summary>
        private readonly TimeSpan asteroidSpawnTime = TimeSpan.FromSeconds(1.5f);

        /// <summary>
        /// The last time an asteroid spawned
        /// </summary>
        private TimeSpan lastAsteroidSpawnTime;

        /// <summary>
        /// Number of large asteroids that can be in the game at a given time
        /// </summary>
        private int asteroidSpawnLimit = DefaultAsteroidCount;

        /// <summary>
        /// Number of ships on the map
        /// </summary>
        private int shipCount = 0;

        /// <summary>
        /// Extra lives
        /// </summary>
        private int maxLives = 0;

        public AsteroidManager(EntityManager entityManager, Player player)
        {
            this.entityManager = entityManager;
            this.player = player;

            random = new Random();

            // Set the time keepers to zero
            lastAsteroidSpawnTime = TimeSpan.Zero;
        }

        /// <summary>
        /// Create an asteroid
        /// </summary>
        /// <param name="content"></param>
        /// <param name="graphicsDevice"></param>
        /// <returns></returns>
        private Asteroid CreateAsteroid(ContentManager content, GraphicsDevice graphicsDevice)
        {
            // Create an asteroid
            var asteroid = new Asteroid(this, player);
            asteroid.Initialize(content, graphicsDevice);

            float speed = 1.0f;
            float x = 0.0f;
            float y = 0.0f;
            Vector2 direction = Vector2.Zero;
            var corner = random.Next(0, 3);
            switch (corner)
            {
                case 0:
                    x = graphicsDevice.Viewport.X + asteroid.Width / 2;
                    y = graphicsDevice.Viewport.Y + asteroid.Height / 2;
                    asteroid.CurrentSpeed = new Vector2(1.0f * speed, 1.0f * speed);
                    break;
                case 1:
                    x = graphicsDevice.Viewport.Width - asteroid.Width / 2;
                    y = graphicsDevice.Viewport.Y + asteroid.Height / 2;
                    asteroid.CurrentSpeed = new Vector2(-1.0f * speed, 1.0f * speed);
                    break;
                case 2:
                    x = graphicsDevice.Viewport.X + asteroid.Width / 2;
                    y = graphicsDevice.Viewport.Height - asteroid.Height / 2;
                    asteroid.CurrentSpeed = new Vector2(1.0f * speed, -1.0f * speed);
                    break;
                case 3:
                    x = graphicsDevice.Viewport.Width - asteroid.Width / 2;
                    y = graphicsDevice.Viewport.Height - asteroid.Height / 2;
                    asteroid.CurrentSpeed = new Vector2(-1.0f * speed, -1.0f * speed);
                    break;
                default:
                    break;
            }

            asteroid.Position = new Vector2(x, y);
            asteroids.Add(asteroid);
            return asteroid;
        }

        /// <summary>
        /// Remove an asteroid from the asteroid manager
        /// </summary>
        /// <param name="asteroid"></param>
        public void Remove(Asteroid asteroid)
        {
            asteroids.Remove(asteroid);
            freshAsteroids.Remove(asteroid);
            entityManager.Remove(asteroid);
        }

        /// <summary>
        /// Clear the asteroid manager
        /// </summary>
        public void Clear()
        {
            asteroids.Clear();
            freshAsteroids.Clear();
            entityManager.Clear();
            shipCount = 0;
            maxLives = 0;
        }

        /// <summary>
        /// Update handled on game loop
        /// </summary>
        /// <param name="content"></param>
        /// <param name="graphicsDevice"></param>
        /// <param name="player"></param>
        /// <param name="gameTime"></param>
        public void Update(ContentManager content, GraphicsDevice graphicsDevice, Player player, GameTime gameTime)
        {
            var spawnTimeReached = (gameTime.TotalGameTime - lastAsteroidSpawnTime) > asteroidSpawnTime;
            var spawnLimitReached = freshAsteroids.Count >= asteroidSpawnLimit;

            if (shipCount < 1)
            {
                CreateEnemyShip(content, graphicsDevice);
            }

            var spawnNewAsteroid = spawnTimeReached && !spawnLimitReached;
            if (spawnNewAsteroid)
            {
                lastAsteroidSpawnTime = gameTime.TotalGameTime;

                // Add an asteroid
                var asteroid = CreateAsteroid(content, graphicsDevice);
                freshAsteroids.Add(asteroid);
                entityManager.Add(asteroid);
            }

            RemoveDeadAsteroids(content, graphicsDevice, player);

            asteroidSpawnLimit = (int)Math.Floor((double)player.Score / 1000) + DefaultAsteroidCount;
            var previousLives = maxLives;
            maxLives = (int)Math.Floor((double)player.Score / 10000) + Player.DefaultLives;

            if (previousLives != maxLives)
            {
                player.Lives += 1;
            }
        }

        private void CreateEnemyShip(ContentManager content, GraphicsDevice graphicsDevice)
        {
            var pos = new Vector2(graphicsDevice.Viewport.X, 
                graphicsDevice.Viewport.Y + random.Next(50, graphicsDevice.Viewport.Height));
            var enemy = CreateEnemy(pos, content, graphicsDevice);
            entityManager.Add(enemy);
            shipCount++;
        }

        /// <summary>
        /// Remove dead asteroids from the map
        /// </summary>
        /// <param name="content"></param>
        /// <param name="graphicsDevice"></param>
        /// <param name="player"></param>
        private void RemoveDeadAsteroids(ContentManager content, GraphicsDevice graphicsDevice, Player player)
        {
            // Remove dead asteroids
            for (int i = asteroids.Count - 1; i >= 0; i--)
            {
                var asteroid = asteroids[i];
                if (!(asteroid.Killer is EnemyBullet) && asteroid.Dead)
                {
                    //Add to the player's score
                    player.Score += asteroid.ScoreWorth;
                }

                if (asteroid.Dead)
                {
                    var nextSize = (asteroid.Size == Sizes.Large) ? Sizes.Medium : Sizes.Small;
                    var explosion = CreateExplosion(nextSize, asteroid.Position, content, graphicsDevice);
                    entityManager.Add(explosion);
                    explosion.PlayExplosionSound();

                    var splitAsteroids = new List<Asteroid>();
                    if (asteroid.Size != Sizes.Small)
                    {

                        var a1 = new Asteroid(this, player, nextSize);
                        var a2 = new Asteroid(this, player, nextSize);
                        a1.Position = new Vector2(asteroid.Position.X, asteroid.Position.Y + 15);
                        a1.CurrentSpeed = asteroid.CurrentSpeed;
                        a2.Position = asteroid.Position;
                        a2.CurrentSpeed = new Vector2(-asteroid.CurrentSpeed.X, -asteroid.CurrentSpeed.Y);
                        splitAsteroids.AddRange(new Asteroid[] { a1, a2 });
                    }

                    Remove(asteroid);

                    foreach (var newAsteroid in splitAsteroids)
                    {
                        entityManager.Add(newAsteroid);
                        asteroids.Add(newAsteroid);
                    }
                }
            }
        }

        /// <summary>
        /// Create asteroid explosion after an asteroid has been killed
        /// </summary>
        /// <param name="size"></param>
        /// <param name="position"></param>
        /// <param name="content"></param>
        /// <param name="graphicsDevice"></param>
        /// <returns></returns>
        private Explosion CreateExplosion(Sizes size, Vector2 position, ContentManager content, GraphicsDevice graphicsDevice)
        {
            Func<Sizes, String> fun = (explosionSize) =>
            {
                switch (explosionSize)
                {
                    case Sizes.Small:
                        return "asteroidSmall";
                    case Sizes.Medium:
                        return "asteroidMedium";
                    case Sizes.Large:
                        return "asteroidLarge";
                    default:
                        return "asteroidLarge";
                }
            };

            var texture = fun(size) + "_Animated_Trans1";
            var explosion = new Explosion(entityManager, texture);
            explosion.Initialize(content, graphicsDevice);
            explosion.Position = new Vector2(position.X, position.Y);

            return explosion;
        }

        /// <summary>
        /// This does not belong here...
        /// </summary>
        /// <param name="position"></param>
        /// <param name="content"></param>
        /// <param name="graphicsDevice"></param>
        /// <returns></returns>
        public EnemyShip CreateEnemy(Vector2 position, ContentManager content, GraphicsDevice graphicsDevice)
        {
            Sizes size = Sizes.Large;
            if (player.Score > 10000)
            {
                size = Sizes.Small;
            }

            var speed = new Vector2(1.0f, 0.0f);
            var enemy = new EnemyShip(entityManager, position, speed, size, player);
            enemy.Initialize(content, graphicsDevice);
            return enemy;
        }
    }
}
