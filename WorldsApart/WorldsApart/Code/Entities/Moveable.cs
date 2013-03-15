using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldsApart.Code.Controllers;
using WorldsApart.Code.Levels;

namespace WorldsApart.Code.Entities
{
    class Moveable : EventObject
    {
        public float moveModifier = .5f;
        public bool moveCollided = false;
        public Player parent;

        public Moveable(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            gravity = new Vector2(0, .5f);
            hitBox.omitBottom = true;
        }

        public override void Update()
        {
            SlowX();
            if (parent != null)
            {
                psyHold = true;
                auraColor = parent.auraColor;
                speed.X = parent.speed.X;
                if (speed.X > 0) speed.X += .01f;
                else speed.X -= .01f;
            }
            else
            {
                psyHold = false;
            }
            base.Update();
            moveCollided = false;
            
        }

        public void AdjustToPlayer(Player player)
        {
            if (playerTangible != PlayerObjectMode.None && player.playerTangible != PlayerObjectMode.None)
            {
                if (playerTangible != player.playerTangible) return;
            }
            if (!player.isSolidObject || !hitBox.CheckCollision(player.hitBox)) return;
            AABB aabb = player.hitBox.GetAABB();
            Vector2 solution = aabb.SolveCollision(hitBox.GetAABB());
            solution.Y = 0;
            hitBox.SetPosition(hitBox.GetPosition() + solution);

            Vector2 newpos = hitBox.GetPosition();
            if (newpos.Y != position.Y) speed.Y = Level.SolveBounce(this, true);
            if (newpos.X != position.X) speed.X = Level.SolveBounce(this, false);
            if (newpos.Y < position.Y)
            {
                state = PhysState.Grounded;
                frictionMultiplier = EnvironmentData.NORMAL;
                player.currentMass += currentMass;
                if (player.affectsResident) player.residentList.Add(this);

            }

            position = newpos;
        }

        public void AdjustToMoveable(Moveable move)
        {
            if (playerTangible != PlayerObjectMode.None && move.playerTangible != PlayerObjectMode.None)
            {
                if (playerTangible != move.playerTangible) return;
            }
            //moveCollided = true;
            //if (move.moveCollided) return;
            float pushX = 0;
            float movePushX = 0;
            if (!hitBox.CheckCollision(move.hitBox)) return;
            pushX = speed.X * move.moveModifier;
            movePushX = move.speed.X * moveModifier;
            AABB aabb = move.hitBox.GetAABB();
            Vector2 solution = aabb.SolveCollision(hitBox.GetAABB());
            solution /= 2;
            hitBox.SetPosition(hitBox.GetPosition() + solution);
            move.hitBox.SetPosition(move.hitBox.GetPosition() - solution);

            AdjustPositions(move, hitBox.GetPosition());
            move.AdjustPositions(this, move.hitBox.GetPosition());
            if (speed.X == 0)
            {
                speed.X = pushX;
                move.speed.X = movePushX;
            }

        }

        public void AdjustPositions(Moveable move, Vector2 newpos)
        {
            if (newpos.Y != position.Y) speed.Y = Level.SolveBounce(this, true);
            if (newpos.X != position.X) speed.X = Level.SolveBounce(this, false);
            if (newpos.Y < position.Y)
            {
                state = PhysState.Grounded;
                frictionMultiplier = EnvironmentData.NORMAL;
                move.currentMass += currentMass;
                if (move.affectsResident && !move.residentList.Contains(this)) move.residentList.Add(this);
            }

            Vector2 prevPosition = position;
            position = newpos;
            hitBox.SetPosition(position);

            Vector2 dPos = position - prevPosition;
            foreach (PhysObj resident in residentList)
            {
                resident.position += dPos;
                resident.hitBox.SetPosition(resident.position);
            }
            

        }
    }
}
