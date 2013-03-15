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
    enum PlatformMode
    {
        PressToActivate,
        PressToReverse
    }

    class MovingPlatform : EventObject
    {
        Vector2 startPosition = Vector2.Zero;
        Vector2 endPosition = Vector2.Zero;
        public int duration = 240;
        AnimationType type = AnimationType.EaseInOutQuad;

        public PlatformMode platformerMode = PlatformMode.PressToReverse;


        //List<PhysObj> residentList = new List<PhysObj>();

        public bool looping = true;
        public bool waitingEvent = false;
        public bool moving = true;

        public MovingPlatform(Texture2D texture, Vector2 startPosition, Vector2 endPosition)
            : base(texture, startPosition)
        {
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            isSolidEnvironment = false;
        }

        public override void Update()
        {
            //Vector2 prevPosition = position;
            if (moving) base.Update();
            hitBox.SetPosition(position);
            if (looping)
            {
                if (!am.animating)
                {
                    if (position == startPosition) am.StartNewAnimation(type, position, endPosition, duration);
                    else if (position == endPosition) am.StartNewAnimation(type, position, startPosition, duration);
                }
            }

            //Vector2 dPos = position - prevPosition;
            //foreach (PhysObj resident in residentList)
            //{
            //    resident.position += dPos;
            //    resident.hitBox.SetPosition(resident.position);
            //}
            //residentList.Clear();
        }

        //public void CheckForResident(PhysObj obj)
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

        public override void ActivateEvent(TriggerState ts)
        {

            if (platformerMode == PlatformMode.PressToReverse)
            {
                int currentTime = duration;
                if (am.animating) currentTime = am.animateTime;
                if (ts == TriggerState.Triggered)
                {
                    am.StartNewAnimation(type, startPosition, endPosition, duration);
                    am.animateTime = duration - currentTime;
                }
                else
                {
                    am.StartNewAnimation(type, endPosition, startPosition, duration);
                    am.animateTime = duration - currentTime;
                }
            }
            else if (platformerMode == PlatformMode.PressToActivate)
            {
                if (ts == TriggerState.Triggered)
                {
                    moving = true;
                }
                else
                {
                    moving = false;
                }
            }
        }


    }
}
