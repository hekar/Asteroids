using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Asteroids_Xbox.Entities;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Types
{
    interface Initializable
    {
        void Initialize(ContentManager content, GraphicsDevice graphicsDevice);
    }
}
