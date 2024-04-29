﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class Camera
    {
        #region Default variables
        public Matrix Transform { get; private set; }
        private Viewport viewport;
        private Vector2 center;

        private float zoom = 1;
        private float rotation = 0;

        private float maximumZoom = 3;
        private float minimumZoom = 0.1f;
        private const double rotationConstFor360 = Math.PI * 2;
        #endregion

        public float X { get { return center.X; } }
        public float Y { get { return center.Y; } }

        public GameObject target;

        public float Zoom
        {
            get { return zoom; }

            set
            {
                #region Saftey net
                zoom = value;

                // Cap maximum and minimum zoom
                if (zoom < minimumZoom)
                {
                    zoom = minimumZoom;
                }
                else if (zoom > maximumZoom)
                {
                    zoom = maximumZoom;
                }
                #endregion
            }
        }

        public float Rotation
        {
            get { return rotation; }

            set
            {
                #region Saftey net
                rotation = value;

                // Set rotation to 0 when camera has rotated 360°
                if (rotation > rotationConstFor360 || rotation < -rotationConstFor360)
                {
                    rotation = 0;
                }
                #endregion
            }
        }

        public Camera(Viewport _viewport, GameObject _target)
        {
            viewport = _viewport;
            target = _target;
        }

        public void Update()
        {
            if (target != null)
            {
                center = new Vector2(target.Position.X, target.Position.Y);
            }

            Transform =
                Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 0)) * 
                Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
        }

        public void ScreenShake(float duration, float intensity)
        {

        }
    }
}
