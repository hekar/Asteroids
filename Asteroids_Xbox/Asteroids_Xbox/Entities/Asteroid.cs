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
        private Animation Animation { get; set; }

        /// <summary>
        /// The hit points of the Asteroid, if this goes to zero the Asteroid EXPPLODES INTO PIECES OF 
        /// BORA DSAFOJSDAFJSADOFJASDOIFJASKDFJLAKSDJFLK;ASDJFKL;ASDJFKLASJDFKL;JASDKFL;JSADKLFJSKLAD;F
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// The amount of damage the Asteroid inflicts on the player ship
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// The amount of score the Asteroid will give to the player
        /// </summary>
        public int ScoreWorth { get; set; }

        /// <summary>
        /// Get the width of the Asteroid
        /// </summary>
        public int Width
        {
            get { return Animation.FrameWidth; }
        }

        /// <summary>
        /// Get the height of the Asteroid
        /// </summary>
        public int Height
        {
            get { return Animation.FrameHeight; }
        }

        private readonly AsteroidManager asteroidManager;
        private readonly Player player;
        private Texture2D asteroidTexture;
        private string asteroidTextureName;

        public Asteroid(AsteroidManager asteroidManager, Player player) : this(asteroidManager, player, "asteroidLarge")
        {
        }

        public Asteroid(AsteroidManager asteroidManager, Player player, string asteroidTextureName)
        {
            this.asteroidManager = asteroidManager;
            this.player = player;
            this.asteroidTextureName = asteroidTextureName;
        }

        public override void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            // TODO: Make asteroid animation
            Animation = new Animation();
            asteroidTexture = content.Load<Texture2D>(asteroidTextureName);
            Animation.Initialize(asteroidTexture, Vector2.Zero, 
                asteroidTexture.Width, asteroidTexture.Height, 1, 30, Color.White, 1f, false);

            Active = true;
            Health = 50;
            Damage = 100;
            ScoreWorth = 100;

            MoveSpeed = 5.0f;
            MaxSpeed = MoveSpeed;
            RotationSpeed = 15.0f;
        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            // Implement asteroid random floating shit bzzzzzzz
            // BZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ

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
                    player.Score += ScoreWorth;
                }

                asteroidManager.Remove(this);
            }
            else
            {
                Rotation += RotationSpeed;
                Animation.Rotation = Rotation;
                Forward();
            }

            // Update the position of the Animation
            Animation.Position = Position;

            // Update Animation
            Animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the animation
            Animation.Draw(spriteBatch);
        }
    }
}
