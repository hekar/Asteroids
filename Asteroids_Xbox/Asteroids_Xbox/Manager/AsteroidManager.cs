using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Asteroids_Xbox.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Manager
{
    class AsteroidManager
    {
        private EntityManager entityManager;

        // asteroids
        //Texture2D asteroidTexture; //To implement
        private List<Asteroid> asteroids;

        // The rate at which the asteroids appear
        private TimeSpan asteroidSpawnTime;
        private TimeSpan previousSpawnTime;

        public AsteroidManager(EntityManager entityManager)
        {
            this.entityManager = entityManager;

            asteroids = new List<Asteroid>();
            // Set the time keepers to zero
            previousSpawnTime = TimeSpan.Zero;
            // Used to determine how fast asteroids spawns
            asteroidSpawnTime = TimeSpan.FromSeconds(.5f);
        }

        // No asteroid texture and the asteroid class does not have the
        // algorythm for floating around the screen in place yet.
        public Asteroid CreateAsteroid(ContentManager content, GraphicsDevice graphicsDevice, Player player)
        {
            // Create an asteroid
            Asteroid asteroid = new Asteroid(this, player);
            asteroid.Initialize(content, graphicsDevice);

            var rand = new Random();
            // Randomly generate the position of the asteroid
            asteroid.Position = new Vector2(graphicsDevice.Viewport.Width + asteroid.Width / 2,
                rand.Next(asteroid.Height / 2, graphicsDevice.Viewport.Height - asteroid.Height));

            asteroids.Add(asteroid);

            return asteroid;
        }

        public void Remove(Asteroid asteroid)
        {
            throw new NotImplementedException();
        }

        public void Update(ContentManager content, GraphicsDevice graphicsDevice, Player player, GameTime gameTime)
        {
            // Spawn a new asteroid asteroid every 1.5 seconds
            if (gameTime.TotalGameTime - previousSpawnTime > asteroidSpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;
                // Add an asteroid
                var asteroid = CreateAsteroid(content, graphicsDevice, player);
                entityManager.Add(asteroid);
            }
        }
    }
}
