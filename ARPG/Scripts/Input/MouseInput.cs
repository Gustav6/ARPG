using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG.Scripts.Input
{
    public class MouseInput
    {
        public static MouseState currentState;
        public static MouseState prevState;

        public void SetMouseStates()
        {
            prevState = currentState;
            currentState = Mouse.GetState();
        }

        public static Rectangle GetBounds(bool useCurrentState)
        {
            if (useCurrentState)
                return new Rectangle(currentState.X, currentState.Y, 1, 1);
            else
                return new Rectangle(prevState.X, prevState.Y, 1, 1);
        }

        public static bool IsPressed(ButtonState buttonState)
        {
            if (buttonState == ButtonState.Pressed)
                return true;

            return false;
        }

        public static bool HasBeenPressed(ButtonState _currentState, ButtonState _prevState)
        {
            if (_currentState == ButtonState.Pressed && _prevState == ButtonState.Released)
                return true;

            return false;
        }
    }
}
