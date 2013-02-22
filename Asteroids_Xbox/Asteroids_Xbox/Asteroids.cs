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
        private readonly AsteroidManager asteroidManager;
        private readonly InputManager inputManager;
        private readonly EntityManager entityManager;

        // Entities
        private Player player;
        private ScoreDisplay scoreDisplay;

        public Asteroids()
        {
            graphics = new GraphicsDeviceManager(this);

            entityManager = new EntityManager();
            inputManager = new InputManager();
            asteroidManager = new AsteroidManager(entityManager);

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Load the game
        /// </summary>
        protected override void Initialize()
        {
            entityManager.Add(new Background());

            // TODO: Move this code elsewhere
            player = new Player();
            entityManager.Add(player);

            scoreDisplay = new ScoreDisplay(new List<Player>(new Player[] {player}));
            entityManager.Add(scoreDisplay);

            base.Initialize();
        }

        /// <summary>
        /// Load the game content
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            entityManager.Initialize(Content, GraphicsDevice);
        }

        /// <summary>
        /// Unload the game content
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Hack: Exit game on Xbox 360
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            inputManager.PreviousKeyboardState = inputManager.CurrentKeyboardState;
            inputManager.CurrentKeyboardState = Keyboard.GetState();

            inputManager.PreviousGamePadState = inputManager.CurrentGamePadState;
            inputManager.CurrentGamePadState = GamePad.GetState(PlayerIndex.One);

            asteroidManager.Update(Content, GraphicsDevice, player, gameTime);
            entityManager.Update(inputManager, gameTime);

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
            entityManager.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
