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
    class InteractiveParticleSet
    {
        GSPlay gsPlay;

        List<PhysObj> particleList = new List<PhysObj>();

        public InteractiveParticleSet(GSPlay gsPlay, AnimatedSprite sprite)
        {
            this.gsPlay = gsPlay;
        }



        



    }
}
