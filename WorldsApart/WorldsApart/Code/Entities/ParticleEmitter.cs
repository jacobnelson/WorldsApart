﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldsApart.Code.Graphics;
using WorldsApart.Code.Gamestates;
using WorldsApart.Code.Controllers;

namespace WorldsApart.Code.Entities
{
    class ParticleEmitter
    {
        public GSPlay gsPlay;

        public bool isActive = true;

        public Vector2 position;
        public Vector2 randomDisplacement = Vector2.Zero;
        public AnimatedSprite particle;
        public float rotationSpeed = .01f;
        public float startRotation = 0;
        public bool randomRotation = false;
        public Vector2 speed = Vector2.Zero;
        public Vector2 randomSpeedX = Vector2.Zero;
        public Vector2 randomSpeedY = Vector2.Zero;

        public Color color = Color.White;
        public bool selfIlluminated = false;

        public bool fadeInOut = false;
        public bool bgParticles = false;

        public byte startAlpha = 0;
        public byte endAlpha = 255;
        public float startScale = .5f;
        public float endScale = .8f;

        public int spawnRate = 60;
        public int spawnCounter = 0;

        public int life = 30;

        public ParticleEmitter(GSPlay gsPlay, AnimatedSprite particle, Vector2 position)
        {
            this.gsPlay = gsPlay;
            this.particle = particle;
            this.position = position;
            spawnCounter = Mathness.RandomNumber(0, spawnRate);
        }

        public bool IsOnScreen(Camera camera)
        {
            Rectangle textureArea = new Rectangle((int)position.X - 150, (int)position.Y - 150, 300, 300);
            return textureArea.Intersects(camera.visibleArea);
        }

        virtual public void Update()
        {
            if (isActive)
            {
                spawnCounter++;
                if (spawnCounter >= spawnRate)
                {
                    Vector2 spawnPos = position + new Vector2(Mathness.RandomNumber((int)-randomDisplacement.X, (int)randomDisplacement.X), Mathness.RandomNumber((int)-randomDisplacement.Y, (int)randomDisplacement.Y));
                    Particle p;
                    Vector2 tempSpeed = speed + new Vector2(Mathness.RandomNumber(randomSpeedX.X, randomSpeedX.Y), Mathness.RandomNumber(randomSpeedY.X, randomSpeedY.Y));
                    if (bgParticles) p = gsPlay.AddAnimatedBGParticle(particle, spawnPos, tempSpeed);
                    else p = gsPlay.AddAnimatedParticle(particle, spawnPos, tempSpeed);
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
