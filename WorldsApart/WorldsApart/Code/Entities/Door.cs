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
        public OpenState openState = OpenState.Closed;

        Vector2 openPosition = Vector2.Zero;
        Vector2 closePosition = Vector2.Zero;

        public DoorType doorType = DoorType.Opening;

        public int fadeRate = 30;

        public List<MagicTile> tileList = new List<MagicTile>();

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

        public MagicTile AddCoolTile(Texture2D texture)
        {
            if (tileList.Count == 0) return null;

            MagicTile tile = new MagicTile(texture, tileList[0].position); 
            tile.SetPlayerMode(PlayerObjectMode.Two);
            tileList.Add(tile);
            return tile;
        }

        public override void SetPlayerMode(PlayerObjectMode pi)
        {
            base.SetPlayerMode(pi);
            foreach (MagicTile tile in tileList)
            {
                tile.SetPlayerMode(pi);
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
            openState = OpenState.Open;
            am.StartNewAnimation(AnimationType.EaseInOutCubic, position, openPosition, 30);
            AudioManager.doorOpen.Play();
        }

        public void Close()
        {
            openState = OpenState.Closed;
            am.StartNewAnimation(AnimationType.EaseInOutCubic, position, closePosition, 30);
            AudioManager.doorClose.Play();
        }

        public void Appear()
        {
            am.StartFade(fadeRate, alpha, 255);
            AudioManager.doorClose.Play();
            
        }

        public void Disappear()
        {
            am.StartFade(fadeRate, alpha, 0);
            AudioManager.doorOpen.Play();
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
                        foreach (MagicTile tile in tileList)
                        {
                            tile.ActivateEvent(ts);
                        } 
                    }
                    else
                    {
                        Appear();
                        foreach (MagicTile tile in tileList)
                        {
                            tile.ActivateEvent(ts);
                        } 
                    }
                    break;
            }
        }
    }
}
