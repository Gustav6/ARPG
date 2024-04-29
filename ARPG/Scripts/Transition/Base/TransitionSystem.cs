using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                if (transitions[i].owner == null)
                {
                    transitions.RemoveAt(i);
                }
                else if (transitions[i].isRemoved)
                {
                    transitions[i].CallOnDisable();
                    transitions.RemoveAt(i);
                }
            }
        }

        #region Rotation Transitions
        public static void RotationTransition(float duration, GameObject affected, float target, TransitionType type)
        {
            transitions.Add(new RotationTransition(duration, affected, target, type));
        }
        public static void CrossFadeRotationTransition(float duration, GameObject affected, float target, TransitionType start, TransitionType end)
        {
            transitions.Add(new RotationTransition(duration, affected, target, start, end));
        }

        public static void SINTransition(float duration, GameObject affected, float target, float repetitions, float amplitude)
        {
            transitions.Add(new RotationTransition(duration, affected, target, TransitionType.SinCurve, repetitions, amplitude));
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
