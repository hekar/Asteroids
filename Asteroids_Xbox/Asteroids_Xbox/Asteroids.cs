using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Asteroids_Xbox.Types;
using Asteroids_Xbox.Entities;
using Asteroids_Xbox.Manager;

namespace Asteroids_Xbox
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Asteroids : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // Managers
        private AsteroidManager asteroidManager;
        private InputManager inputManager;
        private EntityManager entityManager;

        // Screens
        private Titlescreen titleScreen;

        // Entities

        private Player player;

        public Asteroids()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Load the game
        /// </summary>
        protected override void Initialize()
        {
            NewGame();

            base.Initialize();
        }

        /// <summary>
        /// Load the game content
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            titleScreen.Initialize(Content, GraphicsDevice);
            entityManager.Initialize(Content, GraphicsDevice);

            titleScreen.Visible = true;
        }

        /// <summary>
        /// Unload the game content
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            inputManager.PreviousKeyboardState = inputManager.CurrentKeyboardState;
            inputManager.CurrentKeyboardState = Keyboard.GetState();

            inputManager.PreviousGamePadState = inputManager.CurrentGamePadState;
            inputManager.CurrentGamePadState = GamePad.GetState(PlayerIndex.One);

            if (titleScreen.Visible)
            {
                titleScreen.Update(inputManager, gameTime);
                if (titleScreen.NewGameRequested)
                {
                    NewGame();
                    titleScreen.NewGameRequested = false;
                    return;
                }
                else if (titleScreen.ExitRequested)
                {
                    this.Exit();
                }
            }
            else
            {
                var exitPressed = inputManager.WasKeyPressed(Keys.Escape) ||
                    inputManager.WasButtonPressed(Buttons.Back);
                // Hack: Show titlescreen/Pause game
                if (exitPressed)
                {
                    // Game is paused
                    titleScreen.TitlescreenStatus = TitlescreenStatus.Pause;
                    titleScreen.Visible = true;
                }
                else if (!player.Alive)
                {
                    // Gameover
                    titleScreen.TitlescreenStatus = TitlescreenStatus.GameOver;
                    titleScreen.Visible = true;
                }
                else
                {
                    asteroidManager.Update(Content, GraphicsDevice, player, gameTime);
                    entityManager.Update(inputManager, gameTime);
                }
            }
            
            base.Update(gameTime);
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            if (titleScreen.Visible)
            {
                titleScreen.Draw(spriteBatch);
            }
            else
            {
                entityManager.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Initialize the new game
        /// </summary>
        private void NewGame()
        {
            if (entityManager == null)
            {
                entityManager = new EntityManager(Content, GraphicsDevice);
            }
            else
            {
                entityManager.Clear();
            }

            if (inputManager == null)
            {
                inputManager = new InputManager();
            }

            if (asteroidManager == null)
            {
                asteroidManager = new AsteroidManager(entityManager);
            }
            else
            {
                asteroidManager.Clear();
            }

            player = new Player(entityManager);

            if (titleScreen == null)
            {
                titleScreen = new Titlescreen(player);
            }
            else
            {
                titleScreen.Player = player;
            }

            entityManager.Add(new Background());
            entityManager.Add(player);
            entityManager.Add(new ScoreDisplay(new List<Player>(new Player[] { player })));
        }

    }
}
