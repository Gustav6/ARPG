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
        private static List<Transition> transitions = new();

        public static void Update(GameTime gameTime)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                try
                {
                    transitions[i].Update(gameTime);
                }
                catch (NullReferenceException)
                {
                    transitions[i].RemoveTransition();

                    throw;
                }
            }

            for (int i = transitions.Count - 1; i >= 0; i--)
            {
                try
                {
                    if (transitions[i].IsRemoved)
                    {
                        transitions[i].CallOnDisable?.Invoke();
                        transitions[i].SafetyNet();
                        transitions.RemoveAt(i);
                    }
                }
                catch (NullReferenceException)
                {
                    transitions.RemoveAt(i);

                    throw;
                }
            }
        }

        #region Rotation Transitions
        public static void RotationTransition(float duration, IRotatable affected, float target, TransitionType type, RunOnDisable callOnDisable = null)
        {
            transitions.Add(new RotationTransition(duration, affected, target, type, callOnDisable));
        }
        public static void CrossFadeRotationTransition(float duration, IRotatable affected, float target, TransitionType start, TransitionType end, RunOnDisable callOnDisable = null)
        {
            transitions.Add(new RotationTransition(duration, affected, target, start, end, callOnDisable));
        }

        public static void SINTransition(float duration, IRotatable affected, float repetitions, float amplitude, RunOnDisable callOnDisable = null)
        {
            transitions.Add(new RotationTransition(duration, affected, repetitions, amplitude, callOnDisable));
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
        public static float SinCurve(float repetitions,float amplitude, float t)
        {
            //return MathF.Sin(t * MathF.PI * repetitions) * amplitude;
            return MathF.Sin(t * MathF.PI * repetitions) * amplitude;
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
