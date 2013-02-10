using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Types
{
    class Asteroid
    {
        // Animation representing the Asteroid
        public Animation AsteroidAnimation;

        // The position of the Asteroid relative to the top left corner of thescreen
        public Vector2 Position;

        // The state of the Asteroid
        public bool Active;

        // The hit points of the Asteroid, if this goes to zero the Asteroid dies
        public int Health;

        // The amount of damage the Asteroid inflicts on the player ship
        public int Damage;

        // The amount of score the Asteroid will give to the player
        public int Value;

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

        // The speed at which the Asteroid
        float asteroidMoveSpeed;

        public void Initialize(Animation animation, Vector2 position)
        {
            // Load the Asteroid texture
            AsteroidAnimation = animation;

            // Set the position of the Asteroid
            Position = position;

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

        public void Update(GameTime gameTime)
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the animation
            AsteroidAnimation.Draw(spriteBatch);
        }
    }
}
