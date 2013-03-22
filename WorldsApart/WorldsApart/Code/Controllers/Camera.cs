﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using WorldsApart.Code.Graphics;

namespace WorldsApart.Code.Controllers
{
    class Camera
    {
        public Vector2 position = Vector2.Zero;
        public Vector2 targetPosition = Vector2.Zero;

        public Matrix transform = Matrix.Identity;

        public Sprite target;
        //public Sprite secondaryTarget;
        public List<Sprite> targetList = new List<Sprite>();
        public float innerDistance = 600;
        public float midDistance = 600;
        public float farDistance = 1000;

        public Vector2 offset = Vector2.Zero;

        public static float scaleConstant = .6f;
        public float scaleValue = scaleConstant;
        public float targetScale = scaleConstant;

        public float shiftRate = 8;

        public float parallaxRatio = 1f;

        public Camera(Sprite target, Sprite secondaryTarget, Vector2 offset)
        {
            this.target = target;
            AddTarget(secondaryTarget);
            this.offset = offset;
        }

        public void AddTarget(Sprite t)
        {
            targetList.Add(t);
        }

        public void Update()
        {
            Sprite secondaryTarget = targetList[0];
            float distance = Vector2.Distance(target.position, secondaryTarget.position);

            foreach (Sprite tar in targetList)
            {
                float newDist = Vector2.Distance(target.position, tar.position);
                if (newDist < distance)
                {
                    distance = newDist;
                    secondaryTarget = tar;
                }
            }

            
            float halfDistance = distance / 2;
            if (distance > 0 && distance < innerDistance)
            {
                float ratio = distance / innerDistance;
                targetPosition = target.position + Mathness.VectorToTarget(target.position, secondaryTarget.position, (halfDistance) * ratio);
                targetScale = scaleConstant - (scaleConstant * .2f) * ratio;
            }
            //else if (distance >= innerDistance && distance < farDistance)
            //{
            //    //float ratio = (distance - innerDistance) / (farDistance - innerDistance);
            //    //targetPosition = target.position + Mathness.VectorToTarget(target.position, secondaryTarget.position, halfDistance - halfDistance * ratio);
            //    //scaleValue = .8f + .2f * ratio;
            //    targetPosition = target.sPosition;
            //    targetScale = 1;
            //    shiftRate = 8;
            //}
            else
            {
                targetPosition = target.sPosition + offset;
                targetScale = scaleConstant;
            }

            position += (targetPosition - position) / shiftRate;
            scaleValue += (targetScale - scaleValue) / shiftRate;


            UpdateMatrixValues();

            

        }

        public void UpdateMatrixValues()
        {
            //scaleValue = 1 - ((1 - scaleValue) * parallaxRatio);
            Matrix scale = Matrix.CreateScale(scaleValue, scaleValue, 1);
            Matrix translate = Matrix.CreateTranslation(-position.X * 2 * parallaxRatio, -position.Y * 2 * parallaxRatio, 0);
            Matrix originTranslate = Matrix.CreateTranslation(Game1.screenWidth / 2, Game1.screenHeight / 2, 0);
            Matrix reverseTranslate = Matrix.CreateTranslation(-Game1.screenWidth / 2, -Game1.screenHeight / 2, 0);

            transform = translate * scale * originTranslate;
        }
    }
}
