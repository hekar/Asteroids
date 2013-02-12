using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Asteroids_Xbox.Manager;

namespace Asteroids_Xbox.Types
{
    interface Updatable
    {
        void Update(InputManager inputManager, GameTime gameTime);
    }
}
