using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class KeyboardInput
    {
        #region Variables
        private static KeyboardState currentState;
        private static KeyboardState prevState;
        #endregion

        public void SetKeyboardStates()
        {
            prevState = currentState;
            currentState = Keyboard.GetState();
        }

        public static bool IsPressed(Keys key)
        {
            return currentState.IsKeyDown(key);
        }

        public static bool HasBeenPressed(Keys key)
        {
            return currentState.IsKeyDown(key) && !prevState.IsKeyDown(key);
        }

    }
}
