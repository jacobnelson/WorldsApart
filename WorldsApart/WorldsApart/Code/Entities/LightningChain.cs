using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using WorldsApart.Code.Graphics;
using WorldsApart.Code.Controllers;

using System.Diagnostics;

namespace WorldsApart.Code.Entities
{
    class LightningChain
    {
        public bool setID = false;

        public bool isActive = false;
        public bool defaultActive = false;

        public List<Lightning> lightningList = new List<Lightning>();
        public Vector2 lastVertex = Vector2.Zero;

        public List<Vector2> vertexList = new List<Vector2>();

        public Color color = Color.White;

        public PlayerObjectMode playerObjectMode = PlayerObjectMode.None;

        public LightningChain(Vector2 vertex1, Vector2 vertex2, Color color)
        {
            Lightning lightning = new Lightning(vertex1, vertex2);
            this.color = color;
            lightning.color = color;
            lightningList.Add(lightning);
            lastVertex = vertex2;
            //vertexList.Add(vertex1);
            //vertexList.Add(vertex2);
        }

        public void SetPlayerMode(PlayerObjectMode pi)
        {
            playerObjectMode = pi;
        }

        public void AddVertex(Vector2 vertex)
        {
            Lightning lastLightning = lightningList[lightningList.Count - 1];
            Lightning lightning = new Lightning(lastVertex, vertex);
            if (lastLightning.target2 != null) lightning.target1 = lastLightning.target2;
            lightning.color = color;
            lightningList.Add(lightning);
            lastVertex = vertex;
        }

        public void ConvertEndPointToTarget(Sprite target)
        {
            lightningList[lightningList.Count - 1].target2 = target;
        }

        public void SetActive(bool isActive)
        {
            this.isActive = isActive;
            foreach (Lightning lightning in lightningList)
            {
                lightning.isActive = isActive;
                lightning.redrawCounter = lightning.redrawRate;
            }
        }

        public void Update()
        {
            foreach (Lightning lightning in lightningList)
            {
                lightning.Update();

            }

            if (setID)
                foreach (Lightning lightning in lightningList)
                {
                    foreach (Line line in lightning.boltList)
                    {
                        line.traceID = 99;
                        return;
                    }
                }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Lightning lightning in lightningList) lightning.Draw(spriteBatch);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            foreach (Lightning lightning in lightningList) lightning.Draw(spriteBatch, camera);
        }


    }
}
