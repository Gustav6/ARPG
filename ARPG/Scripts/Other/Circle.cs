using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG.Scripts.Other
{
    public class Circle
    {
        public Vector2 position;
        public float radius;

        public Circle(float x, float y, float radius)
        {
            position = new Vector2(x, y);
            this.radius = radius;
        }

        public bool Intersects(Circle other)
        {
            Vector2 distance = other.position - position;

            if (distance.LengthSquared() <= (radius + other.radius) * (radius + other.radius))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
