using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using WorldsApart.Code.Graphics;

namespace WorldsApart.Code.Controllers
{
    enum AnimationType
    {
        Linear,
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInQuart,
        EaseOutQuart,
        EaseInOutQuart,
        EaseInQuint,
        EaseOutQuint,
        EaseInOutQuint,
        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo,
        EaseInSine,
        EaseOutSine,
        EaseInOutSine,
        EaseInCirc,
        EaseOutCirc,
        EaseInOutCirc,
        LinearArc
    }

    class AnimationManager
    {
        public Sprite target;

        public AnimationType animationType;
        public Vector2 startPosition;
        public Vector2 endPosition;
        public int animateDuration;
        public Vector2 currentPosition;
        public int animateTime = 0;
        public bool animating;

        public byte currentAlpha;
        public byte startAlpha;
        public byte endAlpha;
        public int fadeDuration;
        public int fadeTime = 0;
        public bool fading;

        public Vector2 currentScale;
        public Vector2 startScale;
        public Vector2 endScale;
        public int scaleDuration;
        public int scaleTime;
        public bool scaling;



        public AnimationManager(Sprite target)
        {
            this.target = target;
        }

        public void StartNewAnimation(AnimationType animationType, Vector2 startPosition, Vector2 endPosition, int duration)
        {
            this.animationType = animationType;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.animateDuration = duration;
            currentPosition = startPosition;
            animateTime = 0;
            animating = true;
        }

        public void StartFade(int duration, byte startAlpha, byte endAlpha = 0)
        {
            if (startAlpha < 0 || endAlpha > 255) return;
            fadeDuration = duration;
            this.startAlpha = startAlpha;
            this.endAlpha = endAlpha;
            fadeTime = 0;
            fading = true;
        }

        public void StartScale(int duration, Vector2 startScale, Vector2 endScale)
        {
            scaleDuration = duration;
            this.startScale = startScale;
            this.endScale = endScale;
            scaleTime = 0;
            scaling = true;
        }

