using System;
using System.Collections.Generic;
using Asteroids_Xbox.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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

        private readonly List<Explosion> explosions = new List<Explosion>();

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
        private int asteroidSpawnLimit;

        public enum Sizes { Small = 1, Medium, Large };

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
            asteroid.Position = new Vector2(graphicsDevice.Viewport.Width + asteroid.Width / 2,
                random.Next(asteroid.Height / 2, graphicsDevice.Viewport.Height - asteroid.Height));

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
                    // Add an explosion
                    //TODO Toggle what explosion size.
                    var explosion = CreateExplosion(Sizes.Large, content, graphicsDevice);
                    explosions.Add(explosion);
                    entityManager.Add(explosion);
                    explosion.explodeSound.Play();


                    // TODO: Play the explosion sound
                    //explosionSound.Play();

                    //Add to the player's score
                    player.Score += asteroid.ScoreWorth;

                    Remove(asteroid);
                }
                if (asteroid.Offscreen)
                {
                    //Wrap around.
                }
            }
        }

        public Explosion CreateExplosion(Sizes size, ContentManager content, GraphicsDevice graphicsDevice/*, Asteroid asteroid*/)
        {
            Explosion explosion;
            switch (size)
            {
                case Sizes.Small:
                    explosion = new Explosion("asteroidSmall");
                    break;
                case Sizes.Medium:
                    explosion = new Explosion("asteroidMedium");
                    break;
                case Sizes.Large:
                    explosion = new Explosion("asteroidLarge");
                    break;
                default:
                    explosion = new Explosion("asteroidLarge");
                    break;
            }


            explosion.Initialize(content, graphicsDevice);

            // TODO: Randomly generate the position of the asteroid
            explosion.Position = new Vector2(graphicsDevice.Viewport.Width / 4, graphicsDevice.Viewport.Height / 4);

            explosions.Add(explosion);

            return explosion;
        }
    }
}
