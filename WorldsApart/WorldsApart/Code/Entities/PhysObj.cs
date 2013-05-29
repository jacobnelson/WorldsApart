using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using WorldsApart.Code.Graphics;
using WorldsApart.Code.Controllers;
using WorldsApart.Code.Levels;

using System.Diagnostics;

namespace WorldsApart.Code.Entities
{
    enum PhysState
    {
        Grounded,
        Air
    }

    class PhysObj : AnimatedSprite
    {
        public override Vector2 position
        {
            get
            {
                return sPosition;
            }
            set
            { 
                sPosition = value;
            }
        }
        public float rotationSpeed = 0;
        public float mass = 1;
        public float currentMass = 1;
        public Vector2 gravity = Vector2.Zero;
        public Vector2 force = Vector2.Zero;
        public Vector2 acceleration = Vector2.Zero;
        public Vector2 speed = Vector2.Zero;
        public Vector2 terminalSpeed = new Vector2(10, 12);
        public float frictionMultiplier = .9f;
        public float moveForce = .1f;
        public Vector2 nextForce = Vector2.Zero;


        public Vector2 checkpoint = Vector2.Zero;

        public bool redReset = false; //TODO: get rid of these
        public int traceID = 0; //TODO: this too

        public Vector2 tempPosition = Vector2.Zero;

        public bool movingRight = false;
        public bool movingLeft = false;
        public bool movingUp = false;
        public bool movingDown = false;

        public bool canBounce = false;
        public float bounceMultiplier = .5f;

        public PhysState state = PhysState.Air;

        public CollisionBox hitBox;
        public float halfWidth = 0;
        public float halfHeight = 0;

        public bool isSolidEnvironment = true;
        public bool isSolidObject = true;
        public bool ignoreOneWay = false;

        public bool affectsResident = true;
        public List<PhysObj> residentList = new List<PhysObj>();

        public bool psyHold = false;

        public PhysObj(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            if (texture != null)
            {
                halfWidth = texture.Width / 4;
                halfHeight = texture.Height / 4;
                hitBox = new CollisionBox(this, new Vector2(texture.Width / 2, texture.Height / 2));
                hitBox.SetPosition(position);
                checkpoint = position;
            }
        }

        public override void SetAnimationStuff(int _minRow, int _minCol, int _rows, int _cols, int _cellW, int _cellH, int _frames, int _animationRate)
        {
            base.SetAnimationStuff(_minRow, _minCol, _rows, _cols, _cellW, _cellH, _frames, _animationRate);
            halfWidth = cellW / 4;
            halfHeight = cellH / 4;
            SetCollisionBox(cellW / 2, cellH / 2, Vector2.Zero);
        }

        public void SetCollisionBox(float width, float height, Vector2 offset)
        {
            hitBox = new CollisionBox(this, new Vector2(width, height));
            hitBox.SetOffset(offset);
            hitBox.SetPosition(position);
        }

        public override void Update()
        {
            if (psyHold)
            {
                if (auraScalingUp)
                {
                    auraScale += auraIncrement;
                    if (auraScale >= auraScaleMax)
                    {
                        auraScalingUp = false;
                    }
                }
                else
                {
                    auraScale -= auraIncrement;
                    if (auraScale <= auraScaleMin)
                    {
                        auraScalingUp = true;
                    }
                }
                selfIlluminating = true;
            }
            else selfIlluminating = false;

            Vector2 prevPosition = position;
            //if (state == PhysState.Air) force += gravity;
            force += nextForce;
            nextForce = Vector2.Zero;
            force += gravity;
            acceleration += force;
            speed += acceleration;

            if (speed.X > terminalSpeed.X) speed.X = terminalSpeed.X;
            if (speed.X < -terminalSpeed.X) speed.X = -terminalSpeed.X;
            if (speed.Y > terminalSpeed.Y) speed.Y = terminalSpeed.Y;
            if (speed.Y < -terminalSpeed.Y) speed.Y = -terminalSpeed.Y;

            tempPosition = position + speed;
            CollisionEnvironmentResponse();
            position = tempPosition;
            rotation += rotationSpeed;
            //hitBox.SetPosition(position);

            CheckBounds();

            acceleration = Vector2.Zero;
            force = Vector2.Zero;
            currentMass = mass;
            base.Update();

            Vector2 dPos = position - prevPosition;
            foreach (PhysObj resident in residentList)
            {
                resident.position += dPos;
                resident.hitBox.SetPosition(resident.position);
            }
            residentList.Clear();

            //psyHold = false; //TODO: psyhold placeholder
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
                
        //    }
        //}

        protected void SlowX()
        {
            if (speed.X < -.1F || speed.X > .1F)
            {
                speed.X *= frictionMultiplier;
            }
            else
            {
                speed.X = 0;
            }
        }

        public virtual void Die()
        {
            position = new Vector2(checkpoint.X, checkpoint.Y);
            hitBox.SetPosition(position);
            speed = Vector2.Zero;
            nextForce = Vector2.Zero;
        }

        public virtual void CheckBounds()
        {
            if (position.Y > Level.deathHeight)
            {
                Die();
            }
        }

        public virtual bool CheckCollision(PhysObj obj)
        {
            return false;
        }

        public virtual void CollisionEnvironmentResponse()
        {
            if (!isSolidEnvironment) return;
            state = PhysState.Air;
            hitBox.SetPosition(tempPosition);
            Level.CheckEnvironment(this);
        }

        public virtual void AdjustCollision(PhysObj obj)
        {
            
            if (playerTangible != PlayerObjectMode.None && obj.playerTangible != PlayerObjectMode.None)
            {
                if (playerTangible != obj.playerTangible) return;
            }
            if (!obj.isSolidObject || !hitBox.CheckCollision(obj.hitBox))
            {
                return;
            }
            AABB aabb = obj.hitBox.GetAABB();
            Vector2 solution = aabb.SolveCollision(hitBox.GetAABB());
            hitBox.SetPosition(hitBox.GetPosition() + solution);

            Vector2 newpos = hitBox.GetPosition();
            if (newpos.Y != position.Y) speed.Y = Level.SolveBounce(this, true);
            if (newpos.X != position.X) speed.X = Level.SolveBounce(this, false);
            if (newpos.Y < position.Y)
            {
                state = PhysState.Grounded;
                frictionMultiplier = EnvironmentData.NORMAL;
                obj.currentMass += currentMass;
                if (obj.affectsResident && !obj.residentList.Contains(this)) obj.residentList.Add(this);
            }
            if (newpos.Y > position.Y)
            {
                speed.Y = obj.speed.Y;
                if (speed.Y < 0) speed.Y = 0;
            }
            Vector2 prevPosition = position;
            position = newpos;

            Vector2 dPos = position - prevPosition; 
            foreach (PhysObj resident in residentList)
            {
                resident.position += dPos;
                resident.hitBox.SetPosition(resident.position);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }



    }
}
