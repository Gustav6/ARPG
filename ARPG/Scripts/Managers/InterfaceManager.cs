using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public interface IDamageable
    {
        public void ApplyDamage(float damageAmount);
        public void ApplyKnockback(float knockbackStrength, Vector2 projectileDirection);
    }

    public interface ICollidable
    {
        public void OnCollision(ICollidable source);
        public Rectangle BoundingBox { get; set; }
    }

    public interface IRotatable
    {
        public void SetRotation(float newRotation);
        public float Rotation { get; set; }
    }
}
