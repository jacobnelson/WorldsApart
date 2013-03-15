using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldsApart.Code.Entities;

namespace WorldsApart.Code.Controllers
{
    class TimerObject : EventObject
    {
        public int timerCounter = 0;
        public int timerRate = 240;
        public bool ticking = false;

        public TimerObject(EventTrigger eventTrigger, int duration) : base(null, Vector2.Zero)
        {
            AddEvent(eventTrigger);
            timerRate = duration;
        }

        public override void Update()
        {

            if (ticking)
            {
                timerCounter++;
                if (timerCounter >= timerRate)
                {
                    timerCounter = 0;
                    ActivateEvent(TriggerState.Untriggered);
                    ticking = false;
                }
            }
        }

        public override void ActivateEvent(TriggerState triggerState)
        {
            

            if (triggerState == TriggerState.Triggered)
            {
                foreach (EventTrigger eventTrigger in triggerList)
                {
                    eventTrigger.ActivateEvent(TriggerState.Triggered);
                }
                ticking = true;
            }
            else
            {
                foreach (EventTrigger eventTrigger in triggerList)
                {
                    eventTrigger.ActivateEvent(TriggerState.Untriggered);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
