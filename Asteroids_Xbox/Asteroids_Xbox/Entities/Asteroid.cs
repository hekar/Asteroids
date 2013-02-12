using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework.Content;
using Asteroids_Xbox.Manager;

namespace Asteroids_Xbox.Entities
{
    /// <summary>
    /// Asteroid floating in space
    /// </summary>
    class Asteroid : Entity
    {
        /// <summary>
        /// Animation representing the Asteroid
        /// </summary>
        public Animation AsteroidAnimation { get; set; }

        /// <summary>
        /// The state of the Asteroid
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// The hit points of the Asteroid, if this goes to zero the Asteroid dies
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// The amount of damage the Asteroid inflicts on the player ship
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// The amount of score the Asteroid will give to the player
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Get the width of the Asteroid
        /// </summary>
        public int Width
        {
            get { return AsteroidAnimation.FrameWidth; }
        }

        /// <summary>
        /// Get the height of the Asteroid
        /// </summary>
        public int Height
        {
            get { return AsteroidAnimation.FrameHeight; }
        }

        /// <summary>
        /// Speed at which the asteroid moves
        /// </summary>
        private float asteroidMoveSpeed;

        private readonly AsteroidManager asteroidManager;
        private readonly Player player;

        public Asteroid(AsteroidManager asteroidManager, Player player)
        {
            this.asteroidManager = asteroidManager;
            this.player = player;
        }

        public override void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            // TODO: Make asteroid animation
            AsteroidAnimation = new Animation();
            //asteroidTexture = Content.Load<Texture2D>("asteroidAnimation");
            //AsteroidAnimation.Initialize(playerTexture, Vector2.Zero, 75, 30, 8, 30, Color.White, 1f, true);

            Active = true;
            Health = 50;
            Damage = 100;
            asteroidMoveSpeed = 6f;
            Value = 100;
        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            // Implement asteroid random floating shit bzzzzzzz

            // Update the position of the Animation
            AsteroidAnimation.Position = Position;

            // Update Animation
            AsteroidAnimation.Update(gameTime);

            // If the Asteroid is past the screen or its health reaches 0 then deactivate it
            if (Position.X < -Width || Health <= 0)
            {
                // By setting the Active flag to false, the game will remove this objet from the 
                // active game list
                Active = false;
            }

            if (Active == false)
            {
                // If not active and health <= 0
                if (Health <= 0)
                {
                    // Add an explosion - TODO
                    // This should somehow be a breaking apart like the real game...
                    // not sure how to do this yet.
                    //AddExplosion(asteroids[i].Position);

                    // Play the explosion sound - TODO
                    //explosionSound.Play();

                    //Add to the player's score
                    player.Score += Value;
                }

                asteroidManager.Remove(this);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the animation
            AsteroidAnimation.Draw(spriteBatch);
        }
    }
}
