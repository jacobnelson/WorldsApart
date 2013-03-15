using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WorldsApart.Code.Levels;
using WorldsApart.Code.Entities;

using System.Diagnostics;

namespace WorldsApart.Code.Controllers
{
    enum TriggerState
    {
        Triggered,
        Untriggered
    }

    class EventTrigger
    {
        public int eventID = -1;
        Level level;
        //EventObject eventObj;
        public List<EventObject> eventObjList = new List<EventObject>();
        public TriggerState triggerState = TriggerState.Untriggered;

        public EventTrigger(Level level)
        {
            this.level = level;
        }

        public EventTrigger(Level level, EventObject eventObj)
            : this(level)
        {
            eventObjList.Add(eventObj);
        }

        public EventTrigger(Level level, int eventID)
            : this(level)
        {
            this.eventID = eventID;
        }

        public void ActivateEvent()
        {
            //this.triggerState = triggerState;

            foreach (EventObject eventObj in eventObjList)
            {
                if (eventObj.triggerState == TriggerState.Triggered)
                    eventObj.ActivateEvent(TriggerState.Untriggered);
                else
                    eventObj.ActivateEvent(TriggerState.Triggered);
                //if (eventObj == null)
                //    level.ActivateEvent(eventID, triggerState);
                //else
                //    eventObj.ActivateEvent(triggerState);
            }

            if (eventID != -1)
            {
                if (triggerState == TriggerState.Triggered)
                {
                    triggerState = TriggerState.Untriggered;
                    level.ActivateEvent(eventID, triggerState);
                }
                else
                {
                    triggerState = TriggerState.Triggered;
                    level.ActivateEvent(eventID, triggerState);
                }
                
            }
        }

        public void ActivateEvent(TriggerState ts)
        {
            foreach (EventObject eventObj in eventObjList)
            {
                if (eventObj.triggerState != ts)
                {
                    eventObj.triggerState = ts;
                    eventObj.ActivateEvent(ts);
                }
            }
            if (eventID != -1)
            {
                triggerState = ts;
                level.ActivateEvent(eventID, ts);
            }
        }
    }
}
