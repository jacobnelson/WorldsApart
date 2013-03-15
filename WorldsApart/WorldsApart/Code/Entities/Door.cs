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
    enum OpenState
    {
        Open,
        Closed
    }

    enum DoorType
    {
        Opening,
        Fading
    }

    class Door : EventObject
    {
        public OpenState state = OpenState.Closed;

        Vector2 openPosition = Vector2.Zero;
        Vector2 closePosition = Vector2.Zero;

        public DoorType doorType = DoorType.Opening;

        public Door(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            isSolidEnvironment = false;
        }

        public Door(Texture2D texture, Vector2 openPosition, Vector2 closePosition, OpenState state = OpenState.Closed)
            : this(texture, closePosition)
        {
            if (state == OpenState.Open) position = openPosition;
            this.openPosition = openPosition;
            this.closePosition = closePosition;
            doorType = DoorType.Opening;
            //hitBox.omitBottom = true; //TODO: Change omit based on direction door is moving

        }

        public Door(Texture2D texture, Vector2 position, OpenState state = OpenState.Closed)
            : this(texture, position)
        {
            doorType = DoorType.Fading;
            if (state == OpenState.Open)
            {
                alpha = 0;
            }
            else
            {
                alpha = 255;
            }
        }

        public override void Update()
        {
            hitBox.SetPosition(position);
            isSolidObject = alpha > 128;
            base.Update();
        }

        public void Open()
        {
            state = OpenState.Open;
            am.StartNewAnimation(AnimationType.EaseInOutCubic, position, openPosition, 30);
        }

        public void Close()
        {
            state = OpenState.Closed;
            am.StartNewAnimation(AnimationType.EaseInOutCubic, position, closePosition, 30);
        }

        public void Appear()
        {
            am.StartFade(30, alpha, 255);
        }

        public void Disappear()
        {
            am.StartFade(30, alpha, 0);
        }

        public override void ActivateEvent(TriggerState ts)
        {

            switch (doorType)
            {
                case DoorType.Opening:
                    if (ts == TriggerState.Triggered)
                    {
                        Open();
                    }
                    else
                    {
                        Close();
                    }
                    break;
                case DoorType.Fading:
                    
                    if (ts == TriggerState.Triggered)
                    {
                        Disappear();
                    }
                    else
                    {
                        Appear();
                    }
                    break;
            }
        }
    }
}
