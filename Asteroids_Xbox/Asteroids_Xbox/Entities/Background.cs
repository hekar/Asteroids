using System;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Entities
{
    class Background : AnimatedEntity
    {
        private readonly string[] BackgroundContentNames = new string[]
        {
            "space1"
        };

        public override void Load(ContentManager content)
        {
            var rand = new Random();
            var backgroundName = BackgroundContentNames[rand.Next(0, BackgroundContentNames.Length)];
            Texture2D texture = content.Load<Texture2D>("space1");

            Animation.Initialize(texture, Vector2.Zero, texture.Width, texture.Height, 1, 30, Color.White, 1f, true);
            //Animation.Initialize(texture, Vector2.Zero, 75, 30, 8, 30, Color.White, 1f, true);

            MoveSpeed = 8.0f;
            MaxSpeed = 10.0f;
            RotationSpeed = 5.0f;
            CurrentSpeed = Vector2.Zero;
            WrapScreen = true;

            Position = new Vector2
            (
                GraphicsDevice.Viewport.TitleSafeArea.X,
                GraphicsDevice.Viewport.TitleSafeArea.Y
            );
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
