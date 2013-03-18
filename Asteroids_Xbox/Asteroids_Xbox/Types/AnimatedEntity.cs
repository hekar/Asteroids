using Asteroids_Xbox.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Asteroids_Xbox.Entities;

namespace Asteroids_Xbox.Types
{
    /// <summary>
    /// THIS CLASS SUCKS.
    /// </summary>
    abstract class AnimatedEntity : Entity
    {
        public Animation Animation { get; set; }

        protected GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// Position of the unit
        /// </summary>
        public Vector2 CenterPosition
        {
            get
            {
                return new Vector2(Position.X - (Width / 2), Position.Y - (Height / 2));
            }
        }

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

        public float Radius
        {
            get
            {
                return (Bounds.Width > Bounds.Height) ? Bounds.Width / 2 : Bounds.Height / 2;
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

        /// <summary>
        /// Is the entity offscreen?
        /// </summary>
        public bool Offscreen
        {
            get
            {
                var offscreen = !GraphicsDevice.Viewport.Bounds.Intersects(Bounds);
                return offscreen;
            }
        }

        public abstract void Load(ContentManager content);

        public override void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;

            WrapScreen = false;

            Animation = new Animation();
            Load(content);

            Initialized = true;
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

        public bool CheckCollision(AnimatedEntity other)
        {
            var distance = Math.Abs(Vector2.Distance(this.CenterPosition, other.CenterPosition));
            var inThisRadius = distance < this.Radius;
            var inOtherRadius = distance < other.Radius;
            if (inThisRadius || inOtherRadius)
            {
                // Perform pixel check now
                var area = (this.Bounds.Width * this.Bounds.Height);
                var otherArea = (other.Bounds.Width * other.Bounds.Height);

                var smaller = (area > otherArea) ? other : this;
                var larger = (area > otherArea) ? this : other;

                // TODO: later after i eat these muffins
                // and have some ice cream and watch
                // 50 HOURS OF KDRAMAS OMG, NEW SERIES FOUND GOMG AOSDMF ASDFJKSADJG ASDFOSDMAGOMSDAFKLASDHJFLKASDFKLHASDFAJLSDFHASJKDFHASJDKFHASDKLFHASKJDFHASJKDHKJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJJF
                //return PerPixelCollision(smaller, larger);\

                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual void Touch(AnimatedEntity other)
        {
        }

        static bool PerPixelCollision(AnimatedEntity e1, AnimatedEntity e2)
        {
            var at = e1.Animation.Transformations;
            var afw = e1.Animation.FrameWidth;
            var afh = e1.Animation.FrameHeight;
            var acd = e1.Animation.ColorData;
            var bt = e2.Animation.Transformations;
            var bfw = e2.Animation.FrameWidth;
            var bfh = e2.Animation.FrameHeight;
            var bcd = e2.Animation.ColorData;

            // Get Color data of each Texture
            Color[] bitsA = new Color[afw * afh];
            Color[] bitsB = new Color[bfw * bfh];

            // Calculate the intersecting rectangle
            int x1 = Math.Max(e1.Bounds.X, e2.Bounds.X);
            int x2 = Math.Min(e1.Bounds.X + e1.Bounds.Width, e2.Bounds.X + e2.Bounds.Width);

            int y1 = Math.Max(e1.Bounds.Y, e2.Bounds.Y);
            int y2 = Math.Min(e1.Bounds.Y + e1.Bounds.Height, e2.Bounds.Y + e2.Bounds.Height);

            // For each single pixel in the intersecting rectangle
            for (int y = y1; y < y2; ++y)
            {
                for (int x = x1; x < x2; ++x)
                {
                    // Get the color from each texture
                    Color a = bitsA[(x - e1.Bounds.X) + (y - e1.Bounds.Y) * afw];
                    Color b = bitsB[(x - e2.Bounds.X) + (y - e2.Bounds.Y) * bfw];

                    // If both colors are not transparent (the alpha channel is not 0), then there is a collision
                    if (a.A != 0 && b.A != 0)
                    {
                        return true;
                    }
                }
            }
            // If no collision occurred by now, we're clear.
            return false;
        }
    }
}
