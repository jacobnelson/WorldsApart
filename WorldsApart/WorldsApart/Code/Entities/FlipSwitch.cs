using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldsApart.Code.Controllers;

namespace WorldsApart.Code.Entities
{
    class FlipSwitch : EventObject
    {
        public bool pressureCooker = false;
        public bool touching = false;

        public FlipSwitch(EventTrigger eventTrigger, Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            triggerList.Add(eventTrigger);
            SetAnimationStuff(1, 1, 1, 2, texture.Width / 2, texture.Height, 1, 5);
            isAnimating = false;
        }

        public override void Update()
        {
            base.Update();

            if (pressureCooker)
            {
                if (touching)
                {
                    PressureOn();
                }
                else
                {
                    PressureOff();
                }
                touching = false;
            }

            
        }

        public void PressSwitch()
        {
            if (onlyTriggered && triggerState == TriggerState.Triggered) return;

            if (currentCellCol == 1)
            {
                currentCellCol = 2;
                foreach (EventTrigger eventTrigger in triggerList)
                {
                    eventTrigger.ActivateEvent(TriggerState.Triggered);
                    
                }
            }
            else
            {
                currentCellCol = 1;
                foreach (EventTrigger eventTrigger in triggerList)
                {
                    eventTrigger.ActivateEvent(TriggerState.Untriggered);
                }
            }

            

            
            //if (currentCellCol == 1) currentCellCol = 2;
            //else currentCellCol = 1;
        }

        public void PressureOn()
        {
            foreach (EventTrigger eventTrigger in triggerList)
            {
                eventTrigger.ActivateEvent(TriggerState.Triggered);
            }
            currentCellCol = 2;
        }

        public void PressureOff()
        {
            foreach (EventTrigger eventTrigger in triggerList)
            {
                eventTrigger.ActivateEvent(TriggerState.Untriggered);
            }
            currentCellCol = 1;
        }

        //public void SwitchOn()
        //{
        //    currentCellCol = 2;
        //    eventTrigger.ActivateEvent(TriggerState.Triggered);
        //}

        //public void SwitchOff()
        //{
        //    currentCellCol = 1;
        //    eventTrigger.ActivateEvent(TriggerState.Untriggered);
        //}
    }
}
