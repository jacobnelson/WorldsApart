using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldsApart.Code.Controllers;

namespace WorldsApart.Code.Entities
{
    class Collectible : TriggerArea
    {
        public Collectible(EventTrigger eventTrigger, Texture2D texture, Vector2 position)
            : base(eventTrigger, texture, position)
        {
            AddEvent(new EventTrigger(null, this));
            //origin = Vector2.Zero;
        }

        public override void ActivateEvent(TriggerState triggerState)
        {
            visible = false;
            triggerList.Clear();
        }
    }
}
