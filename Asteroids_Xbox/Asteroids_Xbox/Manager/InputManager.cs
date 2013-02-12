using Microsoft.Xna.Framework.Input;

namespace Asteroids_Xbox.Manager
{
    class InputManager
    {


        public KeyboardState CurrentKeyboardState { get; set; }
        public KeyboardState PreviousKeyboardState { get; set; }

        public GamePadState CurrentGamePadState { get; set; }
        public GamePadState PreviousGamePadState { get; set; }
    }
}
