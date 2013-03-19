using Asteroids_Xbox.Manager;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Entities
{
    /// <summary>
    /// Asteroid floating in space
    /// </summary>
    class Asteroid : AnimatedEntity
    {
        /// <summary>
        /// The hit points of the Asteroid
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
        /// Size of the asteroid
        /// </summary>
        public Sizes Size { get; set; }

        /// <summary>
        /// Entity that killed the asteroid
        /// </summary>
        public Entity Killer { get; set; }

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

        /// <summary>
        /// Asteroid manager
        /// </summary>
        private readonly AsteroidManager asteroidManager;

        /// <summary>
        /// Player
        /// </summary>
        private readonly Player player;

        /// <summary>
        /// Texture
        /// </summary>
        private Texture2D asteroidTexture;

        public Asteroid(AsteroidManager asteroidManager, Player player)
            : this(asteroidManager, player, Sizes.Large)
        {
        }

        public Asteroid(AsteroidManager asteroidManager, Player player, Sizes newSize)
        {
            this.asteroidManager = asteroidManager;
            this.player = player;
            this.Size = newSize;
        }

        /// <summary>
        /// Load the asteroid resources
        /// </summary>
        /// <param name="content"></param>
        public override void Load(ContentManager content)
        {
            switch (this.Size)
            {
                case Sizes.Small:
                    asteroidTexture = content.Load<Texture2D>("asteroidSmall");
                    break;
                case Sizes.Medium:
                    asteroidTexture = content.Load<Texture2D>("asteroidMedium");
                    break;
                case Sizes.Large:
                    asteroidTexture = content.Load<Texture2D>("asteroidLarge");
                    break;
                default:
                    asteroidTexture = content.Load<Texture2D>("asteroidLarge");
                    break;
            }

            Animation.Initialize(asteroidTexture, Vector2.Zero,
                asteroidTexture.Width, asteroidTexture.Height, 1, 30, Color.White, 1f, true);

            Health = 50;
            Damage = 100;

            if (Size == Sizes.Large)
            {
                ScoreWorth = 20;
            }
            else if (Size == Sizes.Medium)
            {
                ScoreWorth = 50;
            }
            else if (Size == Sizes.Small)
            {
                ScoreWorth = 100;
            }

            MoveSpeed = 1.0f;
            MaxSpeed = MoveSpeed;
            RotationSpeed = 2.0f;
            WrapScreen = true;

        }

        /// <summary>
        /// Handle update on gameloop
        /// </summary>
        /// <param name="inputManager"></param>
        /// <param name="gameTime"></param>
        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            Rotate(RotationSpeed);
            base.Update(inputManager, gameTime);
        }

        /// <summary>
        /// Handle collisions
        /// </summary>
        /// <param name="other"></param>
        public override void Touch(AnimatedEntity other)
        {
            if (other is Player)
            {
                var player = other as Player;
                player.Kill();
            }

            base.Touch(other);
        }

    }
}
