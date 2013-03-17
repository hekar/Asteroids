using Asteroids_Xbox.Manager;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids_Xbox.Entities
{
    /// <summary>
    /// Display and process the titlescreen
    /// 
    /// TODO: Implement gameover screen
    /// </summary>
    class Titlescreen : AnimatedEntity
    {
        public bool Visible { get; set; }
        private SpriteFont font;
        enum GameStatus { start, gameOver, help, pause };
        private GameStatus gameStatus;
        public bool ExitRequested { get; private set; }
        public override void Load(ContentManager content)
        {
            ExitRequested = false;
            gameStatus = GameStatus.start;
            font = content.Load<SpriteFont>("gameFont");

            var texture = content.Load<Texture2D>("mainMenu");
            Animation.Initialize(texture, Vector2.Zero, texture.Width, texture.Height, 1, 30, Color.White, 1f, true);

            MoveSpeed = 8.0f;
            MaxSpeed = 10.0f;
            RotationSpeed = 5.0f;
            CurrentSpeed = Vector2.Zero;
            WrapScreen = true;

            Position = new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y);
        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            // Handle controls
            var keyboard = inputManager.CurrentKeyboardState;
            var gamepad = inputManager.CurrentGamePadState;

            if (keyboard.IsKeyDown(Keys.F1) ||
                gamepad.Buttons.Y == ButtonState.Pressed)
            {
                gameStatus = GameStatus.help;
            }
            else if (keyboard.IsKeyDown(Keys.Space) ||
                keyboard.IsKeyDown(Keys.Enter) ||
                gamepad.Buttons.A == ButtonState.Pressed)
            {
                // Start game
                Visible = false;
            }

            var escPressed = inputManager.WasKeyPressed(Keys.Escape) ||
                    inputManager.WasButtonPressed(Buttons.Back);
            if (escPressed)
            {
                switch (gameStatus)
                {
                    case GameStatus.start:
                        ExitRequested = true;
                        break;
                    case GameStatus.gameOver:
                        break;
                    case GameStatus.help:
                        gameStatus = GameStatus.start;
                        break;
                    default:
                        break;
                }
            }
            base.Update(inputManager, gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            switch (gameStatus)
            {
                case GameStatus.start:
                    WriteTitleMessage(spriteBatch);
                    WriteSubMessage(spriteBatch);
                    break;
                case GameStatus.gameOver:
                    break;
                case GameStatus.help:
                    WriteHelpMessage(spriteBatch);
                    break;
                default:
                    break;
            }
        }

        private void WriteHelpMessage(SpriteBatch spriteBatch)
        {
            var text = "Help";

            var offset = font.MeasureString(text);
            var pos = new Vector2
            (
                GraphicsDevice.Viewport.X + (GraphicsDevice.Viewport.Width / 2) - (offset.X / 2),
                GraphicsDevice.Viewport.Y + (GraphicsDevice.Viewport.Height / 4) - (offset.Y / 2)
            );

            spriteBatch.DrawString(font, text, pos, Color.Green);
        }

        private void WriteTitleMessage(SpriteBatch spriteBatch)
        {
            var text = "Captain Asteroids";

            var offset = font.MeasureString(text);
            var pos = new Vector2
            (
                GraphicsDevice.Viewport.X + (GraphicsDevice.Viewport.Width / 2) - (offset.X / 2),
                GraphicsDevice.Viewport.Y + (GraphicsDevice.Viewport.Height / 3) - (offset.Y / 2)
            );

            spriteBatch.DrawString(font, text, pos, Color.Green);
        }

        private void WriteSubMessage(SpriteBatch spriteBatch)
        {
            var text = "Press Enter (A) to Start";

            var offset = font.MeasureString(text);
            var pos = new Vector2
            (
                GraphicsDevice.Viewport.X + (GraphicsDevice.Viewport.Width / 2) - (offset.X / 2),
                GraphicsDevice.Viewport.Y + (GraphicsDevice.Viewport.Height / 2) - (offset.Y / 2)
            );

            spriteBatch.DrawString(font, text, pos, Color.White);
        }
    }
}
