using Asteroids_Xbox.Manager;
using Microsoft.Xna.Framework;

namespace Asteroids_Xbox.Types
{
    /// <summary>
    /// An interface for objects that accept updates from the game loop
    /// </summary>
    interface Updatable
    {
        void Update(InputManager inputManager, GameTime gameTime);
    }
}
