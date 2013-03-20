using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using WorldsApart.Code.Controllers;

using System.Diagnostics;

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
        public int fadeCounter = 0;
        public int fadeVariation = 0;
        public int flashRate = 10;
        public int flashVariation = 0;

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

        public void SetFading(int fadeRate, int fadeVariation)
        {
            this.fadeRate = fadeRate;
            this.fadeVariation = fadeVariation;
        }

        public void SetFlashing(int fadeRate, int fadeVariation, int flashRate, int flashVariation)
        {
            fadeMode = FadeMode.RandomFlashes;
            SetFading(fadeRate, fadeVariation);
            this.flashRate = flashRate;
            this.flashVariation = flashVariation;
        }

        public void SetMoving(Vector2 startPosition, Vector2 endPosition, float moveSpeed, float moveSpeedVariation)
        {
            movingBG = true;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.moveSpeedVariation = moveSpeedVariation;
            CalculateDuration();
        }

        public void TimeMovement()
        {
        }

        public void CalculateDuration()
        {
            float distance = Vector2.Distance(startPosition, endPosition);
            duration = (int)(distance / (moveSpeedVariation + Mathness.RandomNumber(-(int)moveSpeedVariation, (int)moveSpeedVariation)));
        }

        public override void Update()
        {
            base.Update();
            
            if (fadingBG)
            {
                if (!am.fading)
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
                    if (fadeMode == FadeMode.RandomFlashes)
                    {
                        if (alpha == 255)
                        {
                            am.StartFade(flashRate + Mathness.RandomNumber(-flashVariation, flashVariation), alpha, 0, AnimationType.EaseInElastic);
                        }
                        else
                        {
                            if (fadeCounter >= fadeRate)
                            {
                                am.StartFade(flashRate + Mathness.RandomNumber(-flashVariation, flashVariation), alpha, 255, AnimationType.EaseOutElastic);
                                fadeCounter = Mathness.RandomNumber(-fadeVariation, fadeVariation);
                            }
                            else fadeCounter++;
                        }
                    }
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
