using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using WorldsApart.Code.Graphics;

using System.Diagnostics;

namespace WorldsApart.Code.Entities
{
    class LightningChain
    {
        public bool isActive = false;

        public List<Lightning> lightningList = new List<Lightning>();
        public List<Vector2> vertexList = new List<Vector2>();

        public Color color = Color.White;

        public LightningChain(Vector2 vertex1, Vector2 vertex2, Color color)
        {
            Lightning lightning = new Lightning(vertex1, vertex2);
            lightning.color = color;
            lightning.isActive = true;
            lightningList.Add(lightning);
            //vertexList.Add(vertex1);
            //vertexList.Add(vertex2);
        }

        public void AddVertex(Vector2 vertex)
        {
            Lightning lastLightning = lightningList[lightningList.Count - 1];
            Lightning lightning = new Lightning(lastLightning.end, vertex);
            if (lastLightning.target2 != null) lightning.target1 = lastLightning.target2;
            lightning.isActive = true;
            lightningList.Add(lightning);
        }

        public void ConvertEndPointToTarget(Sprite target)
        {
            lightningList[lightningList.Count - 1].target2 = target;
        }

        public void Update()
        {
            if (isActive)
                foreach (Lightning lightning in lightningList)
                {
                    lightning.Update();
                }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
                foreach (Lightning lightning in lightningList) lightning.Draw(spriteBatch);
        }


    }
}
