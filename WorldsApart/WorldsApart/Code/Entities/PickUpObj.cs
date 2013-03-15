using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace WorldsApart.Code.Entities
{
    class PickUpObj : PhysObj
    {

        PhysObj parent;
        Vector2 pickUpOffset = Vector2.Zero;

        float angle = 0;
        float radius = 10;

        public PickUpObj(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            gravity = new Vector2(0, .5f);
        }

        public PickUpObj(float bounceMultiplier, Texture2D texture, Vector2 position)
            : this(texture, position)
        {
            canBounce = true;
            this.bounceMultiplier = bounceMultiplier;
        }

        public override void Update()
        {
            
            SlowX();
            base.Update();
            if (parent != null)
            {
                if (!hitBox.CheckCollision(parent.hitBox))
                {
                    GetDropped(Vector2.Zero);
                }
                else
                {
                    
                    float x = (float)Math.Sin(angle) * radius * 2.5f;
                    float y = (float)Math.Sin(angle*2) * radius * 1;
                    angle += .05f;
                    if (angle >=  2 *Math.PI) angle -= 2 * (float)Math.PI;

                    pickUpOffset = new Vector2(x, y - 16);
                    Vector2 targetPosition = parent.position + pickUpOffset;
                    position = targetPosition;
                    hitBox.SetPosition(position);
                    speed = parent.speed;
                    auraColor = parent.auraColor;
                    psyHold = true;
                    //TODO: movement code
                }

            }
            else psyHold = false;
            
        }

        public void GetPickedUp(Player parent)
        {
            if (this.parent == null)
            {
                this.parent = parent;
                position = parent.position;
                parent.pickUp = this;
            }
        }

        public void GetDropped(Vector2 throwForce)
        {
            nextForce += throwForce;
            parent = null;
        }


    }
}
