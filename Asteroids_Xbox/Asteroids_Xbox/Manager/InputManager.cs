using Microsoft.Xna.Framework.Input;

namespace Asteroids_Xbox.Manager
{
    class InputManager
    {
        public KeyboardState CurrentKeyboardState { get; set; }
        public KeyboardState PreviousKeyboardState { get; set; }

        public GamePadState CurrentGamePadState { get; set; }
        public GamePadState PreviousGamePadState { get; set; }

        /// <summary>
        /// Was a keyboard key pressed then released
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool WasKeyPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Was a gamepad button pressed then released
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool WasButtonPressed(Buttons button)
        {
            return CurrentGamePadState.IsButtonDown(button) && PreviousGamePadState.IsButtonUp(button);
        }
    }
}
