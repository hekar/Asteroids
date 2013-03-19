using System.Collections.Generic;
using Asteroids_Xbox.Manager;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Asteroids_Xbox.Entities
{
    /// <summary>
    /// Current status of the title screen
    /// </summary>
    enum TitlescreenStatus
    {
        Start,
        GameOver,
        Help,
        Pause
    }

    /// <summary>
    /// Display and process the titlescreen
    /// 
    /// TODO: Implement gameover screen
    /// </summary>
    class Titlescreen : AnimatedEntity
    {
        /// <summary>
        /// Player
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// Titlescreen status
        /// </summary>
        public TitlescreenStatus TitlescreenStatus { get; set; }

        private bool visible;

        /// <summary>
        /// Is the titlescreen currently visible
        /// </summary>
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

        /// <summary>
        /// This property is a hack. Basically is a new game being requested by the titlescreen
        /// </summary>
        public bool NewGameRequested { get; set; }

        /// <summary>
        /// Is the titlescreen asking for an exit
        /// </summary>
        public bool ExitRequested { get; private set; }

        private SpriteFont font;
        private Song menuSong;
        private Song gameSong;
        private bool isGamepad;

        public Titlescreen(Player player)
        {
            this.Player = player;
        }

        public override void Load(ContentManager content)
        {
            NewGameRequested = false;
            ExitRequested = false;
            TitlescreenStatus = TitlescreenStatus.Start;
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
            catch 
            {
                // Ignore...
            }
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
                TitlescreenStatus = TitlescreenStatus.Help;
            }
            else if (keyboard.IsKeyDown(Keys.Space) ||
                keyboard.IsKeyDown(Keys.Enter) ||
                gamepad.Buttons.A == ButtonState.Pressed)
            {
                if (Player.Alive)
                {
                    // Resume game
                    Visible = false;
                }
                else
                {
                    // Start new game
                    Visible = false;
                    NewGameRequested = true;
                }
            }


            if (inputManager.WasKeyPressed(Keys.Escape) ||
                    inputManager.WasButtonPressed(Buttons.Back))
            {
                switch (TitlescreenStatus)
                {
                    case TitlescreenStatus.Start:
                        ExitRequested = true;
                        break;

                    case TitlescreenStatus.GameOver:
                    case TitlescreenStatus.Help:
                        TitlescreenStatus = TitlescreenStatus.Start;
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

            switch (TitlescreenStatus)
            {
                case TitlescreenStatus.Start:
                    WriteTitle(spriteBatch, "Captain Asteroids");
                    WriteSubTitle(spriteBatch, "Press Enter (A) to Start");
                    break;
                case TitlescreenStatus.GameOver:
                    WriteTitle(spriteBatch, "Game Over!");
                    break;
                case TitlescreenStatus.Help:
                    WriteHelp(spriteBatch);
                    break;
                default:
                    break;
            }
        }

        private void WriteHelp(SpriteBatch spriteBatch)
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

        private void WriteTitle(SpriteBatch spriteBatch, string text)
        {
            var offset = font.MeasureString(text);
            var pos = new Vector2
            (
                GraphicsDevice.Viewport.X + (GraphicsDevice.Viewport.Width / 2) - (offset.X / 2),
                GraphicsDevice.Viewport.Y + (GraphicsDevice.Viewport.Height / 3) - (offset.Y / 2)
            );

            spriteBatch.DrawString(font, text, pos, Color.Green);
        }

        private void WriteSubTitle(SpriteBatch spriteBatch, string text)
        {
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
