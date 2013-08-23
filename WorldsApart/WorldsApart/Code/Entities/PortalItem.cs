using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldsApart.Code.Controllers;

namespace WorldsApart.Code.Entities
{
    class PortalItem : Collectible
    {
        public GlyphEmitter emitter;

        public PortalItem(EventTrigger eventTrigger, Texture2D texture, Vector2 position)
            : base(eventTrigger, texture, position)
        {
        }

        public override void ActivateEvent(TriggerState triggerState)
        {
            base.ActivateEvent(triggerState);
            if (emitter != null)
            {
                emitter.isActive = false;
            }
        }


    }
}
