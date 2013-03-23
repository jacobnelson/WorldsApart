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
        EaseInBounce,
        EaseOutBounce,
        EaseInOutBounce,
        EaseInBack,
        EaseOutBack,
        EaseInOutBack,
        EaseInElastic,
        EaseOutElastic,
        EaseInOutElastic,
        LinearArc,
    }

    class AnimationManager
    {
        public Sprite target;

        public AnimationType positionType;
        public Vector2 startPosition;
        public Vector2 changePosition;
        public float animationDuration;
        public Vector2 currentPosition;
        public int animateTime = 0;
        public bool animating;

        public AnimationType fadeType = AnimationType.Linear;
        public byte startAlpha;
        public int changeAlpha;
        public int fadeDuration;
        public int fadeTime = 0;
        public bool fading;

        public AnimationType scaleType = AnimationType.Linear;
        public Vector2 startScale;
        public Vector2 changeScale;
        public int scaleDuration;
        public int scaleTime;
        public bool scaling;

        public bool pauseMovement = false;
        public Vector2 pausedPosition = Vector2.Zero;

        public AnimationManager(Sprite target)
        {
            this.target = target;
        }

        public void StartNewAnimation(AnimationType animationType, Vector2 startPosition, Vector2 endPosition, int duration)
        {
            positionType = animationType;
            this.startPosition = startPosition;
            this.changePosition = endPosition - startPosition;
            this.animationDuration = duration;
            currentPosition = startPosition;
            animateTime = 0;
            animating = true;
        }

        public void StartFade(int duration, byte startAlpha, byte endAlpha = 0, AnimationType fadeType = AnimationType.Linear)
        {
            if (startAlpha < 0 || endAlpha > 255) return;
            fadeDuration = duration;
            this.fadeType = fadeType;
            this.startAlpha = startAlpha;
            this.changeAlpha = endAlpha - startAlpha;
            fadeTime = 0;
            fading = true;
        }

        public void StartScale(int duration, Vector2 startScale, Vector2 endScale, AnimationType scaleType = AnimationType.Linear)
        {
            scaleDuration = duration;
            this.scaleType = scaleType;
            this.startScale = startScale;
            this.changeScale = endScale - startScale;
            scaleTime = 0;
            scaling = true;
        }

        public float GetValue(AnimationType animationType, float t, float b, float c, float d)
        {
            float value = 0;
            switch (animationType)
            {
                case AnimationType.Linear:
                    value = linear(t, b, c, d);
                    break;
                case AnimationType.EaseInQuad:
                    value = easeInQuad(t, b, c, d);
                    break;
                case AnimationType.EaseOutQuad:
                    value = easeOutQuad(t, b, c, d);
                    break;
                case AnimationType.EaseInOutQuad:
                    value = easeInOutQuad(t, b, c, d);
                    break;
                case AnimationType.EaseInCubic:
                    value = easeInCubic(t, b, c, d);
                    break;
                case AnimationType.EaseOutCubic:
                    value = easeOutCubic(t, b, c, d);
                    break;
                case AnimationType.EaseInOutCubic:
                    value = easeInOutCubic(t, b, c, d);
                    break;
                case AnimationType.EaseInQuart:
                    value = easeInQuart(t, b, c, d);
                    break;
                case AnimationType.EaseOutQuart:
                    value = easeOutQuart(t, b, c, d);
                    break;
                case AnimationType.EaseInOutQuart:
                    value = easeInOutQuart(t, b, c, d);
                    break;
                case AnimationType.EaseInQuint:
                    value = easeInQuint(t, b, c, d);
                    break;
                case AnimationType.EaseOutQuint:
                    value = easeOutQuint(t, b, c, d);
                    break;
                case AnimationType.EaseInOutQuint:
                    value = easeInOutQuint(t, b, c, d);
                    break;
                case AnimationType.EaseInSine:
                    value = easeInSine(t, b, c, d);
                    break;
                case AnimationType.EaseOutSine:
                    value = easeOutSine(t, b, c, d);
                    break;
                case AnimationType.EaseInOutSine:
                    value = easeInOutSine(t, b, c, d);
                    break;
                case AnimationType.EaseInExpo:
                    value = easeInExpo(t, b, c, d);
                    break;
                case AnimationType.EaseOutExpo:
                    value = easeOutExpo(t, b, c, d);
                    break;
                case AnimationType.EaseInOutExpo:
                    value = easeInOutExpo(t, b, c, d);
                    break;
                case AnimationType.EaseInCirc:
                    value = easeInCirc(t, b, c, d);
                    break;
                case AnimationType.EaseOutCirc:
                    value = easeOutCirc(t, b, c, d);
                    break;
                case AnimationType.EaseInOutCirc:
                    value = easeInOutCirc(t, b, c, d);
                    break;
                case AnimationType.EaseInBounce:
                    value = easeInBounce(t, b, c, d);
                    break;
                case AnimationType.EaseOutBounce:
                    value = easeOutBounce(t, b, c, d);
                    break;
                case AnimationType.EaseInOutBounce:
                    value = easeInOutBounce(t, b, c, d);
                    break;
                case AnimationType.EaseInBack:
                    value = easeInBack(t, b, c, d);
                    break;
                case AnimationType.EaseOutBack:
                    value = easeOutBack(t, b, c, d);
                    break;
                case AnimationType.EaseInOutBack:
                    value = easeInOutBack(t, b, c, d);
                    break;
                case AnimationType.EaseInElastic:
                    value = easeInElastic(t, b, c, d);
                    break;
                case AnimationType.EaseOutElastic:
                    value = easeOutElastic(t, b, c, d);
                    break;
                case AnimationType.EaseInOutElastic:
                    value = easeInOutElastic(t, b, c, d);
                    break;
                case AnimationType.LinearArc:

                    break;
            }
            return value;
        }

        public void Update()
        {
            if (animating)
            {
                Vector2 newPos = new Vector2(GetValue(positionType, animateTime, startPosition.X, changePosition.X, animationDuration), GetValue(positionType, animateTime, startPosition.Y, changePosition.Y, animationDuration));
                animateTime++;
                if (animateTime > animationDuration)
                {
                    animating = false;
                }
                target.position = newPos;
            }

            if (fading)
            {
                float newAlpha = GetValue(fadeType, fadeTime, startAlpha, changeAlpha, fadeDuration);
                fadeTime++;
                if (fadeTime > fadeDuration)
                {
                    fading = false;
                }
                
                target.alpha = (byte)MathHelper.Clamp(newAlpha, 0, 255);

            }

            if (scaling)
            {
                Vector2 newScale = new Vector2(GetValue(scaleType, scaleTime, startScale.X, changeScale.X, scaleDuration), GetValue(scaleType, scaleTime, startScale.Y, changeScale.Y, scaleDuration));
                scaleTime++;
                if (scaleTime > scaleDuration)
                {
                    scaling = false;
                }
                target.scale = newScale;
            }
            if (pauseMovement)
            {
                target.position = pausedPosition;
            }



        }

        public void Pause()
        {
            pauseMovement = true;
            pausedPosition = target.position;
        }

        float linear(float t, float b, float c, float d)
        {
            return c * t / d + b;
        }
        float easeInQuad(float t, float b, float c, float d)
        {
            t /= d;
            return c * t * t + b;
        }
        float easeOutQuad(float t, float b, float c, float d)
        {
            t /= d;
            return -c * t * (t - 2) + b;
        }
        float easeInOutQuad(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t + b;
            t--;
            return -c / 2 * (t * (t - 2) - 1) + b;
        }
        float easeInCubic(float t, float b, float c, float d)
        {
            t /= d;
            return c * t * t * t + b;
        }
        float easeOutCubic(float t, float b, float c, float d)
        {
            t /= d;
            t--;
            return c * (t * t * t + 1) + b;
        }
        float easeInOutCubic(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t * t + b;
            t -= 2;
            return c / 2 * (t * t * t + 2) + b;
        }
        float easeInQuart(float t, float b, float c, float d)
        {
            t /= d;
            return c * t * t * t * t + b;
        }
        float easeOutQuart(float t, float b, float c, float d)
        {
            t /= d;
            t--;
            return -c * (t * t * t * t - 1) + b;
        }
        float easeInOutQuart(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t * t * t + b;
            t -= 2;
            return -c / 2 * (t * t * t * t - 2) + b;
        }
        float easeInQuint(float t, float b, float c, float d)
        {
            t /= d;
            return c * t * t * t * t * t + b;
        }
        float easeOutQuint(float t, float b, float c, float d)
        {
            t /= d;
            t--;
            return c * (t * t * t * t * t + 1) + b;
        }
        float easeInOutQuint(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t * t * t * t + b;
            t -= 2;
            return c / 2 * (t * t * t * t * t + 2) + b;
        }
        float easeInSine(float t, float b, float c, float d)
        {
            return -c * (float)Math.Cos(t / d * (Math.PI / 2)) + c + b;
        }
        float easeOutSine(float t, float b, float c, float d)
        {
            return c * (float)Math.Sin(t / d * (Math.PI / 2)) + b;
        }
        float easeInOutSine(float t, float b, float c, float d)
        {
            return -c / 2 * ((float)Math.Cos(Math.PI * t / d) - 1) + b;
        }
        float easeInExpo(float t, float b, float c, float d)
        {
            return c * (float)Math.Pow(2, 10 * (t / d - 1)) + b;
        }
        float easeOutExpo(float t, float b, float c, float d)
        {
            return c * (-(float)Math.Pow(2, -10 * t / d) + 1) + b;
        }
        float easeInOutExpo(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * (float)Math.Pow(2, 10 * (t - 1)) + b;
            t--;
            return c / 2 * (-(float)Math.Pow(2, -10 * t) + 2) + b;
        }
        float easeInCirc(float t, float b, float c, float d)
        {
            t /= d;
            return -c * ((float)Math.Sqrt(1 - t * t) - 1) + b;
        }
        float easeOutCirc(float t, float b, float c, float d)
        {
            t /= d;
            t--;
            return c * (float)Math.Sqrt(1 - t * t) + b;
        }
        float easeInOutCirc(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1) return -c / 2 * ((float)Math.Sqrt(1 - t * t) - 1) + b;
            t -= 2;
            return c / 2 * ((float)Math.Sqrt(1 - t * t) + 1) + b;
        }
        float easeInElastic(float t, float b, float c, float d)
        {
            if (t == 0) return b;
            if ((t /= d) == 1) return b + c;
            float a = 0;
            float p = d * .3f;
            float s = 0;
            if (a < Math.Abs(c))
            {
                a = c;
                s = p / 4;
            }
            else s = p / (2 * (float)Math.PI) * (float)Math.Asin(c / a);
            return -(a * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin((t * d - s) * (2 * Math.PI) / p)) + b;
        }
        float easeOutElastic(float t, float b, float c, float d)
        {
            if (t == 0) return b;
            if ((t /= d) == 1) return b + c;
            float a = 0;
            float p = d * .3f;
            float s = 0;
            if (a < Math.Abs(c))
            {
                a = c;
                s = p / 4;
            }
            else s = p / (2 * (float)Math.PI) * (float)Math.Asin(c / a);
            return a * (float)Math.Pow(2, -10 * t) * (float)Math.Sin((t * d - s) * (2 * Math.PI) / p) + c + b;
        }
        float easeInOutElastic(float t, float b, float c, float d)
        {
            if (t == 0) return b;
            if ((t /= d / 2) == 2) return b + c;
            float a = 0;
            float p = d * (.3f * 1.5f);
            float s = 0;
            if (a < Math.Abs(c))
            {
                a = c;
                s = p / 4;
            }
            else s = p / (2 * (float)Math.PI) * (float)Math.Asin(c / a);
            if (t < 1) return -.5f * (a * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p)) + b;
            return a * (float)Math.Pow(2, -10 * (t -= 1)) * (float)Math.Sin((t * d - s) * (2 * Math.PI) / p) * .5f + c + b;
        }
        float easeInBack(float t, float b, float c, float d)
        {
            float s = 1.70158f;
            return c * (t /= d) * t * ((s + 1) * t - s) + b;
        }
        float easeOutBack(float t, float b, float c, float d)
        {
            float s = 1.70158f;
            return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
        }
        float easeInOutBack(float t, float b, float c, float d)
        {
            float s = 1.70158f;
            if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b;
            return c / 2 * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b;
        }
        float easeInBounce(float t, float b, float c, float d)
        {
            return c - easeOutBounce(d - t, 0, c, d) + b;
        }
        float easeOutBounce(float t, float b, float c, float d)
        {
            if ((t /= d) < (1 / 2.75))
            {
                return c * (7.5625f * t * t) + b;
            }
            else if (t < (2 / 2.75))
            {
                return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f) + b;
            }
            else if (t < (2.5 / 2.75))
            {
                return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f) + b;
            }
            else
            {
                return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f) + b;
            }
        }

        float easeInOutBounce(float t, float b, float c, float d)
        {
            if (t < d / 2) return easeInBounce(t * 2, 0, c, d) * .5f + b;
            return easeOutBounce(t * 2 - d, 0, c, d) * .5f + c * .5f + b;
        }

        //static Vector2 GetLinear(int time, Vector2 startPosition, Vector2 endPosition, int duration)
        //{
        //    Vector2 changePosition = endPosition - startPosition;
        //    float ratio = (float)time / (float)duration;
        //    return startPosition + changePosition * ratio;
        //}

        //static Vector2 GetEaseInQuad(int time, Vector2 startPosition, Vector2 endPosition, int duration)
        //{
        //    float changeX = endPosition.X - startPosition.X;
        //    float changeY = endPosition.Y - startPosition.Y;
        //    float ratio = (float)time / (float)duration;
        //    float posX = changeX * ratio * ratio + startPosition.X;
        //    float posY = changeY * ratio * ratio + startPosition.Y;
        //    return new Vector2(posX, posY);
        //}

        //static Vector2 GetEaseOutQuad(int time, Vector2 startPosition, Vector2 endPosition, int duration)
        //{
        //    Vector2 changePosition = endPosition - startPosition;
        //    float ratio = (float)time / (float)duration;
        //    return -changePosition * ratio * (ratio - 2) + startPosition;
        //}

        //static Vector2 GetEaseInOutQuad(int time, Vector2 startPosition, Vector2 endPosition, int duration)
        //{
        //    Vector2 changePosition = endPosition - startPosition;
        //    float ratio = (float)time / ((float)duration / 2);
        //    if ((ratio) < 1) return changePosition / 2 * ratio * ratio + startPosition;
        //    ratio -= 1;
        //    return -changePosition / 2 * (ratio * (ratio - 2) - 1) + startPosition;
        //}

        //static Vector2 GetEaseInCubic(int time, Vector2 startPosition, Vector2 endPosition, int duration)
        //{
        //    Vector2 changePosition = endPosition - startPosition;
        //    return changePosition * (float)Math.Pow((float)time / (float)duration, 3) + startPosition;
        //}

        //static Vector2 GetEaseOutCubic(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        //{
        //    Vector2 changePosition = endPosition - startPosition;
        //    return changePosition * ((float)Math.Pow(time / duration - 1, 3) + 1) + startPosition;
        //}

        //static Vector2 GetEaseInOutCubic(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        //{
        //    float ratio = time / (duration / 2);
        //    Vector2 changePosition = endPosition - startPosition;
        //    if (ratio < 1) return changePosition / 2 * (float)Math.Pow(ratio, 3) + startPosition;
        //    return changePosition / 2 * (float)(Math.Pow(ratio - 2, 3) + 2) + startPosition;

        //}

        //static Vector2 GetEaseInQuart(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        //{
        //    Vector2 changePosition = endPosition - startPosition;
        //    return changePosition * (float)Math.Pow(time / duration, 4) + startPosition;
        //}

        //static Vector2 GetEaseOutQuart(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        //{
        //    Vector2 changePosition = endPosition - startPosition;
        //    return -changePosition * ((float)Math.Pow(time / duration - 1, 4) - 1) + startPosition;
        //}

        //static Vector2 GetEaseInOutQuart(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        //{
        //    float ratio = time / (duration / 2);
        //    Vector2 changePosition = endPosition - startPosition;
        //    if (ratio < 1) return changePosition / 2 * (float)Math.Pow(ratio, 4) + startPosition;
        //    return -changePosition / 2 * ((float)Math.Pow(ratio - 2, 4) - 2) + startPosition;
        //}

        //static Vector2 GetEaseInQuint(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        //{
        //    Vector2 changePosition = endPosition - startPosition;
        //    return changePosition * (float)Math.Pow(time / duration, 5) + startPosition;
        //}

        //static Vector2 GetEaseOutQuint(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        //{
        //    Vector2 changePosition = endPosition - startPosition;
        //    return changePosition * ((float)Math.Pow(time / duration - 1, 5) + 1) + startPosition;
        //}

        //static Vector2 GetEaseInOutQuint(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        //{
        //    Vector2 changePosition = endPosition - startPosition;
        //    float ratio = time / (duration / 2);
        //    if (ratio < 1) return changePosition / 2 * (float)Math.Pow(ratio, 5) + startPosition;
        //    return changePosition / 2 * ((float)Math.Pow(ratio - 2, 5) + 2) + startPosition;
        //}

        //static Vector2 GetEaseInSine(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        //{
        //    Vector2 changePosition = endPosition - startPosition;
        //    return changePosition * (float)(1 - Math.Cos(time / duration * (Math.PI / 2))) + startPosition;
        //}

        //static Vector2 GetEaseOutSine(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        //{
        //    Vector2 changePosition = endPosition - startPosition;
        //    return changePosition * (float)Math.Sin(time / duration * (Math.PI / 2)) + startPosition;
        //}

        //static Vector2 GetEaseInOutSine(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        //{
        //    Vector2 changePosition = endPosition - startPosition;
        //    return changePosition / 2 * (1 - (float)Math.Cos(Math.PI * time / duration)) + startPosition;
        //}

        //static Vector2 GetEaseInExpo(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        //{
        //    Vector2 changePosition = endPosition - startPosition;
        //    return changePosition * (float)Math.Pow(2, 10 * (time / duration - 1)) + startPosition;
        //}

        //static Vector2 GetEaseOutExpo(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        //{
        //    Vector2 changePosition = endPosition - startPosition;
        //    return changePosition * (-(float)Math.Pow(2, -10 * time / duration) + 1) + startPosition;
        //}

        //static Vector2 GetEaseInOutExpo(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        //{
        //    Vector2 changePosition = endPosition - startPosition;
        //    float ratio = time / (duration / 2);
        //    if (time < 1) return changePosition / 2 * (float)Math.Pow(2, 10 * (ratio - 1)) + startPosition;
        //    return changePosition / 2 * (-(float)Math.Pow(2, -10 * ratio - 1) + 2) + startPosition;
        //}

        //static Vector2 GetEaseInCirc(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        //{
        //    Vector2 changePosition = endPosition - startPosition;
        //    float ratio = time / duration;
        //    return changePosition * (1 - (float)Math.Sqrt(1 - ratio * ratio)) + startPosition;
        //}

        //static Vector2 GetEaseOutCirc(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        //{
        //    Vector2 changePosition = endPosition - startPosition;
        //    float ratio = time / duration - 1;
        //    return changePosition * (float)Math.Sqrt(1 - ratio * ratio) + startPosition;
        //}

        //static Vector2 GetEaseInOutCirc(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        //{
        //    Vector2 changePosition = endPosition - startPosition;
        //    float ratio = time / (duration / 2);
        //    if (ratio < 1) return changePosition / 2 * (1 - (float)Math.Sqrt(1 - ratio * ratio)) + startPosition;
        //    ratio -= 2;
        //    return changePosition / 2 * ((float)Math.Sqrt(1 - ratio * ratio) + 1) + startPosition;
        //}

        static Vector2 GetLinearArc(float time, Vector2 startPosition, Vector2 endPosition, float duration)
        {
            Vector2 changePosition = endPosition - startPosition;
            float ratio = (float)time / (float)duration;
            Vector2 arcPosition;

            return startPosition + changePosition * ratio;
        }


    }
}
