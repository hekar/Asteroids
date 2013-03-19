using Asteroids_Xbox.Manager;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Asteroids_Xbox.Entities
{
    class Player : AnimatedEntity
    {
        private const string PlayerTextureName = "shipAnimation";
        private const string PlayerExplosionTextureName = "Ship_Explode";

        private const double bulletFireTime = 0.5;
        private const double protectionTime = 1.0f;

        private readonly Color BackgroundColor = Color.White;

        private readonly EntityManager entityManager;
        private GameTime gameTime;

        public int Lives { get; set; }
        public int Score { get; set; }

        public bool Won
        {
            get
            {
                return Score > 10000;
            }
        }

        public bool Alive
        {
            get
            {
                return Lives > 0;
            }
        }

        public bool UnderProtection
        {
            get
            {
                return (gameTime.TotalGameTime.TotalSeconds - lastRespawnTime) <= protectionTime;
            }
        }

        private double previousSeconds;
        private double lastRespawnTime;
        private ContentManager content;

        public Player(EntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        public override void Load(ContentManager content)
        {
            this.content = content;
            Texture2D playerTexture = content.Load<Texture2D>(PlayerTextureName);
            Animation.Initialize(playerTexture, Vector2.Zero, 75, 30, 8, 45, BackgroundColor, 1f, true);

            Score = 0;
            Lives = 3;
            MoveSpeed = 1.5f;
            MaxSpeed = 2.5f;
            RotationSpeed = 5.0f;
            CurrentSpeed = Vector2.Zero;
            WrapScreen = true;

            Respawn(false);
        }

        private void Respawn(bool died)
        {
            if (gameTime != null)
            {
                lastRespawnTime = gameTime.TotalGameTime.TotalSeconds; 
            }
            
            if (died)
            {
                var explosion = CreateExplosion(Position, content, GraphicsDevice);
                entityManager.Add(explosion);
                explosion.PlayExplosionSound();
            }

            Position = new Vector2
            (
                GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2,
                GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2
            );
        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            this.gameTime = gameTime;

            // Handle controls
            var keyboard = inputManager.CurrentKeyboardState;
            var gamepad = inputManager.CurrentGamePadState;

            if (keyboard.IsKeyDown(Keys.Left) ||
                gamepad.DPad.Left == ButtonState.Pressed)
            {
                Rotate(-RotationSpeed);
            }
            else if (keyboard.IsKeyDown(Keys.Right) ||
                gamepad.DPad.Right == ButtonState.Pressed)
            {
                Rotate(RotationSpeed);
            }

            if (keyboard.IsKeyDown(Keys.Up) ||
                gamepad.DPad.Up == ButtonState.Pressed)
            {
                Forward();
            }
            else
            {
                // Deccelerate...
                CurrentSpeed = new Vector2(CurrentSpeed.X / 1.05f, CurrentSpeed.Y / 1.05f);
            }

            base.Update(inputManager, gameTime);

            if (keyboard.IsKeyDown(Keys.Space) ||
                gamepad.Buttons.A == ButtonState.Pressed)
            {
                FireBullet(new Vector2(5.0f, 5.0f));
            }
        }

        private Bullet FireBullet(Vector2 speed)
        {
            // If someone is playing around midnight, they get a lucky shot that
            // has no delay
            var totalSeconds = DateTime.Now.TimeOfDay.TotalSeconds;
            var timeSinceLast = totalSeconds - previousSeconds;
            if (timeSinceLast > bulletFireTime)
            {
                Vector2 bulletPosition = new Vector2(Position.X + (Width / 2), Position.Y + (Height / 2));
                var bullet = new Bullet(entityManager, bulletPosition, speed, Rotation);
                entityManager.Add(bullet);
                bullet.laserSound.Play();
                previousSeconds = totalSeconds;
                return bullet;
            }
            else
            {
                return null;
            }
        }

        public override void Touch(AnimatedEntity other)
        {
            if (!UnderProtection && other is Asteroid)
            {
                Kill();
            }

            base.Touch(other);
        }

        public void Kill()
        {
            Lives -= 1;
            if (!Alive)
            {
                // Game over...
            }
            else
            {
                Respawn(true);
            }
        }

        public Explosion CreateExplosion(Vector2 position, ContentManager content, GraphicsDevice graphicsDevice)
        {
            var explosion = new Explosion(entityManager, PlayerExplosionTextureName);
            explosion.Initialize(content, graphicsDevice);
            explosion.Position = new Vector2(position.X, position.Y);
            return explosion;
        }

    }
}
