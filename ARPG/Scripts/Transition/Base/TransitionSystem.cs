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
                if (transitions[i].isRemoved)
                {
                    transitions[i].CallOnDisable();
                    transitions.RemoveAt(i);
                }
            }
        }

        public static void NewRotationTransition(float duration, GameObject affected, float target, TransitionType type)
        {
            RotationTransition transition = new(duration, affected, target, type, repetitions, amplitude);

            transitions.Add(transition);
        }

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
        public static float SinCurve(float repetitions, float amplitude, float t)
        {
            return MathF.Sin(t * MathF.PI * repetitions) * amplitude * -1;
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
