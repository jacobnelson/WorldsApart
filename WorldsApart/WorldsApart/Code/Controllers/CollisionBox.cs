using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using WorldsApart.Code.Entities;

namespace WorldsApart.Code.Controllers
{
    struct AABB
    {
        public float x1;
        public float x2;
        public float y1;
        public float y2;

        public bool omitTop;
        public bool omitLeft;
        public bool omitRight;
        public bool omitBottom;
        public PhysObj physObj;

        public bool incline;

        public AABB(PhysObj obj, Vector2 Position, Vector2 Size)
            : this(obj, Position.X, Position.Y, Position.X + Size.X, Position.Y + Size.Y)
        {
        }
        public AABB(PhysObj obj, float x1, float y1, float x2, float y2)
        {
            this.physObj = obj;
            this.x1 = x1;
            this.x2 = x2;
            this.y1 = y1;
            this.y2 = y2;
            omitTop = false;
            omitLeft = false;
            omitRight = false;
            omitBottom = false;
            incline = false;
        }

        public bool CheckAABBCollision(AABB rect)
        {
            //return !(rect.y1 > y2 || rect.x1 > x2 || rect.x2 < x1 || rect.y2 < y1);
            if (rect.y1 > y2) return false;
            if (rect.x1 > x2) return false;
            if (rect.x2 < x1) return false;
            if (rect.y2 < y1) return false;
            return true;
        }

        public Vector2 SolveCollision(AABB rect)
        {

            // If the two AABBs don't collide, do nothing:
            //if (rect.y1 > y2) return Vector2.Zero;
            //if (rect.x1 > x2) return Vector2.Zero;
            //if (rect.x2 < x1) return Vector2.Zero;
            //if (rect.y2 < y1) return Vector2.Zero;
            if (!CheckAABBCollision(rect)) return Vector2.Zero;

            float Y1 = y1 - rect.y2; // distance to move object up
            float X1 = x1 - rect.x2; // distance to move object left
            float X2 = x2 - rect.x1; // distance to move object right
            float Y2 = y2 - rect.y1; // distance to move object down

            float aX1 = Math.Abs(X1);
            float aX2 = Math.Abs(X2);
            float aY1 = Math.Abs(Y1);
            float aY2 = Math.Abs(Y2);

            /*
            // Allow player to go up stairs!!
            if (!omitTop && (!omitLeft || !omitRight) && aY1 < Level.collisionSize.Y * 1.5) {
                if (rect.physObj != null) {
                    Line path = rect.physObj.GetMovementPath();
                    if (Math.Abs(path.nx) >= .25) {
                        return new Vector2(0, Y1);
                    }
                }
            }
            */

            if (incline)
            {
                if (aY1 <= aX1 && aY1 <= aX2 && aY1 <= aY2) return omitTop ? Vector2.Zero : new Vector2(0, Y1); // move up
                if (aX1 <= aY1 && aX1 <= aY2 && aX1 <= aX2) return omitLeft ? Vector2.Zero : new Vector2(X1, 0); // move left
                if (aX2 <= aY1 && aX2 <= aY2 && aX2 <= aX1) return omitRight ? Vector2.Zero : new Vector2(X2, 0); // more right
                if (aY2 <= aX1 && aY2 <= aX2 && aY2 <= aY1) return omitBottom ? Vector2.Zero : new Vector2(0, Y2); // move down
            }
            else
            {
                if (aY1 <= aX1 && aY1 <= aX2 && aY1 <= aY2) if (!omitTop) return new Vector2(0, Y1); // move up
                if (aX1 <= aY1 && aX1 <= aY2 && aX1 <= aX2) if (!omitLeft) return new Vector2(X1, 0); // move left
                if (aX2 <= aY1 && aX2 <= aY2 && aX2 <= aX1) if (!omitRight) return new Vector2(X2, 0); // more right
                if (aY2 <= aX1 && aY2 <= aX2 && aY2 <= aY1) if (!omitBottom) return new Vector2(0, Y2); // move down
            }

            return Vector2.Zero;
        }
    }

    class CollisionBox
    {

        public PhysObj physObj;

        public Vector2 WorldCornerMin;
        public Vector2 WorldCornerMax;

        Vector2 Offset;
        Vector2 Position;
        public Vector2 Size;

        Vector2 LocalCornerMin;
        Vector2 LocalCornerMax;

        public bool omitTop = false;
        public bool omitBottom = false;
        public bool omitRight = false;
        public bool omitLeft = false;

        public CollisionBox(PhysObj physObj, Vector2 Size)
        {

            this.physObj = physObj;

            WorldCornerMin = Vector2.Zero;
            WorldCornerMax = Vector2.Zero;
            LocalCornerMin = Vector2.Zero;
            LocalCornerMax = Vector2.Zero;
            Position = Vector2.Zero;
            Offset = Vector2.Zero;
            this.Size = Size;

            CalculateCorners();
        }
        public CollisionBox(PhysObj physObj, float Size) : this(physObj, new Vector2(Size, Size)) { }

        public void SetSize(Vector2 Size)
        {
            this.Size = Size;
            CalculateCorners();
        }
        public void SetOffset(Vector2 Offset)
        {
            this.Offset = Offset;
            CalculateCorners();
        }
        public void SetPosition(Vector2 Position)
        {
            this.Position = Position;
            CalculateSides();
        }
        private void CalculateCorners()
        {
            LocalCornerMax = Offset + Size / 2;
            LocalCornerMin = Offset - Size / 2;
            CalculateSides();
        }
        private void CalculateSides()
        {
            WorldCornerMin = LocalCornerMin + Position;
            WorldCornerMax = LocalCornerMax + Position;
        }
        public float Top() { return WorldCornerMin.Y; }
        public float Left() { return WorldCornerMin.X; }
        public float Right() { return WorldCornerMax.X; }
        public float Bottom() { return WorldCornerMax.Y; }
        public Vector2 GetPosition() { return Position; }
        public AABB GetAABB()
        {
            AABB aabb = new AABB(physObj, WorldCornerMin, Size);
            aabb.omitTop = omitTop;
            aabb.omitBottom = omitBottom;
            aabb.omitRight = omitRight;
            aabb.omitLeft = omitLeft;
            return aabb;
        }

        public bool CheckCollision(CollisionBox box)
        {
            return GetAABB().CheckAABBCollision(box.GetAABB());
        }
    }
}
