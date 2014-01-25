using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldsApart.Code.Controllers;
using WorldsApart.Code.Graphics;

namespace WorldsApart.Code.Entities
{
    class FlipSwitch : EventObject
    {
        public bool defaultOn = false;
        public bool multiSwitch = false;

        public bool pressureCooker = false;
        public bool touching = false;

        public PointLight light;

        public FlipSwitch(EventTrigger eventTrigger, Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            triggerList.Add(eventTrigger);
            SetAnimationStuff(1, 4, 1, 4, 64, 64, 4, 5);
            isAnimating = false;
            
        }

        public FlipSwitch(EventTrigger eventTrigger, Texture2D texture, Vector2 position, bool isOn) : this(eventTrigger, texture, position)
        {
            if (isOn)
            {
                defaultOn = true;
                LightsOn();
            }
        }

        public override void SetPlayerMode(PlayerObjectMode pi)
        {
            base.SetPlayerMode(pi);
            light.SetPlayerMode(pi);
            if (defaultOn)
            {
                LightsOn();
            }
            else
            {
                LightsOff();
            }
        }

        public void LightsOn()
        {
            //color = Color.Green;
            switch (playerVisible)
            {
                case PlayerObjectMode.One:
                    AudioManager.switchActivate.Play();
                    currentCellCol = 3;
                    light.color = Color.Orange;
                    break;
                case PlayerObjectMode.Two:
                    AudioManager.switchActivate.Play();
                    currentCellCol = 2;
                    light.color = Color.Blue;
                    break;
                case PlayerObjectMode.None:
                    if (!multiSwitch)
                    {
                        AudioManager.switchActivate.Play();
                        currentCellCol = 1;
                        light.color = Color.Green;
                    }
                    else
                    {
                        AudioManager.switchActivate.Play();
                        currentCellCol = 3;
                        light.color = Color.Orange;
                    }
                    break;
            }
            selfIlluminating = true;
            light.visible = true;
        }

        public void LightsOff()
        {
            //color = Color.Red;

            if (!multiSwitch)
            {
                AudioManager.switchActivate.Play();
                currentCellCol = 4;
                selfIlluminating = false;
                light.visible = false;
            }
            else
            {
                AudioManager.switchDeactivate.Play();
                currentCellCol = 2;
                light.color = Color.Blue;
                selfIlluminating = true;
                light.visible = true;
            }
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

            if (triggerState == TriggerState.Untriggered)
            {
                triggerState = TriggerState.Triggered;
                if (defaultOn) LightsOff();
                else LightsOn();
                foreach (EventTrigger eventTrigger in triggerList)
                {
                    eventTrigger.ActivateEvent(TriggerState.Triggered);
                    
                }
            }
            else
            {
                triggerState = TriggerState.Untriggered;
                if (defaultOn) LightsOn();
                else LightsOff();
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
            currentCellCol = 1;
            light.visible = true;
        }

        public void PressureOff()
        {
            foreach (EventTrigger eventTrigger in triggerList)
            {
                eventTrigger.ActivateEvent(TriggerState.Untriggered);
            }
            currentCellCol = 4;
            light.visible = false;
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
