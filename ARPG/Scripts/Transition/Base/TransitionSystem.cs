using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ARPG.Transition;

namespace ARPG
{
    public static class TransitionSystem
    {
        public static List<Transition> transitions = new();

        public static void Update(GameTime gameTime)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                transitions[i].Update(gameTime);
            }

            for (int i = transitions.Count - 1; i >= 0; i--)
            {
                if (transitions[i].isRemoved || transitions[i].owner == null)
                {
                    transitions[i].CallOnDisable?.Invoke();
                    transitions[i].SafteyNet();
                    transitions.RemoveAt(i);
                }
            }
        }

        #region Rotation Transitions
        public static void RotationTransition(float duration, GameObject affected, float target, TransitionType type, RunOnDisable callOnDisable = null)
        {
            transitions.Add(new RotationTransition(duration, affected, target, type, callOnDisable));
        }
        public static void CrossFadeRotationTransition(float duration, GameObject affected, float target, TransitionType start, TransitionType end, RunOnDisable callOnDisable = null)
        {
            transitions.Add(new RotationTransition(duration, affected, target, start, end, callOnDisable));
        }

        public static void SINTransition(float duration, GameObject affected, float target, float repetitions, float amplitude, bool returnToOriginl = false, RunOnDisable callOnDisable = null)
        {
            transitions.Add(new RotationTransition(duration, affected, target, repetitions, amplitude, returnToOriginl, callOnDisable));
        }
        #endregion

        public static float SmoothStart2(float t)
        {
            return t * t;
        }

        public static float SmoothStart3(float t)
        {
            return t * t * t;
        }

        public static float SmoothStart4(float t)
        {
            return t * t * t * t;
        }

        public static float SmoothStop2(float t)
        {
            return 1 - (1 - t) * (1 - t);
        }

        public static float SmoothStop3(float t)
        {
            return 1 - (1 - t) * (1 - t) * (1 - t);
        }

        public static float SmoothStop4(float t)
        {
            return 1 - (1 - t) * (1 - t) * (1 - t) * (1 - t);
        }
        public static float SinCurve(float amplitude, float t)
        {
            return MathF.Sin(t * MathF.PI * (MathF.PI / 2)) * amplitude;
        }

        public static float Crossfade(float transitionStart, float transitionEnd, float t)
        {
            return (1 - t) * transitionStart + t * transitionEnd;
        }
    }

    public enum TransitionType
    {
        SmoothStart2,
        SmoothStart3,
        SmoothStart4,

        SmoothStop2,
        SmoothStop3,
        SmoothStop4,

        SinCurve,
    }
}
