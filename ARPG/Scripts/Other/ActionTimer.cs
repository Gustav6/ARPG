using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class ActionTimer
    {
        public delegate void TimerCallbackDelegate();
        private TimerCallbackDelegate timerCallback;

        private float timer;

        public void SetTimer(float duration, TimerCallbackDelegate callBack)
        {
            timer = duration;

            timerCallback = callBack;

            Library.timers.Add(this);
        }

        public void Update(GameTime gameTime)
        {
            if (timer <= 0)
            {
                timerCallback?.Invoke();
                Library.timers.Remove(this);
            }
            else
            {
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
