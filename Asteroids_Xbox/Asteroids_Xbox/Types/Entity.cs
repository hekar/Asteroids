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
    /// <summary>
    /// Piez of shit class is a total piez of shit
    /// 
    /// Should had used delegation, but too late. Now dealing with dis piez of shit inheritence shit.
    /// 
    /// C# I PUNCH U IN TEH FAcE
    /// 
    /// PIECE OF SHIT, I HATE THIS CLASS
    /// 
    /// F!#$^$&$@%$ %*&*$^#@%! $!%@^&#%!@ S#@!$
    /// </summary>
    abstract class Entity : Initializable, Updatable, Drawable
    {
        /// <summary>
        /// Has the entity been initialized and had its content loaded?
        /// </summary>
        public bool Initialized { get; protected set; }

        /// <summary>
        /// Rotation in degrees
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Rotation in radians
        /// </summary>
        public float Radians { get { return MathHelper.ToRadians(Rotation); } }

        /// <summary>
        /// Position of the unit
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Current speed of the unit
        /// </summary>
        protected Vector2 CurrentSpeed { get; set; }

        /// <summary>
        /// Maximum speed of the unit
        /// </summary>
        protected float MaxSpeed { get; set; }

        /// <summary>
        /// Speed at which the unit accelerates
        /// </summary>
        protected float MoveSpeed { get; set; }

        /// <summary>
        /// Speed at which the unit rotates
        /// </summary>
        protected float RotationSpeed { get; set; }

        public Entity()
        {
            Rotation = 0;
            Position = Vector2.Zero;

            CurrentSpeed = Vector2.Zero;
            MaxSpeed = 0.0f;
            MoveSpeed = 0.0f;
            RotationSpeed = 0.0f;
        }

        public virtual void Move(float x, float y)
        {
            this.Position = new Vector2(this.Position.X + x, this.Position.Y + y);
        }

        public void Forward()
        {
            Forward(MoveSpeed, MoveSpeed);
        }

        public void Backward()
        {
            Forward(-MoveSpeed, -MoveSpeed);
        }

        protected void EntityDeath()
        {

        }

        protected void Forward(float speedModX, float speedModY)
        {
            double rad = MathHelper.ToRadians(Rotation);

            float speedX = (float)Math.Cos(rad) * speedModX;
            float speedY = (float)Math.Sin(rad) * speedModY;

            var nextSpeedX = MathHelper.Clamp(CurrentSpeed.X + speedX, -MaxSpeed, MaxSpeed);
            var nextSpeedY = MathHelper.Clamp(CurrentSpeed.Y + speedY, -MaxSpeed, MaxSpeed);

            CurrentSpeed = new Vector2(nextSpeedX, nextSpeedY);
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

        /// <summary>
        /// Load content and resources
        /// </summary>
        /// <param name="content"></param>
        /// <param name="graphicsDevice"></param>
        public abstract void Initialize(ContentManager content, GraphicsDevice graphicsDevice);

        /// <summary>
        /// Update on each game loop
        /// </summary>
        /// <param name="inputManager"></param>
        /// <param name="gameTime"></param>
        public abstract void Update(InputManager inputManager, GameTime gameTime);

        /// <summary>
        /// Redraw unit
        /// </summary>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
