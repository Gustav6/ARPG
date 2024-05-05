﻿using Microsoft.Xna.Framework;
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

        private static readonly Dictionary<KeyList, List<Keys>> keyPairs = new()
        {
            { KeyList.Left, new List<Keys>() { Keys.A, Keys.Left } },
            { KeyList.Right, new List<Keys>() { Keys.D, Keys.Right } },
            { KeyList.Up, new List<Keys>() { Keys.W, Keys.Up } },
            { KeyList.Down, new List<Keys>() { Keys.S, Keys.Down } },
        };

        #endregion

        public static void SetKeyboardStates()
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

        #region Input for category  
        public static int Horizontal()
        {
            int tempXDirection = 0;

            for (int i = 0; i < keyPairs[KeyList.Left].Count; i++)
            {
                if (IsPressed(keyPairs[KeyList.Left][i]))
                {
                    tempXDirection = -1;
                    break;
                }
            }

            for (int i = 0; i < keyPairs[KeyList.Right].Count; i++)
            {
                if (IsPressed(keyPairs[KeyList.Right][i]))
                {
                    if (tempXDirection != 0)
                    {
                        tempXDirection = 0;
                        break;
                    }

                    tempXDirection = 1;
                    break;
                }
            }

            return tempXDirection;
        }

        public static int Vertical()
        {
            int tempYDirection = 0;

            for (int i = 0; i < keyPairs[KeyList.Up].Count; i++)
            {
                if (IsPressed(keyPairs[KeyList.Up][i]))
                {
                    tempYDirection = -1;
                    break;
                }
            }

            for (int i = 0; i < keyPairs[KeyList.Down].Count; i++)
            {
                if (IsPressed(keyPairs[KeyList.Down][i]))
                {
                    if (tempYDirection != 0)
                    {
                        tempYDirection = 0;
                        break;
                    }

                    tempYDirection = 1;
                    break;
                }
            }

            return tempYDirection;
        }
        #endregion

        private enum KeyList
        {
            Left,
            Right, 
            Up, 
            Down
        }
    }
}
