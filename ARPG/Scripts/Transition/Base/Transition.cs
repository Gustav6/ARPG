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
        public GameObject owner;

        public delegate void RunOnDisable();
        public RunOnDisable CallOnDisable { get; protected set; }

        public bool isRemoved;

        public float timer;
        public float duration;


        public virtual void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer > duration)
            {
                isRemoved = true;
            }
        }

        public virtual void SafteyNet()
        {

        }
    }
}
