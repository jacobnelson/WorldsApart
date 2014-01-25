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
    class MagicTile : EventObject
    {
        public MagicTile(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            selfIlluminating = false;
        }

        public override void Update()
        {
            visible = alpha != 0;

            base.Update();
        }

        public override void ActivateEvent(TriggerState triggerState)
        {
            base.ActivateEvent(triggerState);

            if (triggerState == TriggerState.Triggered)
            {
                //Fade Out
                am.StartFade(30, alpha, 0);
            }
            else
            {
                //Fade in
                am.StartFade(30, alpha, 255);
            }
        }
    }
}
