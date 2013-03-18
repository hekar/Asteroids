using Asteroids_Xbox.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Types
{
    /// <summary>
    /// THIS CLASS SUCKS.
    /// </summary>
    abstract class AnimatedEntity : Entity
    {
        public Animation Animation;

        protected GraphicsDevice GraphicsDevice;

        /// <summary>
        /// Wrap movement around the screen (ie. instead of item moving off screen, it wraps back to the other end)
        /// </summary>
        public bool WrapScreen { get; set; }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }

        /// <summary>
        /// Frame width of the animated texture
        /// </summary>
        public int Width
        {
            get { return Animation.FrameWidth; }
        }

        /// <summary>
        /// Frame height of the animated texture
        /// </summary>
        public int Height
        {
            get { return Animation.FrameHeight; }
        }

        public abstract void Load(ContentManager content);

        public override void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;

            WrapScreen = false;

            Animation = new Animation();
            Load(content);
        }

        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            Animation.Position = Position;
            Animation.Update(gameTime);

            Move(CurrentSpeed.X, CurrentSpeed.Y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch);
        }

        public override void Move(float x, float y)
        {
            base.Move(x, y);

            if (WrapScreen)
            {
                // Make sure that the unit does not go out of bounds, but instead wraps across the screen
                if (Position.X <= 0.0f)
                {
                    var width = GraphicsDevice.Viewport.Width;
                    Position = new Vector2(width - 1, Position.Y);
                }

                if (Position.Y <= 0.0f)
                {
                    var height = GraphicsDevice.Viewport.Height;
                    Position = new Vector2(Position.X, height - 1);
                }

                Position = new Vector2
                (
                    Position.X % GraphicsDevice.Viewport.Width,
                    Position.Y % GraphicsDevice.Viewport.Height
                );
            }
        }

        public override void Rotate(float angle)
        {
            base.Rotate(angle);
            Animation.Rotation = Rotation;
        }
    }
}
