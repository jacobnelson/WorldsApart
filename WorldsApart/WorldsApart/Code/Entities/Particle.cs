using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldsApart.Code.Gamestates;
using WorldsApart.Code.Graphics;

namespace WorldsApart.Code.Entities
{
    class Particle : PhysObj
    {
        GSPlay gsPlay;

        public Sprite target;

        public int life = 15;
        int lifeCounter = 0;

        public bool isDead = false;

        public byte startAlpha = 255;
        public byte endAlpha = 128;
        public float startScale = 1;
        public float endScale = 1.2f;


        public Particle(GSPlay gsPlay, Texture2D texture, Vector2 position) : base(texture, position)
        {
            this.gsPlay = gsPlay;
            isSolidEnvironment = false;
            rotation = (float)Mathness.RandomNumber(0, 100) / 100 * (float)Math.PI * 2;
        }

        public void StartParticleSystems()
        {
            am.StartFade(life, startAlpha, endAlpha);
            am.StartScale(life, new Vector2(startScale), new Vector2(endScale));
            
        }

        public override void Update()
        {
            base.Update();
            lifeCounter++;
            if (lifeCounter >= life)
            {
                Die();
            }
            if (target != null)
            {
                position = target.position;
            }
        }

        public override void Die()
        {
            isDead = true;
        }
    }
}
