using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public IRotatable Rotatable { get; private set; }

        private float repetitions;
        private float amplitude;

        private float startingValue;
        private float target;

        private bool callSafetyNet;


        #region Normal transition
        public RotationTransition(float duration, IRotatable affected, float target, TransitionType type, RunOnDisable run, bool callSaftey = true)
        {
            Rotatable = affected;
            Duration = duration;
            startingValue = Rotatable.Rotation;

            this.target = target;
            transitionType = type;
            callSafetyNet = callSaftey;
            CallOnDisable = run;
        }
        #endregion

        #region Cross fade transition
        public RotationTransition(float duration, IRotatable affected, float target, TransitionType start, TransitionType end, RunOnDisable run, bool callSaftey = true)
        {
            Rotatable = affected;
            Duration = duration;
            startingValue = Rotatable.Rotation;

            this.target = target;
            transitionStart = start;
            transitionEnd = end;
            callSafetyNet = callSaftey;
            CallOnDisable = run;
        }
        #endregion

        #region Sin Curve
        public RotationTransition(float duration, IRotatable affected, float repetitions, float amplitude, RunOnDisable run)
        {
            Rotatable = affected;
            Duration = duration;
            startingValue = Rotatable.Rotation;

            transitionType = TransitionType.SinCurve;
            this.repetitions = repetitions;
            this.amplitude = amplitude;
            CallOnDisable = run;

            CallOnDisable += ResetRotation;
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float t = 0;

            if (Rotatable != null)
            {
                if (transitionType != null)
                {
                    #region Transitions 
                    switch (transitionType)
                    {
                        case TransitionType.SmoothStart2:
                            t = TransitionSystem.SmoothStart2(timer / Duration);
                            break;
                        case TransitionType.SmoothStart3:
                            t = TransitionSystem.SmoothStart3(timer / Duration);
                            break;
                        case TransitionType.SmoothStart4:
                            t = TransitionSystem.SmoothStart4(timer / Duration);
                            break;
                        case TransitionType.SmoothStop2:
                            t = TransitionSystem.SmoothStop2(timer / Duration);
                            break;
                        case TransitionType.SmoothStop3:
                            t = TransitionSystem.SmoothStop3(timer / Duration);
                            break;
                        case TransitionType.SmoothStop4:
                            t = TransitionSystem.SmoothStop4(timer / Duration);
                            break;
                        case TransitionType.SinCurve:
                            t = TransitionSystem.SinCurve(repetitions, amplitude, timer / Duration);
                            break;
                        default:
                            break;
                    }
                    #endregion
                }
                else
                {
                    float t2 = 0;

                    #region Cross fade transitions
                    switch (transitionStart)
                    {
                        case TransitionType.SmoothStart2:
                            t = TransitionSystem.SmoothStop2(timer / Duration);
                            break;
                        case TransitionType.SmoothStart3:
                            t = TransitionSystem.SmoothStop3(timer / Duration);
                            break;
                        case TransitionType.SmoothStart4:
                            t = TransitionSystem.SmoothStop4(timer / Duration);
                            break;
                        default:
                            break;
                    }

                    switch (transitionEnd)
                    {
                        case TransitionType.SmoothStop2:
                            t2 = TransitionSystem.SmoothStop2(timer / Duration);
                            break;
                        case TransitionType.SmoothStop3:
                            t2 = TransitionSystem.SmoothStop3(timer / Duration);
                            break;
                        case TransitionType.SmoothStop4:
                            t2 = TransitionSystem.SmoothStop4(timer / Duration);
                            break;
                        default:
                            break;
                    }
                    #endregion

                    t = TransitionSystem.Crossfade(t, t2, timer / Duration);
                }
            }

            if (transitionType == TransitionType.SinCurve)
            {
                Rotatable.SetRotation(t + startingValue);
            }
            else
            {
                Rotatable.SetRotation(MathHelper.Lerp(startingValue, target + startingValue, t));
            }
        }

        private void ResetRotation()
        {
            Rotatable.SetRotation(0);
        }

        public override void SafetyNet()
        {
            if (transitionType != TransitionType.SinCurve && callSafetyNet)
            {
                Rotatable.SetRotation(target + startingValue);
            }
        }
    }
}
