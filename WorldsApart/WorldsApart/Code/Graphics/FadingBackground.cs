using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using WorldsApart.Code.Controllers;

namespace WorldsApart.Code.Graphics
{
    enum FadeMode
    {
        InAndOut,
        RandomFlashes
    }

    class FadingBackground : AnimatedSprite
    {

        FadeMode fadeMode = FadeMode.InAndOut;

        public bool fadingBG = true;
        public int fadeRate = 60;
        public int fadeVariation = 0;

        public bool movingBG = false;
        public int duration = 0;
        public Vector2 startPosition = Vector2.Zero;
        public Vector2 endPosition = Vector2.Zero;
        public float moveSpeedVariation = 0;
        public Sprite followTarget;
        public Vector2 followOffset = Vector2.Zero;
        public Sprite exactTarget;
        public Vector2 exactOffset = Vector2.Zero;
        

        public FadingBackground(Texture2D texture, Vector2 position) : base(texture, position)
        {
        }

        public void SetMoving(Vector2 startPosition, Vector2 endPosition, float moveSpeed, float moveSpeedVariation)
        {
            movingBG = true;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.moveSpeedVariation = moveSpeedVariation;
            CalculateDuration();
        }

        public void CalculateDuration()
        {
            float distance = Vector2.Distance(startPosition, endPosition);
            duration = (int)(distance / (moveSpeedVariation + Mathness.RandomNumber(-(int)moveSpeedVariation, (int)moveSpeedVariation)));
        }

        public override void Update()
        {
            base.Update();

            if (!am.fading && fadingBG)
            {
                if (fadeMode == FadeMode.InAndOut)
                {
                    if (alpha == 255)
                    {
                        am.StartFade(fadeRate + Mathness.RandomNumber(-fadeVariation, fadeVariation), alpha, 0);
                    }
                    else if (alpha == 0)
                    {
                        am.StartFade(fadeRate + Mathness.RandomNumber(-fadeVariation, fadeVariation), alpha, 255);
                    }
                }
                else if (fadeMode == FadeMode.RandomFlashes)
                {
                }
            }
            if (!am.animating && movingBG)
            {
                if (exactTarget == null)
                {
                    if (position == endPosition)
                    {
                        if (followTarget != null) startPosition = followTarget.position + followOffset;
                        CalculateDuration();
                        am.StartNewAnimation(AnimationType.Linear, startPosition, endPosition, duration);
                    }
                }
                else position = exactTarget.position;
            }
        }
    }
}
