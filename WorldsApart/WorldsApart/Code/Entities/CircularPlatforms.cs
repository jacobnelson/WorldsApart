using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldsApart.Code.Controllers;

using System.Diagnostics;

namespace WorldsApart.Code.Entities
{
    class CircularPlatform : EventObject
    {
        public PlatformMode platformMode = PlatformMode.PressToReverse;
        public float duration = 120;
        public float increment = 0;
        public float angle = 0;
        public float startAngle = 0;
        public float radius = 100;
        public float durationCounter = 0;
        public Vector2 centerPosition = Vector2.Zero;

        public bool moving = true;

        Vector2 pPos = Vector2.Zero; 

        public CircularPlatform(Texture2D texture, Vector2 centerPosition, float radius)
            : base(texture, Vector2.Zero)
        {
            this.centerPosition = centerPosition;
            this.radius = radius;
            isSolidEnvironment = false;
            UpdateIncrement(duration);
        }

        public void UpdateIncrement(float duration)
        {
            this.duration = duration;
            increment = 2 * (float)Math.PI / duration;
        }

        

        public override void Update()
        {
            base.Update();

            
        }

        public override void GetMovement()
        {
            base.GetMovement();
            if (moving)
            {
                angle += increment;


                durationCounter++;
                if (durationCounter >= duration)
                {
                    durationCounter = 0;
                    angle = startAngle;
                }
                
            }
            SetPosition();
        }

        public void SetPosition()
        {
            float posX = radius * (float)Math.Cos(angle) + centerPosition.X;
            float posY = radius * (float)Math.Sin(angle) + centerPosition.Y;


            //float posX = (float)Math.Sin(angle) * radius * 2.5f;
            //float posY = (float)Math.Sin(angle * 2) * radius * 1;

            //position = new Vector2(posX, posY) + centerPosition;
            position = new Vector2(posX, posY);
            hitBox.SetPosition(position);
            speed = position - pPos;
            pPos = position;
        }

        public override void ActivateEvent(TriggerState triggerState)
        {
            base.ActivateEvent(triggerState);

            if (platformMode == PlatformMode.PressToReverse)
            {
                increment = -increment;
            }
            else if (platformMode == PlatformMode.PressToActivate)
            {
                if (triggerState == TriggerState.Triggered)
                {
                    moving = true;
                }
                else
                {
                    moving = false;
                }
            }
        }


    }
}
