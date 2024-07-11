using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public abstract class Transition
    {
        public delegate void RunOnDisable();
        public RunOnDisable CallOnDisable { get; protected set; }
        public float Duration { get; protected set; }
        public bool IsRemoved { get; private set; }

        protected bool canRemove;

        public float timer;


        public virtual void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= Duration)
            {
                IsRemoved = true;
            }
        }

        public void RemoveTransition()
        {
            IsRemoved = true;
        }

        public virtual void SafetyNet()
        {

        }
    }
}
