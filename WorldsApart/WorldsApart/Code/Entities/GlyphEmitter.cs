using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldsApart.Code.Graphics;
using WorldsApart.Code.Gamestates;

namespace WorldsApart.Code.Entities
{
    class GlyphEmitter : ParticleEmitter
    {
        public List<Texture2D> textureList;


        public GlyphEmitter(GSPlay gsPlay, Texture2D texture, Vector2 position) : base(gsPlay, new AnimatedSprite(texture), position)
        {
            this.gsPlay = gsPlay;
            this.position = position;
            textureList = new List<Texture2D>();
            AddTexture(texture);
        }

        public void AddTexture(Texture2D texture)
        {
            textureList.Add(texture);
        }

        override public void Update()
        {
            if (isActive)
            {
                spawnCounter++;
                if (spawnCounter >= spawnRate)
                {
                    Vector2 spawnPos = position + new Vector2(Mathness.RandomNumber((int)-randomDisplacement.X, (int)randomDisplacement.X), Mathness.RandomNumber((int)-randomDisplacement.Y, (int)randomDisplacement.Y));
                    Particle p;
                    Vector2 tempSpeed = speed + new Vector2(Mathness.RandomNumber(randomSpeedX.X, randomSpeedX.Y), Mathness.RandomNumber(randomSpeedY.X, randomSpeedY.Y));
                    Texture2D texture = textureList[Mathness.RandomNumber(0, textureList.Count - 1)];
                    p = gsPlay.AddParticle(texture, position);
                    p.speed = tempSpeed;
                    p.rotationSpeed = rotationSpeed;
                    if (randomRotation) p.rotation = (float)Mathness.RandomNumber(0, 100) / 100 * (float)Math.PI * 2;
                    else p.rotation = startRotation;
                    p.color = color;
                    p.life = life;
                    p.startScale = startScale;
                    p.endScale = endScale;
                    p.startAlpha = startAlpha;
                    p.endAlpha = endAlpha;
                    p.fadeInOut = fadeInOut;
                    p.selfIlluminating = selfIlluminated;
                    p.StartParticleSystems();
                    spawnCounter = Mathness.RandomNumber(-2, 2);
                }
            }
        }
    }
}
