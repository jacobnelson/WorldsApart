using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldsApart.Code.Controllers;
using WorldsApart.Code.Graphics;
using WorldsApart.Code.Gamestates;

using System.Diagnostics;

namespace WorldsApart.Code.Entities
{
    class PortalParticles : EventObject
    {
        public bool isActive = false;
        public bool defaultActive = false;

        public List<Particle> particleList;
        public List<Vector2> verticeList = new List<Vector2>();
        int duration = 30;
        Vector2 minScale = new Vector2(.5f);
        Vector2 maxScale = new Vector2(1f);
        //public List<ParticleEmitter> emitterList;
        GSPlay gsPlay;

        float trailCounter = 0;
        float trailRate = .05f;

        public PortalParticles(GSPlay gsPlay, List<Particle> particleList) : base(Art.sparkle, Vector2.Zero)
        {
            this.gsPlay = gsPlay;
            this.particleList = particleList;

            visible = false;
        }

        public void AddVertex(Vector2 vertex)
        {
            verticeList.Add(vertex);
        }

        public void SetUpParticles()
        {
            for (int i = 0; i < particleList.Count; i++)
            {
                if (verticeList.Count > i)
                {
                    particleList[i].scale = minScale;
                    particleList[i].position = verticeList[i];
                }
                else return;
            }
        }

        public void Activate()
        {
            isActive = true;
            foreach (Particle p in particleList)
            {
                p.visible = true;
            }
        }

        public void Deactivate()
        {
            isActive = false;
            foreach (Particle p in particleList)
            {
                p.visible = false;
            }
        }

        public override void ActivateEvent(TriggerState triggerState)
        {
            base.ActivateEvent(triggerState);

            if (triggerState == TriggerState.Triggered)
            {
                if (defaultActive)
                {
                    Deactivate();
                }
                else
                {
                    Activate();
                }
            }
            else
            {
                if (!defaultActive)
                {
                    Deactivate();
                }
                else
                {
                    Activate();
                }
            }
        }
        

        public override void Update()
        {
            if (!isActive) return;

            bool leaveTrail = false;
            trailCounter += Time.GetSeconds();
            if (trailCounter >= trailRate)
            {
                trailCounter = 0;
                leaveTrail = true;
            }

            foreach (Particle particle in particleList)
            {
                for (int i = 0; i < verticeList.Count; i++)
                {
                    if (!particle.am.animating)
                    {
                        if (particle.position == verticeList[i])
                        {
                            if (i + 1 < verticeList.Count)
                            {
                                particle.am.StartNewAnimation(AnimationType.Linear, particle.position, verticeList[i + 1], duration);
                            }
                            else
                            {
                                particle.am.StartNewAnimation(AnimationType.Linear, particle.position, verticeList[0], duration);
                            }
                        }
                    }
                }

                if (!particle.am.scaling)
                {
                    if (particle.scale == minScale) particle.am.StartScale(duration, particle.scale, maxScale);
                    if (particle.scale == maxScale) particle.am.StartScale(duration, particle.scale, minScale);
                }


                if (leaveTrail)
                {
                    Particle trail = gsPlay.AddParticle(Art.sparkle, particle.position);
                    trail.color = particle.color;
                    trail.scale = particle.scale;
                    trail.startAlpha = particle.alpha;
                    trail.endAlpha = 0;
                    trail.life = 60;
                    trail.StartParticleSystems();
                }

                

            }

        }


    }
}
