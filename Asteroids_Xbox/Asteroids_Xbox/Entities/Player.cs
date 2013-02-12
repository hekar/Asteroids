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
        // Animation representing the player
        public Animation PlayerAnimation;

        // State of the player
        public bool Active;

        // Amount of hit points that player has
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

            Animation animation = new Animation();
            Texture2D playerTexture = content.Load<Texture2D>("shipAnimation");
            animation.Initialize(playerTexture, Vector2.Zero, 75, 30, 8, 30, Color.White, 1f, true);

            PlayerAnimation = animation;

            // Set the player to be active
            Active = true;

            // Set the player health
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

            var currentKeyboardState = inputManager.CurrentKeyboardState;
            var currentGamePadState = inputManager.CurrentGamePadState;

            // Get Thumbstick Controls
            Move
            (
                currentGamePadState.ThumbSticks.Left.X * moveSpeed,
                -(currentGamePadState.ThumbSticks.Left.Y * moveSpeed)
            );

            // Use the Keyboard / Dpad
            if (currentKeyboardState.IsKeyDown(Keys.Left) ||
                currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                Move(-moveSpeed, 0.0f);
            }

            if (currentKeyboardState.IsKeyDown(Keys.Right) ||
                currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                Move(moveSpeed, 0.0f);
            }

            if (currentKeyboardState.IsKeyDown(Keys.Up) ||
                currentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                Move(0.0f, -moveSpeed);
            }

            if (currentKeyboardState.IsKeyDown(Keys.Down) ||
                currentGamePadState.DPad.Down == ButtonState.Pressed)
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
