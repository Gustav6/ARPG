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

        private float repetitions;
        private float amplitude;

        private float startingValue;
        private float target;

        private bool callSafteyNet;


        #region Normal transition
        public RotationTransition(float duration, GameObject affected, float target, TransitionType type, RunOnDisable run, bool callSaftey = true)
        {
            Affected = affected;
            Duration = duration;
            this.target = target;
            startingValue = Affected.rotation;
            transitionType = type;
            callSafteyNet = callSaftey;
            CallOnDisable = run;
        }
        #endregion

        #region Cross fade transition
        public RotationTransition(float duration, GameObject affected, float target, TransitionType start, TransitionType end, RunOnDisable run, bool callSaftey = true)
        {
            Affected = affected;
            Duration = duration;
            this.target = target;
            startingValue = Affected.rotation;
            transitionStart = start;
            transitionEnd = end;
            callSafteyNet = callSaftey;
            CallOnDisable = run;
        }
        #endregion

        #region Sin Curve
        public RotationTransition(float duration, GameObject affected, float target, float repetitions, float amplitude, bool returnToStart, RunOnDisable run)
        {
            Affected = affected;
            Duration = duration;
            this.target = target;
            startingValue = Affected.rotation;
            transitionType = TransitionType.SinCurve;
            this.repetitions = repetitions;
            this.amplitude = amplitude;
            CallOnDisable = run;

            if (returnToStart)
            {
                CallOnDisable += ReturnToOriginalRotation;
            }

            if (Library.cameraInstance != null && affected == Library.cameraInstance)
            {

            }
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            float t = 0;

            if (Affected != null)
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
                            t = TransitionSystem.SinCurve(amplitude, timer / Duration);
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

                Affected.rotation = MathHelper.Lerp(startingValue, target + startingValue, t);
            }

            base.Update(gameTime);
        }

        private void ReturnToOriginalRotation()
        {
            TransitionSystem.transitions.Add(new RotationTransition(Duration, Library.cameraInstance, startingValue + target, TransitionType.SmoothStop2, ResetRotation, false));
        }

        private void ResetRotation()
        {
            Affected.rotation = 0;
        }

        public override void SafteyNet()
        {
            if (transitionType != TransitionType.SinCurve && callSafteyNet)
            {
                Affected.rotation = target + startingValue;
            }
        }
    }
}
