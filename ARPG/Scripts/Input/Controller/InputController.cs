using ARPG.Scripts.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public static class InputController
    {
        private static readonly KeyboardInput keyboard = new();
        private static readonly MouseInput mouse = new();

        public static void GetInput()
        {
            keyboard.SetKeyboardStates();
            mouse.SetMouseStates();
        }
    }
}
