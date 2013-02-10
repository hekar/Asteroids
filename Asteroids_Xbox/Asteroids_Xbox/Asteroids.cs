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

namespace Asteroids_Xbox
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Asteroids : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;

        // Keyboard states used to determine key presses
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        // Gamepad states used to determine button presses
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        // A movement speed for the player
        float playerMoveSpeed;

        // Image used to display the static background
        //Texture2D mainBackground;       //To implement
        //Texture2D gameOverBackground;   //To implement
        //Texture2D mainMenuBackground;   //To implement

        // asteroids
        //Texture2D asteroidTexture; //To implement
        List<Asteroid> asteroids;

        // The rate at which the asteroids appear
        TimeSpan asteroidSpawnTime;
        TimeSpan previousSpawnTime;

        // The font used to display UI elements
        SpriteFont font;

        Random random;

        public Asteroids()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player();
            playerMoveSpeed = 8.0f;

            // Initialize the asteroids list
            asteroids = new List<Asteroid>();

            // Set the time keepers to zero
            previousSpawnTime = TimeSpan.Zero;

            // Used to determine how fast asteroids spawns
            asteroidSpawnTime = TimeSpan.FromSeconds(.5f);

            player.Score = 0;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            // Load the player resources
            Animation playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>("shipAnimation");
            playerAnimation.Initialize(playerTexture, Vector2.Zero, 75, 30, 8, 30, Color.White, 1f, true);

            // Start the player on the screen in the center
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Y
            + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(playerAnimation, playerPosition);

            // Missing this from the resources. Needs to be drawn up in PAAAAAAAAAAIIIINNNNNTTTT
            //asteroidTexture = Content.Load<Texture2D>("asteroidAnimation");

            // Need to draw up some background images for these.
            //mainBackground = Content.Load<Texture2D>("mainbackground");
            //gameOverBackground = Content.Load<Texture2D>("endMenu");
            //mainMenuBackground = Content.Load<Texture2D>("mainMenu");

            // Load the score font
            font = Content.Load<SpriteFont>("gameFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>Class1.cs
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            // Save the previous state of the keyboard and game pad so we can determinesingle key/button presses
            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            // Read the current state of the keyboard and gamepad and store it
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            //Update the player
            UpdatePlayer(gameTime);

            // Update the asteroids
            //UpdateAsteroids(gameTime); //Wont work because no asteroid texture

            base.Update(gameTime);
        }

        /// <summary>
        /// This is not even used right now because asteroids class is missing the
        /// algorythm to allow them to float around and there is no image file
        /// for asteroids yet.
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateAsteroids(GameTime gameTime)
        {
            // Spawn a new asteroid asteroid every 1.5 seconds
            if (gameTime.TotalGameTime - previousSpawnTime > asteroidSpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;
                // Add an asteroid
                //AddAsteroid();
            }

            // Update the asteroids
            for (int i = asteroids.Count - 1; i >= 0; i--)
            {
                asteroids[i].Update(gameTime);

                if (asteroids[i].Active == false)
                {
                    // If not active and health <= 0
                    if (asteroids[i].Health <= 0)
                    {
                        // Add an explosion - TODO
                        // This should somehow be a breaking apart like the real game...
                        // not sure how to do this yet.
                        //AddExplosion(asteroids[i].Position);

                        // Play the explosion sound - TODO
                        //explosionSound.Play();

                        //Add to the player's score
                        player.Score += asteroids[i].Value;
                    }
                    asteroids.RemoveAt(i);
                }
            }
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update(gameTime);

            // Get Thumbstick Controls
            player.Position.X += currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
            player.Position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;

            // Use the Keyboard / Dpad
            if (currentKeyboardState.IsKeyDown(Keys.Left) ||
            currentGamePadState.DPad.Left == ButtonState.Pressed)
                player.Position.X -= playerMoveSpeed;

            if (currentKeyboardState.IsKeyDown(Keys.Right) ||
            currentGamePadState.DPad.Right == ButtonState.Pressed)
                player.Position.X += playerMoveSpeed;

            if (currentKeyboardState.IsKeyDown(Keys.Up) ||
            currentGamePadState.DPad.Up == ButtonState.Pressed)
                player.Position.Y -= playerMoveSpeed;

            if (currentKeyboardState.IsKeyDown(Keys.Down) ||
            currentGamePadState.DPad.Down == ButtonState.Pressed)
                player.Position.Y += playerMoveSpeed;

            // Make sure that the player does not go out of bounds
            player.Position.X = MathHelper.Clamp(player.Position.X, 0, GraphicsDevice.Viewport.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, GraphicsDevice.Viewport.Height);
        }

        // No asteroid texture and the asteroid class does not have the
        // algorythm for floating around the screen in place yet.
        //private void AddAsteroid()
        //{
        //    // Create the animation object
        //    Animation asteroidAnimation = new Animation();
        //    // Initialize the animation with the correct animation information
        //    asteroidAnimation.Initialize(asteroidTexture, Vector2.Zero, 10, 10, 8, 30, Color.White, 1f, true);
        //    // Randomly generate the position of the asteroid
        //    Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + asteroidTexture.Width / 2, random.Next(asteroidTexture.Height / 2, GraphicsDevice.Viewport.Height - asteroidTexture.Height));
        //    // Create an asteroid
        //    Asteroid asteroid = new Asteroid();
        //    // Initialize the asteroid
        //    asteroid.Initialize(asteroidAnimation, position);
        //    // Add the asteroid to the active asteroids list
        //    asteroids.Add(asteroid);
        //}

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            // Draw the score
            spriteBatch.DrawString(font, "Score: " + player.Score, new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y), Color.Black);
            // Draw the lives of the player
            spriteBatch.DrawString(font, "Lives: " + player.Lives, new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y + 60), Color.White);

            // Draw the Player
            player.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
