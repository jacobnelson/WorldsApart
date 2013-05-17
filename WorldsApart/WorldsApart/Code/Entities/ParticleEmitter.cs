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
        public Vector2 speed = Vector2.Zero;

        public byte startAlpha = 128;
        public byte endAlpha = 64;
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
        }

        public void Update()
        {
            spawnCounter++;
            if (spawnCounter >= spawnRate)
            {
                Vector2 spawnPos = position + new Vector2(Mathness.RandomNumber((int)-randomDisplacement.X, (int)randomDisplacement.X), Mathness.RandomNumber((int)-randomDisplacement.Y, (int)randomDisplacement.Y));
                Particle p = gsPlay.AddParticle(particle, spawnPos, speed);
                p.rotationSpeed = rotationSpeed;
                p.life = life;
                p.startScale = startScale;
                p.endScale = endScale;
                p.startAlpha = startAlpha;
                p.endAlpha = endAlpha;
                p.StartParticleSystems();
                spawnCounter = Mathness.RandomNumber(-2,2);
            }
        }
        

    }
}
