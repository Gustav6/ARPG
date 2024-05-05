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
        public GameObject Affected { get; protected set; }

        public delegate void RunOnDisable();
        public RunOnDisable CallOnDisable { get; protected set; }
        public float Duration { get; protected set; }

        public bool isRemoved;
        protected bool canRemove;

        public float timer;


        public virtual void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer > Duration)
            {
                isRemoved = true;
            }
        }

        public virtual void SafetyNet()
        {

        }
    }
}
