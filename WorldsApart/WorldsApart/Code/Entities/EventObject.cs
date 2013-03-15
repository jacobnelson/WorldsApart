using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldsApart.Code.Controllers;

using System.Diagnostics;

namespace WorldsApart.Code.Entities
{
    class EventObject : PhysObj
    {
        //public EventTrigger eventTrigger;
        public TriggerState triggerState = TriggerState.Untriggered;
        public List<EventTrigger> triggerList = new List<EventTrigger>();
        public bool onlyTriggered = false;

        public EventObject(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
        }

        public virtual void AddEvent(EventTrigger eventTrigger)
        {
            triggerList.Add(eventTrigger);
        }

        public virtual void ActivateEvent(TriggerState triggerState)
        {
            //if (onlyTriggered && triggerState != TriggerState.Triggered) return;
            if (triggerState == this.triggerState) return;
            this.triggerState = triggerState;
        }
    }
}
