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
    class Asteroid : AnimatedEntity
    {
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
        /// Is the asteroid dead?
        /// </summary>
        public bool Dead
        {
            get
            {
                return Health <= 0;
            }
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

        public override void Load(ContentManager content)
        {
            asteroidTexture = content.Load<Texture2D>(asteroidTextureName);
            Animation.Initialize(asteroidTexture, Vector2.Zero,
                asteroidTexture.Width, asteroidTexture.Height, 1, 30, Color.White, 1f, true);

            Health = 50;
            Damage = 100;
            ScoreWorth = 100;

            MoveSpeed = 1.0f;
            MaxSpeed = MoveSpeed;
            RotationSpeed = 2.0f;
            WrapScreen = false;
            CurrentSpeed = new Vector2(MoveSpeed, MoveSpeed);
        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            // TODO: Implement asteroid random floating shit bzzzzzzz
            Rotate(RotationSpeed);

            base.Update(inputManager, gameTime);
        }
    }
}
