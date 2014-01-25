using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldsApart.Code.Controllers;
using WorldsApart.Code.Graphics;

using System.Diagnostics;

namespace WorldsApart.Code.Entities
{
    class Button : EventObject
    {
        //List<PhysObj> residentList = new List<PhysObj>();

        //bool pressingDown = false;
        bool raisingUp = false;

        Vector2 unpressedScale = Vector2.One;
        Vector2 pressedScale = new Vector2(1, 0.4375f);

        public float neededMass = 1;
        public float heldMass = 0;

        public SpriteIMG bBase;

        public Button(EventTrigger eventTrigger, Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            AddEvent(eventTrigger);
            isSolidEnvironment = false;
            origin = new Vector2(texture.Width / 2, texture.Height);
        }

        public override void Update()
        {
            float totalMass = 0;
            foreach (PhysObj resident in residentList)
            {
                totalMass += resident.currentMass;
            }
            
            if (residentList.Count != 0)
            {
                if (totalMass > neededMass) totalMass = neededMass;
                Vector2 dScale = unpressedScale - pressedScale;
                float ratio = totalMass / neededMass;
                raisingUp = false;
                if (heldMass != totalMass)
                {
                    am.StartScale(60, scale, unpressedScale - (dScale * ratio));
                }

            }
            else
            {
                if (!raisingUp)
                {
                    raisingUp = true;
                    am.StartScale(60, scale, unpressedScale);
                }

            }
            heldMass = totalMass;
            //residents.Clear();
            float newHalfHeight = halfHeight * scale.Y;
            hitBox.SetSize(new Vector2(halfWidth * 2, newHalfHeight * 2));
            hitBox.SetPosition(new Vector2(position.X, position.Y - newHalfHeight));
            
            
            

            if (scale.Y <= pressedScale.Y)
            {
                foreach (EventTrigger eventTrigger in triggerList)
                {
                    
                    eventTrigger.ActivateEvent(TriggerState.Triggered);
                }
            }
            else
            {
                foreach (EventTrigger eventTrigger in triggerList)
                {
                    eventTrigger.ActivateEvent(TriggerState.Untriggered);
                }
            }

            base.Update();
            
        }

        public override void SetPlayerMode(PlayerObjectMode pi)
        {
            base.SetPlayerMode(pi);
            bBase.SetPlayerMode(pi);
        }

        //public void CheckForPress(PhysObj obj)
        //{
        //    //if (obj.hitBox.Bottom() + 1 <= hitBox.Top() && obj.hitBox.Left() < hitBox.Right() && obj.hitBox.Right() > hitBox.Left())
        //    //{
        //    //    residents.Add(obj);
        //    //}
        //    CollisionBox checkBox = new CollisionBox(null, hitBox.Size - new Vector2(2, 0));
        //    checkBox.SetPosition(hitBox.GetPosition() + new Vector2(0, -1));
        //    if (checkBox.GetAABB().CheckAABBCollision(obj.hitBox.GetAABB()))
        //    {
        //        residentList.Add(obj);
        //    }
        //}


    }
}
