using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Asteroids_Xbox.Entities;
using Microsoft.Xna.Framework;

namespace Asteroids_Xbox.Manager
{
    class AsteroidManager
    {
        // asteroids
        //Texture2D asteroidTexture; //To implement
        private List<Asteroid> asteroids;

        // The rate at which the asteroids appear
        private TimeSpan asteroidSpawnTime;
        private TimeSpan previousSpawnTime;

        public AsteroidManager()
        {
            asteroids = new List<Asteroid>();
            // Set the time keepers to zero
            previousSpawnTime = TimeSpan.Zero;
            // Used to determine how fast asteroids spawns
            asteroidSpawnTime = TimeSpan.FromSeconds(.5f);
        }
    

        // No asteroid texture and the asteroid class does not have the
        // algorythm for floating around the screen in place yet.
        //private void AddAsteroid()
        //{
        //    // Create the animation object
        //    Animation asteroidAnimation = new Animation();
        //    // Initialize the animation with the correct animation information
        //    asteroidAnimation.Initialize(asteroidTexture, Vector2.Zero, 10, 10, 8, 30, Color.White, 1f, true);
        //    // Randomly generate the position of the asteroid
        //    Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + asteroidTexture.Width / 2, random.Next(asteroidTexture.Height / 2, GraphicsDevice.Viewport.Height - asteroidTexture.Height));
        //    // Create an asteroid
        //    Asteroid asteroid = new Asteroid();
        //    // Initialize the asteroid
        //    asteroid.Initialize(asteroidAnimation, position);
        //    // Add the asteroid to the active asteroids list
        //    asteroids.Add(asteroid);
        //}

        public void Remove(Asteroid asteroid)
        {
            throw new NotImplementedException();
        }

        internal void Update(GameTime gameTime)
        {
            // Spawn a new asteroid asteroid every 1.5 seconds
            if (gameTime.TotalGameTime - previousSpawnTime > asteroidSpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;
                // Add an asteroid
                //AddAsteroid();
            }
        }
    }
}
