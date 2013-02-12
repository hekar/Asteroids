using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Asteroids_Xbox.Manager;

namespace Asteroids_Xbox.Entities
{
    class ScoreDisplay : Entity
    {
        private SpriteFont font;
        private readonly List<Player> players = new List<Player>();
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
                    new Vector2(graphicsDevice.Viewport.X, graphicsDevice.Viewport.Y), Color.Black);

                // Draw the lives of the player
                spriteBatch.DrawString(font, "Lives: " + player.Lives,
                    new Vector2(graphicsDevice.Viewport.X, graphicsDevice.Viewport.Y + 60), Color.White);    
            }
        }
    }
}
