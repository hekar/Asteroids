using System.Collections.Generic;
using Asteroids_Xbox.Manager;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Entities
{
    /// <summary>
    /// Display the score for the player and their remaining lives
    /// </summary>
    class ScoreDisplay : Entity
    {
        private readonly List<Player> players = new List<Player>();
        private readonly Color FontColor = Color.FloralWhite;

        private SpriteFont font;
        private GraphicsDevice graphicsDevice;

        public ScoreDisplay(List<Player> players)
        {
            this.players.AddRange(players);
        }

        public override void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            font = content.Load<SpriteFont>("gameFont");
            this.graphicsDevice = graphicsDevice;
        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var player in players)
            {
                // Draw the score
                spriteBatch.DrawString(font, "Score: " + player.Score,
                    new Vector2(graphicsDevice.Viewport.X, graphicsDevice.Viewport.Y), FontColor);

                // Draw the lives of the player
                spriteBatch.DrawString(font, "Lives: " + player.Lives,
                    new Vector2(graphicsDevice.Viewport.X, graphicsDevice.Viewport.Y + 35), FontColor);    
            }
        }
    }
}
