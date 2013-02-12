using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework.Content;
using Asteroids_Xbox.Manager;

namespace Asteroids_Xbox.Entities
{
    class Asteroid : Entity
    {
        // Animation representing the Asteroid
        public Animation AsteroidAnimation { get; set; }

        // The state of the Asteroid
        public bool Active { get; set; }

        // The hit points of the Asteroid, if this goes to zero the Asteroid dies
        public int Health { get; set; }

        // The amount of damage the Asteroid inflicts on the player ship
        public int Damage { get; set; }

        // The amount of score the Asteroid will give to the player
        public int Value { get; set; }

        // Get the width of the Asteroid
        public int Width
        {
            get { return AsteroidAnimation.FrameWidth; }
        }

        // Get the height of the Asteroid
        public int Height
        {
            get { return AsteroidAnimation.FrameHeight; }
        }

        // Speed that asteroid moves at
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

            // Missing this from the resources. Needs to be drawn up in PAAAAAAAAAAIIIINNNNNTTTT
            //asteroidTexture = Content.Load<Texture2D>("asteroidAnimation");
            //AsteroidAnimation.Initialize(playerTexture, Vector2.Zero, 75, 30, 8, 30, Color.White, 1f, true);

            // We initialize the Asteroid to be active so it will be update in the game
            Active = true;

            // Set the health of the Asteroid
            Health = 50;

            // Set the amount of damage the Asteroid can do
            Damage = 100;

            // Set how fast the Asteroid moves
            asteroidMoveSpeed = 6f;

            // Set the score value of the Asteroid
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
