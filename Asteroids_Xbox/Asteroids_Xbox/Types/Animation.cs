using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Asteroids_Xbox.Types;

namespace Asteroids_Xbox.Types
{
    class Animation : Drawable
    {
        /// <summary>
        /// The image representing the collection of images used for animation
        /// </summary>
        private Texture2D spriteStrip;

        /// <summary>
        /// The scale used to display the sprite strip
        /// </summary>
        private float scale;

        /// <summary>
        /// The time since we last updated the frame
        /// </summary>
        private int elapsedTime;

        /// <summary>
        /// The time we display a frame until the next one
        /// </summary>
        private int frameTime;

        /// <summary>
        /// The number of frames that the animation contains
        /// </summary>
        private int frameCount;

        /// <summary>
        /// The index of the current frame we are displaying
        /// </summary>
        private int currentFrame;

        /// <summary>
        /// The color of the frame we will be displaying
        /// </summary>
        private Color color;

        /// <summary>
        /// The area of the image strip we want to display
        /// </summary>
        private Rectangle sourceRect = new Rectangle();

        /// <summary>
        /// The area where we want to display the image strip in the game
        /// </summary>
        private Rectangle destinationRect = new Rectangle();

        /// <summary>
        /// Width of a given frame
        /// </summary>
        public int FrameWidth;

        /// <summary>
        /// Height of a given frame
        /// </summary>
        public int FrameHeight;

        /// <summary>
        /// The state of the Animation
        /// </summary>
        public bool ShouldDraw;

        /// <summary>
        /// Determines if the animation will keep playing or deactivate after one run
        /// </summary>
        public bool Looping;

        public Vector2 Position { get; set; }

        public float Rotation { get; set; }

        public float LayerDepth;

        private bool initialized;

        public Animation()
        {
            initialized = false;
            ShouldDraw = false;
        }

        public void Initialize(Texture2D texture, Vector2 position,
            int frameWidth, int frameHeight, int frameCount,
            int frametime, Color color, float scale, bool looping)
        {
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frametime;
            this.scale = scale;

            Looping = looping;
            Position = position;
            spriteStrip = texture;

            elapsedTime = 0;
            currentFrame = 0;
            LayerDepth = 0.0f;

            ShouldDraw = true;
            initialized = true;
        }

        public void Update(GameTime gameTime)
        {
            // Do not update the game if we are not active
            if (!ShouldDraw)
            {
                return;
            }
            else if (!initialized)
            {
                throw new AnimationNotInitializedException("Animation must be initialized prior to updating");
            }

            // Update the elapsed time
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            // If the elapsed time is larger than the frame time
            // we need to switch frames
            if (elapsedTime > frameTime)
            {
                // Move to the next frame
                currentFrame++;

                // If the currentFrame is equal to frameCount reset currentFrame to zero
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                    // If we are not looping deactivate the animation
                    if (Looping == false)
                        ShouldDraw = false;
                }

                // Reset the elapsed time to zero
                elapsedTime = 0;
            }

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            destinationRect = new Rectangle
            (
                (int)Position.X - (int)(FrameWidth * scale) / 2,
                (int)Position.Y - (int)(FrameHeight * scale) / 2,
                (int)(FrameWidth * scale),
                (int)(FrameHeight * scale)
            );
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (ShouldDraw)
            {
                if (!initialized)
                {
                    throw new AnimationNotInitializedException("Animation must be initialized prior to drawing");
                }

                var radians = MathHelper.ToRadians(Rotation);
                var center = new Vector2(((float)FrameWidth) / 2, ((float)FrameHeight) / 2);

                spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color,
                    radians, center, SpriteEffects.None, LayerDepth);
            }
        }
    }

    public class AnimationNotInitializedException : Exception
    {
        public AnimationNotInitializedException() { }
        public AnimationNotInitializedException(string message) : base(message) { }
        public AnimationNotInitializedException(string message, Exception inner) : base(message, inner) { }
    }
}
