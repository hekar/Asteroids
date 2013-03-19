using Asteroids_Xbox.Manager;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Asteroids_Xbox.Entities
{
    /// <summary>
    /// Display and process the titlescreen
    /// 
    /// TODO: Implement gameover screen
    /// </summary>
    class Titlescreen : AnimatedEntity
    {
        private bool visible;

        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                if (visible)
                {
                    PlayMusic(menuSong);
                }
                else
                {
                    PlayMusic(gameSong);
                }
            }
        }

        private SpriteFont font;
        enum GameStatus { start, gameOver, help, pause };
        private GameStatus gameStatus;
        public bool ExitRequested { get; private set; }
        private Song menuSong;
        private Song gameSong;
        private bool isGamepad;
        public override void Load(ContentManager content)
        {
            ExitRequested = false;
            gameStatus = GameStatus.start;
            font = content.Load<SpriteFont>("gameFont");

            menuSong = content.Load<Song>("sound/menuMusic");
            gameSong = content.Load<Song>("sound/gameMusic");

            var texture = content.Load<Texture2D>("mainMenu");
            Animation.Initialize(texture, Vector2.Zero, texture.Width, texture.Height, 1, 30, Color.White, 1f, true);

            MoveSpeed = 8.0f;
            MaxSpeed = 10.0f;
            RotationSpeed = 5.0f;
            CurrentSpeed = Vector2.Zero;
            WrapScreen = true;

            Position = new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y);
        }

        private void PlayMusic(Song song)
        {
            // Due to the way the MediaPlayer plays music,
            // we have to catch the exception. Music will play when the game is not tethered
            try
            {
                MediaPlayer.Stop();
                // Play the music
                MediaPlayer.Play(song);

                // Loop the currently playing song
                MediaPlayer.IsRepeating = true;
            }
            catch { }
        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            // Handle controls
            var keyboard = inputManager.CurrentKeyboardState;
            var gamepad = inputManager.CurrentGamePadState;

            if (keyboard.IsKeyDown(Keys.F1) ||
                gamepad.Buttons.Y == ButtonState.Pressed)
            {
                if (gamepad.Buttons.Y == ButtonState.Pressed)
                {
                    isGamepad = true;
                }
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
                GraphicsDevice.Viewport.Width / 2, 0
            );
            spriteBatch.DrawString(font, text, pos, Color.Green);
            List<string> texts = new List<string>();
            if (isGamepad)
            {
                text = "Press A to shoot";
                texts.Add(text);
                text = "Use D-Pad to move";
                texts.Add(text);
            }
            else
            {
                text = "Press Spacebar to shoot";
                texts.Add(text);
                text = "Use Arrows to move";
                texts.Add(text);
            }
            int i = 1;
            foreach (var line in texts)
            {
                offset = font.MeasureString(line);
                pos = new Vector2
                (
                    GraphicsDevice.Viewport.X + GraphicsDevice.Viewport.Width / 4, (offset.Y * i)
                );
                spriteBatch.DrawString(font, text, pos, Color.Green);
                i++;
            }
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
