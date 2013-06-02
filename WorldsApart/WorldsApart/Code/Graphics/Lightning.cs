using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldsApart.Code.Controllers;

namespace WorldsApart.Code.Graphics
{
    class Lightning
    {
        public bool isActive = false;
        public bool canDraw = false;
        public List<Line> boltList = new List<Line>();

        public Sprite target1;
        public Sprite target2;

        public Vector2 start = Vector2.Zero;
        public Vector2 end = Vector2.Zero;
        public Vector2 prevStart = Vector2.Zero;
        public Vector2 prevEnd = Vector2.Zero;

        public Color color = Color.White;

        public float redrawCounter = 0;
        public float redrawRate = .05f;

        public Lightning(Vector2 start, Vector2 end)
        {
            SetStart(start);
            SetEnd(end);
        }

        public void SetStart(Vector2 start)
        {
            this.start = start * 2; //x2 for resolution fix
        }
        public void SetEnd(Vector2 end)
        {
            this.end = end * 2; //x2 for resolution fix
        }

        public void Update()
        {
            if (target1 != null) SetStart(target1.position);
            if (target2 != null) SetEnd(target2.position);

            if (isActive)
            {
                redrawCounter += Time.GetSeconds();
                if (redrawCounter >= redrawRate)
                {
                    boltList = CreateBolt(start, end, 1);
                    redrawCounter = 0;
                }
                //boltList = CreateBolt(start, end, 1);
                canDraw = true;
            }
            else canDraw = false;

            //prevStart = start;
            //prevEnd = end;

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            if (canDraw)
            {
                foreach (Line line in boltList)
                {
                    line.Draw(spriteBatch, color);
                }
            }
        }

        protected static List<Line> CreateBolt(Vector2 source, Vector2 dest, float thickness)
        {
            var results = new List<Line>();
            Vector2 tangent = dest - source;
            Vector2 normal = Vector2.Normalize(new Vector2(tangent.Y, -tangent.X));
            float length = tangent.Length();

            List<float> positions = new List<float>();
            positions.Add(0);

            for (int i = 0; i < length / 4; i++)
                positions.Add(Mathness.RandomNumber(0f, 1f));

            positions.Sort();

            const float Sway = 80;
            const float Jaggedness = 1 / Sway;

            Vector2 prevPoint = source;
            float prevDisplacement = 0;
            for (int i = 1; i < positions.Count; i++)
            {
                float pos = positions[i];

                // used to prevent sharp angles by ensuring very close positions also have small perpendicular variation.
                float scale = (length * Jaggedness) * (pos - positions[i - 1]);

                // defines an envelope. Points near the middle of the bolt can be further from the central line.
                float envelope = pos > 0.95f ? 20 * (1 - pos) : 1;

                float displacement = Mathness.RandomNumber(-Sway, Sway);
                displacement -= (displacement - prevDisplacement) * (1 - scale);
                displacement *= envelope;

                Vector2 point = source + pos * tangent + displacement * normal;
                results.Add(new Line(prevPoint, point, thickness));
                prevPoint = point;
                prevDisplacement = displacement;
            }

            results.Add(new Line(prevPoint, dest, thickness));

            return results;
        }
    }
}
