using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldsApart.Code.Controllers;

namespace WorldsApart.Code.Entities
{
    class Portal : EventObject
    {
        Player player1;
        Player player2;

        public ParticleEmitter pulse;

        public bool goodMode = false;

        public Portal(Player player1, Player player2, EventTrigger eventTrigger, Texture2D texture, Vector2 position) : base(texture, position)
        {
            this.player1 = player1;
            this.player2 = player2;
            isSolidEnvironment = false;
            AddEvent(eventTrigger);
            selfIlluminating = true;
            illuminatingAllTheTime = true;
            
        }

        public override void Update()
        {
            base.Update();

            if (hitBox.CheckCollision(player1.hitBox) && hitBox.CheckCollision(player2.hitBox))
            {
                foreach (EventTrigger eventTrigger in triggerList)
                {
                    eventTrigger.ActivateEvent(TriggerState.Triggered);
                }
            }
        }

        public override void ActivateEvent(TriggerState triggerState)
        {
            if (triggerState == TriggerState.Triggered)
            {
                goodMode = true;
                pulse.color = Color.White;
                ChangeAnimationBounds(1, 1, 8);
            }
        }

        
    }
}
