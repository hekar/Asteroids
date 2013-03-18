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
    /// TODO: Implement levels and difficulty increases, etc
    /// </summary>
    class AsteroidManager
    {
        private readonly EntityManager entityManager;
        private readonly Random random = new Random();

        /// <summary>
        /// List of all asteroids
        /// </summary>
        private readonly List<Asteroid> asteroids = new List<Asteroid>();

        /// <summary>
        /// Newly created large asteroids. Created by the game timer (not by breaking larger asteroids into smaller ones)
        /// </summary>
        private readonly List<Asteroid> freshAsteroids = new List<Asteroid>();

        // The rate at which the asteroids appear
        private TimeSpan asteroidSpawnTime;
        private TimeSpan previousSpawnTime;

        /// <summary>
        /// Number of large asteroids that can be in the game at a given time
        /// </summary>
        private int asteroidSpawnLimit = 15;

        public AsteroidManager(EntityManager entityManager)
        {
            this.entityManager = entityManager;

            // Set the time keepers to zero
            previousSpawnTime = TimeSpan.Zero;
            // Used to determine how fast asteroids spawns
            asteroidSpawnTime = TimeSpan.FromSeconds(1.5f);
        }

        public Asteroid CreateAsteroid(ContentManager content, GraphicsDevice graphicsDevice, Player player)
        {
            // Create an asteroid
            Asteroid asteroid = new Asteroid(this, player);
            asteroid.Initialize(content, graphicsDevice);

            // TODO: Randomly generate the position of the asteroid
            float x = graphicsDevice.Viewport.X + asteroid.Width / 2;
            float y = random.Next(asteroid.Height / 2, graphicsDevice.Viewport.Y + asteroid.Height);

            asteroid.Position = new Vector2(x, y);
            asteroids.Add(asteroid);
            return asteroid;
        }

        public void Remove(Asteroid asteroid)
        {
            asteroids.Remove(asteroid);
            freshAsteroids.Remove(asteroid);
            entityManager.Remove(asteroid);
        }

        public void Update(ContentManager content, GraphicsDevice graphicsDevice, Player player, GameTime gameTime)
        {
            var spawnTimeReached = (gameTime.TotalGameTime - previousSpawnTime) > asteroidSpawnTime;
            var spawnLimitReached = freshAsteroids.Count >= asteroidSpawnLimit;

            var spawnNewAsteroid = spawnTimeReached && !spawnLimitReached;
            if (spawnNewAsteroid)
            {
                previousSpawnTime = gameTime.TotalGameTime;

                // Add an asteroid
                var asteroid = CreateAsteroid(content, graphicsDevice, player);
                freshAsteroids.Add(asteroid);
                entityManager.Add(asteroid);
            }

            // Remove dead asteroids
            for (int i = asteroids.Count - 1; i >= 0; i--)
            {
                var asteroid = asteroids[i];
                if (asteroid.Dead)
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
                        a2.Position = asteroid.Position;
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

        public Explosion CreateExplosion(Sizes size, Vector2 position, ContentManager content, GraphicsDevice graphicsDevice)
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

            var texture = fun(size);
            var explosion = new Explosion(entityManager, texture);
            explosion.Initialize(content, graphicsDevice);
            explosion.Position = new Vector2(position.X, position.Y);

            return explosion;
        }
    }
}
