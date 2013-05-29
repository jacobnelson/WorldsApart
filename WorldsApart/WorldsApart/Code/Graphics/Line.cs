using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WorldsApart.Code.Graphics
{
    class Line
    {
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
    }
}
