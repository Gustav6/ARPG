using ARPG.Scripts.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public static class InputManager
    {
        public static bool ControllerActive { get; private set; }

        public static void GetInput()
        {
            KeyboardInput.SetKeyboardStates();
            MouseInput.SetMouseStates();
        }
    }
}
