using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG.Scripts.Managers
{
    public class CollisionManager
    {
        public static void Update()
        {
            foreach (GameObject g1 in Library.gameObjects)
            {
                if (g1 is ICollidable c1)
                {
                    foreach (GameObject g2 in Library.gameObjects)
                    {
                        if (g2 is ICollidable c2)
                        {
                            if (g1 != g2)
                            {
                                if (c1.BoundingBox.Intersects(c2.BoundingBox))
                                {
                                    c1.OnCollision(c2);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
