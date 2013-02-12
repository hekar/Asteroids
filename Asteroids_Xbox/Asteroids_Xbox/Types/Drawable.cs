using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Asteroids_Xbox.Types
{
    interface Drawable
    {
        /// <summary>
        /// Rotation 0 - 359
        /// </summary>
        float Rotation { get; set; }

        /// <summary>
        /// Position of the Player relative to the upper left side of the screen
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// Draw the entity
        /// </summary>
        /// <param name="spriteBatch"></param>
        void Draw(SpriteBatch spriteBatch);
    }
}
