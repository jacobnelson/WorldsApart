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
    class ParticleEmitter
    {
        GSPlay gsPlay;

        public Vector2 position;
        public Vector2 randomDisplacement = Vector2.Zero;
        public AnimatedSprite particle;
        public float rotationSpeed = .01f;
        public float startRotation = 0;
        public bool randomRotation = false;
        public Vector2 speed = Vector2.Zero;

        public bool fadeInOut = false;

        public byte startAlpha = 0;
        public byte endAlpha = 255;
        public float startScale = .5f;
        public float endScale = .8f;

        public int spawnRate = 60;
        int spawnCounter = 0;

        public int life = 30;

        public ParticleEmitter(GSPlay gsPlay, AnimatedSprite particle, Vector2 position)
        {
            this.gsPlay = gsPlay;
            this.particle = particle;
            this.position = position;
            spawnCounter = Mathness.RandomNumber(0, spawnRate);
        }

        public void Update()
        {
            spawnCounter++;
            if (spawnCounter >= spawnRate)
            {
                Vector2 spawnPos = position + new Vector2(Mathness.RandomNumber((int)-randomDisplacement.X, (int)randomDisplacement.X), Mathness.RandomNumber((int)-randomDisplacement.Y, (int)randomDisplacement.Y));
                Particle p = gsPlay.AddAnimatedParticle(particle, spawnPos, speed);
                p.rotationSpeed = rotationSpeed;
                if (randomRotation) p.rotation = (float)Mathness.RandomNumber(0, 100) / 100 * (float)Math.PI * 2;
                else p.rotation = startRotation;
                p.life = life;
                p.startScale = startScale;
                p.endScale = endScale;
                p.startAlpha = startAlpha;
                p.endAlpha = endAlpha;
                p.fadeInOut = fadeInOut;
                p.StartParticleSystems();
                spawnCounter = Mathness.RandomNumber(-2,2);
            }
        }
        

    }
}
