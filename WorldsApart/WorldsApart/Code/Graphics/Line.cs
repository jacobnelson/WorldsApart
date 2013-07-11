using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using WorldsApart.Code.Controllers;

using System.Diagnostics;

namespace WorldsApart.Code.Graphics
{
    class Line
    {
        public int traceID = 0;

        public Vector2 a;
        public Vector2 b;
        public float thickness;

        public Line() { }
        public Line(Vector2 a, Vector2 b, float thickness = 1)
        {
            this.a = a;
            this.b = b;
            this.thickness = thickness;
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            Vector2 tangent = b - a;
            float rotation = (float)Math.Atan2(tangent.Y, tangent.X);

            const float ImageThickness = 1;
            float thicknessScale = thickness / ImageThickness;

            Vector2 capOrigin = new Vector2(Art.lineEnd.Width, Art.lineEnd.Height / 2f);
            Vector2 middleOrigin = new Vector2(0, Art.lineMiddle.Height / 2f);
            Vector2 middleScale = new Vector2(tangent.Length(), thicknessScale);

            spriteBatch.Draw(Art.lineMiddle, a, null, color, rotation, middleOrigin, middleScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(Art.lineEnd, a, null, color, rotation, capOrigin, thicknessScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(Art.lineEnd, b, null, color, rotation + MathHelper.Pi, capOrigin, thicknessScale, SpriteEffects.None, 0f);
        }

        public void Draw(SpriteBatch spriteBatch, Color color, Camera camera)
        {
            Vector2 cornerTopLeft = Vector2.Zero;
            if (a.X < b.X) cornerTopLeft.X = a.X / 2;
            else cornerTopLeft.X = b.X / 2;
            if (a.Y < b.Y) cornerTopLeft.Y = a.Y / 2;
            else cornerTopLeft.Y = b.Y / 2;

            Vector2 tangent = b - a;

            Rectangle textureArea = new Rectangle((int)cornerTopLeft.X, (int)cornerTopLeft.Y, (int)Math.Abs(tangent.X), (int)Math.Abs(tangent.Y));
            if (textureArea.Intersects(camera.visibleArea))
            {
                Draw(spriteBatch, color);
            }

            //if (traceID == 99)
            //{
            //    Trace.WriteLine("Texture:");
            //    Trace.WriteLine(textureArea.X + ", " + textureArea.Y + ", " + textureArea.Width + ", " + textureArea.Height);
            //    Trace.WriteLine("Camera:");
            //    Trace.WriteLine(camera.visibleArea.X + ", " + camera.visibleArea.Y + ", " + camera.visibleArea.Width + ", " + camera.visibleArea.Height);
            //}

        }
    }
}
