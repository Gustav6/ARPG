using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.TimeZoneInfo;

namespace ARPG
{
    public class RotationTransition : Transition
    {
        private readonly TransitionType? transitionType;
        private readonly TransitionType transitionStart;
        private readonly TransitionType transitionEnd;
        private GameObject affected; 

        private float repetitions;
        private float amplitude;

        private float startingValue;
        private float target;

        #region Normal transition
        public RotationTransition(float duration, GameObject affected, float target, TransitionType type)
        {
            this.affected = affected;
            this.duration = duration;
            this.target = target;
            owner = affected;
            startingValue = owner.rotation;
            transitionType = type;
        }
        #endregion

        #region Cross fade transition
        public RotationTransition(float duration, GameObject affected, float target, TransitionType start, TransitionType end)
        {
            this.affected = affected;
            this.duration = duration;
            this.target = target;
            owner = affected;
            startingValue = owner.rotation;
            transitionStart = start;
            transitionEnd = end;
        }
        #endregion

        #region Sin Curve
        public RotationTransition(float duration, GameObject affected, float target, TransitionType type, float repetitions = 0, float amplitude = 0)
        {
            this.affected = affected;
            this.duration = duration;
            this.target = target;
            owner = affected;
            startingValue = owner.rotation;
            transitionType = type;
            this.repetitions = repetitions;
            this.amplitude = amplitude;
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            float t = 0;

            if (transitionType != null)
            {
                switch (transitionType)
                {
                    case TransitionType.SmoothStart2:
                        t = TransitionSystem.SmoothStart2(timer / duration);
                        break;
                    case TransitionType.SmoothStart3:
                        t = TransitionSystem.SmoothStart3(timer / duration);
                        break;
                    case TransitionType.SmoothStart4:
                        t = TransitionSystem.SmoothStart4(timer / duration);
                        break;
                    case TransitionType.SmoothStop2:
                        t = TransitionSystem.SmoothStop2(timer / duration);
                        break;
                    case TransitionType.SmoothStop3:
                        t = TransitionSystem.SmoothStop3(timer / duration);
                        break;
                    case TransitionType.SmoothStop4:
                        t = TransitionSystem.SmoothStop4(timer / duration);
                        break;
                    case TransitionType.SinCurve:
                        t = TransitionSystem.SinCurve(repetitions, amplitude, timer / duration);
                        break;
                    default:
                        break;
                }

                affected.rotation = (target * t) + startingValue;
            }

            base.Update(gameTime);
        }

        public override void CallOnDisable()
        {
            affected.rotation = target;

            base.CallOnDisable();
        }
    }
}
