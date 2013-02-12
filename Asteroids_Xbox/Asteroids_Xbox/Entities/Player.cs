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
        public Animation PlayerAnimation;

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

        private float moveSpeed;
        private GraphicsDevice graphicsDevice;

        // Get the width of the player ship
        public int Width
        {
            get { return PlayerAnimation.FrameWidth; }
        }

        // Get the height of the player ship
        public int Height
        {
            get { return PlayerAnimation.FrameHeight; }
        }

        // Initialize the player
        public override void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;

            PlayerAnimation = new Animation();
            Texture2D playerTexture = content.Load<Texture2D>("shipAnimation");
            PlayerAnimation.Initialize(playerTexture, Vector2.Zero, 75, 30, 8, 30, Color.White, 1f, true);

            Active = true;
            Health = 100;
            Lives = 3;
            moveSpeed = 8.0f;

            Position = new Vector2
            (
                graphicsDevice.Viewport.TitleSafeArea.X + graphicsDevice.Viewport.TitleSafeArea.Width / 2,
                graphicsDevice.Viewport.TitleSafeArea.Y + graphicsDevice.Viewport.TitleSafeArea.Height / 2
            );
        }

        // Update the player animation
        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            PlayerAnimation.Position = Position;
            PlayerAnimation.Update(gameTime);

            var keyboard = inputManager.CurrentKeyboardState;
            var gamepad = inputManager.CurrentGamePadState;

            // Get Thumbstick Controls
            Move
            (
                gamepad.ThumbSticks.Left.X * moveSpeed,
                -(gamepad.ThumbSticks.Left.Y * moveSpeed)
            );

            // Use the Keyboard / Dpad
            if (keyboard.IsKeyDown(Keys.Left) ||
                gamepad.DPad.Left == ButtonState.Pressed)
            {
                Move(-moveSpeed, 0.0f);
            }

            if (keyboard.IsKeyDown(Keys.Right) ||
                gamepad.DPad.Right == ButtonState.Pressed)
            {
                Move(moveSpeed, 0.0f);
            }

            if (keyboard.IsKeyDown(Keys.Up) ||
                gamepad.DPad.Up == ButtonState.Pressed)
            {
                Move(0.0f, -moveSpeed);
            }

            if (keyboard.IsKeyDown(Keys.Down) ||
                gamepad.DPad.Down == ButtonState.Pressed)
            {
                Move(0.0f, moveSpeed);
            }

            // Make sure that the player does not go out of bounds
            Position = new Vector2
            (
                MathHelper.Clamp(Position.X, 0, graphicsDevice.Viewport.Width),
                MathHelper.Clamp(Position.Y, 0, graphicsDevice.Viewport.Height)
            );
        }

        // Draw the player
        public override void Draw(SpriteBatch spriteBatch)
        {
            PlayerAnimation.Draw(spriteBatch);
        }
    }
}
