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
    class CircularEmitter : ParticleEmitter
    {
        public float angle = 0;
        public float increment = .1f;
        public Vector2 centerPosition;
        public float radius = 48;


        public CircularEmitter(GSPlay gsPlay, AnimatedSprite particle, Vector2 position)
            : base(gsPlay, particle, Vector2.Zero)
        {
            centerPosition = position;
        }

        public override void Update()
        {
            angle += increment;
            float posX = radius * (float)Math.Cos(angle) + centerPosition.X;
            float posY = radius * (float)Math.Sin(angle) + centerPosition.Y;

            position.X = posX;
            position.Y = posY;

            base.Update();
        }
    }
}
