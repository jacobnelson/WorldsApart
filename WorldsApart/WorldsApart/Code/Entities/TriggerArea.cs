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
    class TriggerArea : EventObject
    {

        public bool touching = false;

        public TriggerArea(EventTrigger eventTrigger, Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            triggerList.Add(eventTrigger);
            isSolidEnvironment = false;
            //origin = Vector2.Zero;
        }

        public void EnterArea()
        {
            //if (eventTrigger.triggerState == TriggerState.Triggered) DeactivateTrigger();
            //else ActivateTrigger();
        }

        public override void Update()
        {
            base.Update();

            if (touching)
            {
                ActivateTrigger();
            }
            else
            {
                DeactivateTrigger();
            }
            touching = false;
        }

        public void ActivateTrigger()
        {
            EventTrigger[] tempTrigers = new EventTrigger[triggerList.Count];
            triggerList.CopyTo(tempTrigers);
            foreach (EventTrigger eventTrigger in tempTrigers)
            {
                eventTrigger.ActivateEvent(TriggerState.Triggered);
            }
        }

        public void DeactivateTrigger()
        {
            EventTrigger[] tempTrigers = new EventTrigger[triggerList.Count];
            triggerList.CopyTo(tempTrigers);
            foreach (EventTrigger eventTrigger in tempTrigers)
            {
                eventTrigger.ActivateEvent(TriggerState.Untriggered);
            }
        }

        //public override void Draw(SpriteBatch spriteBatch)
        //{
            

        //}
    }
}
