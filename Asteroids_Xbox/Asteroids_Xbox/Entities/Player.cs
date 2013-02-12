using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework.Content;
using Asteroids_Xbox.Manager;
using Microsoft.Xna.Framework.Input;

namespace Asteroids_Xbox.Entities
{
    class Player : Entity
    {
        /// <summary>
        /// Animation representing the player
        /// </summary>
        public Animation Animation;

        /// <summary>
        /// State of the player
        /// </summary>
        public bool Active;

        /// <summary>
        /// Amount of hit points that player has
        /// </summary>
        public int Health;

        public int Lives;

        public int Score;

        private Vector2 currentSpeed;
        private float maxSpeed;
        private float moveSpeed;
        private GraphicsDevice graphicsDevice;

        // Get the width of the player ship
        public int Width
        {
            get { return Animation.FrameWidth; }
        }

        // Get the height of the player ship
        public int Height
        {
            get { return Animation.FrameHeight; }
        }

        // Initialize the player
        public override void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;

            Animation = new Animation();
            Texture2D playerTexture = content.Load<Texture2D>("shipAnimation");
            Animation.Initialize(playerTexture, Vector2.Zero, 75, 30, 8, 30, Color.White, 1f, true);

            Active = true;
            Health = 100;
            Lives = 3;
            moveSpeed = 8.0f;
            maxSpeed = 10.0f;
            currentSpeed = Vector2.Zero;

            Position = new Vector2
            (
                graphicsDevice.Viewport.TitleSafeArea.X + graphicsDevice.Viewport.TitleSafeArea.Width / 2,
                graphicsDevice.Viewport.TitleSafeArea.Y + graphicsDevice.Viewport.TitleSafeArea.Height / 2
            );
        }

        // Update the player animation
        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            Animation.Position = Position;
            Animation.Update(gameTime);

            var keyboard = inputManager.CurrentKeyboardState;
            var gamepad = inputManager.CurrentGamePadState;

            // TODO: Handle thumbsticks for strafing
            // Move(gamepad.ThumbSticks.Left.X * moveSpeed, -(gamepad.ThumbSticks.Left.Y * moveSpeed));

            // Use the Keyboard / Dpad
            if (keyboard.IsKeyDown(Keys.Left) ||
                gamepad.DPad.Left == ButtonState.Pressed)
            {
                Rotate(-0.2f);
            }
            else if (keyboard.IsKeyDown(Keys.Right) ||
                gamepad.DPad.Right == ButtonState.Pressed)
            {
                Rotate(0.2f);
            }

            if (keyboard.IsKeyDown(Keys.Up) ||
                gamepad.DPad.Up == ButtonState.Pressed)
            {
                Forward(-moveSpeed);
            }
            else if (keyboard.IsKeyDown(Keys.Down) ||
                gamepad.DPad.Down == ButtonState.Pressed)
            {
                Forward(moveSpeed);
            }
            else
            {
                // Deccelerate...
                currentSpeed.X *= 0.2f;
                currentSpeed.Y *= 0.2f;
            }

            Move(currentSpeed.X, currentSpeed.Y);
        }

        // Draw the player
        public override void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch);
        }

        public void Forward(float speed)
        {
            double rad = Rotation * (Math.PI / 180);

            float speedX = (float)Math.Sin(rad) * speed;
            float speedY = (float)Math.Cos(rad) * speed;

            var nextSpeedX = MathHelper.Clamp(currentSpeed.X + speedX, -maxSpeed, maxSpeed);
            var nextSpeedY = MathHelper.Clamp(currentSpeed.Y + speedY, -maxSpeed, maxSpeed);

            currentSpeed = new Vector2(nextSpeedX, nextSpeedY);
        }

        public override void Move(float x, float y)
        {
            base.Move(x, y);

            // Make sure that the player does not go out of bounds
            if (Position.X <= 0.0f)
            {
                var width = graphicsDevice.Viewport.Width;
                Position = new Vector2(width - 1, Position.Y);
            }

            if (Position.Y <= 0.0f)
            {
                var height = graphicsDevice.Viewport.Height;
                Position = new Vector2(Position.X, height - 1);
            }

            Position = new Vector2
            (
                Position.X % graphicsDevice.Viewport.Width,
                Position.Y % graphicsDevice.Viewport.Height
            );
        }

        public override void Rotate(float angle)
        {
            base.Rotate(angle);

            Animation.Rotation = Rotation;
        }
    }
}
