using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using WorldsApart.Code.Entities;
using WorldsApart.Code.Controllers;

namespace WorldsApart.Code.Graphics
{
    class PointLight : EventObject
    {
        public bool glowing = false;
        public int glowRate = 60;
        public Vector2 startScale = new Vector2(1);
        public Vector2 endScale = new Vector2(1.2f);

        public SpriteIMG target;

        public PointLight(Texture2D texture, Vector2 position, Vector2 scale) : base(texture, position)
        {
            this.scale = scale;
        }

        public void SetGlowing(float startScale, float endScale, int glowRate)
        {
            SetGlowing(new Vector2(startScale), new Vector2(endScale), glowRate);
        }

        public void SetGlowing(Vector2 startScale, Vector2 endScale, int glowRate)
        {
            glowing = true;
            this.startScale = startScale;
            this.endScale = endScale;
            this.glowRate = glowRate;
        }

        public override void Update()
        {
            base.Update();
            if (glowing)
            {
                if (!am.scaling)
                {
                    if (scale == startScale)
                    {
                        am.StartScale(glowRate, scale, endScale);
                    }
                    else if (scale == endScale)
                    {
                        am.StartScale(glowRate, scale, startScale);
                    }
                }
            }

            if (target != null)
            {
                position = target.position;
            }
        }

        public override void Die()
        {
        }

        public override void ActivateEvent(TriggerState triggerState)
        {
            base.ActivateEvent(triggerState);

            if (triggerState == TriggerState.Triggered)
            {
                visible = true;
            }
            else
            {
                visible = false;
            }
        }
    }
}
