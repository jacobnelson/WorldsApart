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

        public Sprite target;

        public bool fadeInOut = false;

        public int life = 15;
        int lifeCounter = 0;

        public bool isDead = false;

        public byte startAlpha = 255;
        public byte endAlpha = 128;
        public float startScale = 1;
        public float endScale = 1.2f;

        public Vector2 randomMinForce = Vector2.Zero;
        public Vector2 randomMaxForce = Vector2.Zero;


        public Particle(Texture2D texture, Vector2 position) : base(texture, position)
        {
            isSolidEnvironment = false;
            //rotation = (float)Mathness.RandomNumber(0, 100) / 100 * (float)Math.PI * 2;
        }

        public void StartParticleSystems()
        {
            if (fadeInOut)
            {
                am.StartFade(life / 2, startAlpha, endAlpha);
            }
            else
            {
                am.StartFade(life, startAlpha, endAlpha);
            }
            am.StartScale(life, new Vector2(startScale), new Vector2(endScale));
            
        }

        public override void Update()
        {
            force = new Vector2(Mathness.RandomNumber(randomMinForce.X, randomMaxForce.X), Mathness.RandomNumber(randomMinForce.Y, randomMaxForce.Y));
            base.Update();
            lifeCounter++;
            if (lifeCounter >= life)
            {
                Die();
            }

            if (lifeCounter == life / 2)
            {
                am.StartFade(life / 2, endAlpha, startAlpha);
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
