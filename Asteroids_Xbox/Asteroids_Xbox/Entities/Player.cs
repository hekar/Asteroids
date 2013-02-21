using Asteroids_Xbox.Manager;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids_Xbox.Entities
{
    class Player : Entity
    {
        private const string PlayerTextureName = "shipAnimation";

        private readonly Color BackgroundColor = Color.White;

        public Animation Animation;

        public int Health;

        public int Lives;

        public int Score;

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

        // TODO: Pass in bullet
        public Player()
        {
        }

        public override void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;

            Animation = new Animation();
            Texture2D playerTexture = content.Load<Texture2D>(PlayerTextureName);
            Animation.Initialize(playerTexture, Vector2.Zero, 75, 30, 8, 30, BackgroundColor, 1f, true);

            Active = true;
            Health = 100;
            Score = 0;
            Lives = 3;
            MoveSpeed = 8.0f;
            MaxSpeed = 10.0f;
            RotationSpeed = 5.0f;
            CurrentSpeed = Vector2.Zero;

            Position = new Vector2
            (
                graphicsDevice.Viewport.TitleSafeArea.X + graphicsDevice.Viewport.TitleSafeArea.Width / 2,
                graphicsDevice.Viewport.TitleSafeArea.Y + graphicsDevice.Viewport.TitleSafeArea.Height / 2
            );
        }

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
            else if (keyboard.IsKeyDown(Keys.Down) ||
                gamepad.DPad.Down == ButtonState.Pressed)
            {
                Backward();
            }
            else
            {
                // Deccelerate...
                CurrentSpeed = new Vector2(CurrentSpeed.X / 1.15f, CurrentSpeed.Y / 1.15f);
            }

            Move(CurrentSpeed.X, CurrentSpeed.Y);

            if (keyboard.IsKeyDown(Keys.Space) ||
                gamepad.Buttons.A == ButtonState.Pressed)
            {
                FireBullet(CurrentSpeed);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch);
        }

        private Bullet FireBullet(Vector2 speed)
        {
            var bullet = new Bullet(this, speed, Rotation);
            return bullet;
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
