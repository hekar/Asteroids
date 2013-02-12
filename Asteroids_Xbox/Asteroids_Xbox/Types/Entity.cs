using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Asteroids_Xbox.Manager;

namespace Asteroids_Xbox.Types
{
    abstract class Entity : Initializable, Updatable, Drawable
    {
        public virtual void Move(float x, float y)
        {
            this.Position = new Vector2(this.Position.X + x, this.Position.Y + y);
        }

        public virtual void Rotate(float angle)
        {
            var nextRotation = Rotation + angle;
            if (nextRotation > 0.0f)
            {
                this.Rotation = nextRotation % 360.0f;
            }
            else
            {
                this.Rotation = 360.0f + angle;
            }
        }

        public abstract void Initialize(ContentManager content, GraphicsDevice graphicsDevice);

        public abstract void Update(InputManager inputManager, GameTime gameTime);

        public float Rotation { get; set; }
        public Vector2 Position { get; set; }
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
