﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using WorldsApart.Code.Entities;
using WorldsApart.Code.Controllers;
using WorldsApart.Code.Graphics;

namespace WorldsApart.Code.Entities
{
    class LightConsole : EventObject
    {
        public LightConsole linkedConsole;
        public PointLight light;
        public PointLight sleeperLight;

        public bool hasLight = true;

        public LightConsole(Texture2D texture, Vector2 position) : base(texture, position)
        {
        }

        public void LinkConsole(LightConsole console)
        {
            linkedConsole = console;
            console.linkedConsole = this;
        }

        public void AddLight(PointLight light)
        {
            this.light = light;
            light.target = this;
        }

        public override void ActivateEvent(TriggerState triggerState)
        {
            base.ActivateEvent(triggerState);

            if (triggerState == TriggerState.Triggered)
            {
            }
            else
            {
            }
        }

        public void PressConsole(Player player, bool canDo)
        {
            if (hasLight && canDo)
            {
                if (light != null && player.light == null)
                {
                    light.target = player;
                    player.light = light;
                    light = null;
                    if (linkedConsole != null && linkedConsole.light != null && player.sleeperLight == null)
                    {
                        linkedConsole.light.target = player;
                        player.sleeperLight = linkedConsole.light;
                        player.sleeperLight.visible = false;
                        linkedConsole.light = null;
                        linkedConsole.hasLight = false;
                    }
                    else if (linkedConsole == null && player.sleeperLight == null && sleeperLight != null)
                    {
                        player.sleeperLight = sleeperLight;
                        sleeperLight.target = player;
                        sleeperLight = null;
                    }
                }
                hasLight = false;
            }
            else
            {
                if (light == null && player.light != null)
                {
                    light = player.light;
                    player.light = null;
                    light.target = this;
                    if (linkedConsole != null && linkedConsole.light == null && player.sleeperLight != null)
                    {
                        linkedConsole.light = player.sleeperLight;
                        linkedConsole.light.target = linkedConsole;
                        linkedConsole.light.visible = true;
                        player.sleeperLight = null;
                        linkedConsole.hasLight = true;
                    }
                    else if (linkedConsole == null && player.sleeperLight != null)
                    {
                        sleeperLight = player.sleeperLight;
                        player.sleeperLight = null;
                        sleeperLight.target = this;
                    }
                }
                hasLight = true;
            }
        }


    }
}