        public void Update()
        {
            if (animating)
            {
                switch (animationType)
                {
                    case AnimationType.Linear:
                        currentPosition = GetLinear(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseInQuad:
                        currentPosition = GetEaseInQuad(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseOutQuad:
                        currentPosition = GetEaseOutQuad(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseInOutQuad:
                        currentPosition = GetEaseInOutQuad(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseInCubic:
                        currentPosition = GetEaseInCubic(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseOutCubic:
                        currentPosition = GetEaseOutCubic(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseInOutCubic:
                        currentPosition = GetEaseInOutCubic(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseInQuart:
                        currentPosition = GetEaseInQuart(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseOutQuart:
                        currentPosition = GetEaseOutQuart(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseInOutQuart:
                        currentPosition = GetEaseInOutQuart(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseInQuint:
                        currentPosition = GetEaseInQuint(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseOutQuint:
                        currentPosition = GetEaseOutQuint(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseInOutQuint:
                        currentPosition = GetEaseInOutQuint(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseInSine:
                        currentPosition = GetEaseInSine(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseOutSine:
                        currentPosition = GetEaseOutSine(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseInOutSine:
                        currentPosition = GetEaseInOutSine(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseInExpo:
                        currentPosition = GetEaseInExpo(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseOutExpo:
                        currentPosition = GetEaseOutExpo(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseInOutExpo:
                        currentPosition = GetEaseInOutExpo(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseInCirc:
                        currentPosition = GetEaseInCirc(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseOutCirc:
                        currentPosition = GetEaseOutCirc(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.EaseInOutCirc:
                        currentPosition = GetEaseInOutCirc(animateTime, startPosition, endPosition, animateDuration);
                        break;
                    case AnimationType.LinearArc:
                        currentPosition += GetLinearArc(animateTime, startPosition, endPosition, animateDuration);
                        break;
                }
                animateTime++;
                if (animateTime > animateDuration)
                {
                    animating = false;
                }
                target.position = currentPosition;
            }

            if (fading)
            {
                int changeAlpha = endAlpha - startAlpha;
                float ratio = (float)fadeTime / (float)fadeDuration;
                changeAlpha = startAlpha + (int)(changeAlpha * ratio);
                if (changeAlpha > 255) changeAlpha = 255;
                else if (changeAlpha < 0) changeAlpha = 0;
                currentAlpha = (byte)changeAlpha;
                fadeTime++;
                if (fadeTime > fadeDuration)
                {
                    fading = false;
                    currentAlpha = endAlpha;

                }
                target.alpha = currentAlpha;

            }

            if (scaling)
            {
                Vector2 changeScale = endScale - startScale;
                float ratio = (float)scaleTime / (float)scaleDuration;
                changeScale = startScale + changeScale * ratio;
                currentScale = changeScale;
                scaleTime++;
                if (scaleTime > scaleDuration)
                {
                    scaling = false;
                    currentScale = endScale;
                }
                target.scale = currentScale;
            }




        }

        static Vector2 GetLinear(int time, Vector2 startPosition, Vector2 endPosition, int duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            float ratio = (float)time / (float)duration;
            return startPosition + changePosition * ratio;
        }

        static Vector2 GetEaseInQuad(int time, Vector2 startPosition, Vector2 endPosition, int duration)
        {
            float changeX = endPosition.X - startPosition.X;
            float changeY = endPosition.Y - startPosition.Y;
            float ratio = (float)time / (float)duration;
            float posX = changeX * ratio * ratio + startPosition.X;
            float posY = changeY * ratio * ratio + startPosition.Y;
            return new Vector2(posX, posY);
        }

        static Vector2 GetEaseOutQuad(int time, Vector2 startPosition, Vector2 endPosition, int duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            float ratio = (float)time / (float)duration;
            return -changePosition * ratio * (ratio - 2) + startPosition;
        }

        static Vector2 GetEaseInOutQuad(int time, Vector2 startPosition, Vector2 endPosition, int duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            float ratio = (float)time / ((float)duration / 2);
            if ((ratio) < 1) return changePosition / 2 * ratio * ratio + startPosition;
            ratio -= 1;
            return -changePosition / 2 * (ratio * (ratio - 2) - 1) + startPosition;
        }

        static Vector2 GetEaseInCubic(int time, Vector2 startPosition, Vector2 endPosition, int duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            return changePosition * (float)Math.Pow((float)time / (float)duration, 3) + startPosition;
        }

        static Vector2 GetEaseOutCubic(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            return changePosition * ((float)Math.Pow(time / duration - 1, 3) + 1) + startPosition;
        }

        static Vector2 GetEaseInOutCubic(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        {
            float ratio = time / (duration / 2);
            Vector2 changePosition = endPosition - startPosition;
            if (ratio < 1) return changePosition / 2 * (float)Math.Pow(ratio, 3) + startPosition;
            return changePosition / 2 * (float)(Math.Pow(ratio - 2, 3) + 2) + startPosition;

        }

        static Vector2 GetEaseInQuart(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            return changePosition * (float)Math.Pow(time / duration, 4) + startPosition;
        }

        static Vector2 GetEaseOutQuart(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            return -changePosition * ((float)Math.Pow(time / duration - 1, 4) - 1) + startPosition;
        }

        static Vector2 GetEaseInOutQuart(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        {
            float ratio = time / (duration / 2);
            Vector2 changePosition = endPosition - startPosition;
            if (ratio < 1) return changePosition / 2 * (float)Math.Pow(ratio, 4) + startPosition;
            return -changePosition / 2 * ((float)Math.Pow(ratio - 2, 4) - 2) + startPosition;
        }

        static Vector2 GetEaseInQuint(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            return changePosition * (float)Math.Pow(time / duration, 5) + startPosition;
        }

        static Vector2 GetEaseOutQuint(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            return changePosition * ((float)Math.Pow(time / duration - 1, 5) + 1) + startPosition;
        }

        static Vector2 GetEaseInOutQuint(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            float ratio = time / (duration / 2);
            if (ratio < 1) return changePosition / 2 * (float)Math.Pow(ratio, 5) + startPosition;
            return changePosition / 2 * ((float)Math.Pow(ratio - 2, 5) + 2) + startPosition;
        }

        static Vector2 GetEaseInSine(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            return changePosition * (float)(1 - Math.Cos(time / duration * (Math.PI / 2))) + startPosition;
        }

        static Vector2 GetEaseOutSine(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            return changePosition * (float)Math.Sin(time / duration * (Math.PI / 2)) + startPosition;
        }

        static Vector2 GetEaseInOutSine(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            return changePosition / 2 * (1 - (float)Math.Cos(Math.PI * time / duration)) + startPosition;
        }

        static Vector2 GetEaseInExpo(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            return changePosition * (float)Math.Pow(2, 10 * (time / duration - 1)) + startPosition;
        }

        static Vector2 GetEaseOutExpo(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            return changePosition * (-(float)Math.Pow(2, -10 * time / duration) + 1) + startPosition;
        }

        static Vector2 GetEaseInOutExpo(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            float ratio = time / (duration / 2);
            if (time < 1) return changePosition / 2 * (float)Math.Pow(2, 10 * (ratio - 1)) + startPosition;
            return changePosition / 2 * (-(float)Math.Pow(2, -10 * ratio - 1) + 2) + startPosition;
        }

        static Vector2 GetEaseInCirc(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            float ratio = time / duration;
            return changePosition * (1 - (float)Math.Sqrt(1 - ratio * ratio)) + startPosition;
        }

        static Vector2 GetEaseOutCirc(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            float ratio = time / duration - 1;
            return changePosition * (float)Math.Sqrt(1 - ratio * ratio) + startPosition;
        }

        static Vector2 GetEaseInOutCirc(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            float ratio = time / (duration / 2);
            if (ratio < 1) return changePosition / 2 * (1 - (float)Math.Sqrt(1 - ratio * ratio)) + startPosition;
            ratio -= 2;
            return changePosition / 2 * ((float)Math.Sqrt(1 - ratio * ratio) + 1) + startPosition;
        }

        static Vector2 GetLinearArc(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            float ratio = (float)time / (float)duration;
            Vector2 arcPosition;

            return startPosition + changePosition * ratio;
        }


    }
}
