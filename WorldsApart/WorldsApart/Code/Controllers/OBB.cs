using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using WorldsApart.Code.Graphics;

namespace WorldsApart.Code.Controllers
{
    class OBB
    {
        public AnimatedSprite image;

        public Texture2D testTexture; 

        Matrix matrix = Matrix.Identity;

        public Vector2 position = new Vector2();
        public float rotation = 0;
        public float speedRotation = 0;

        //float halfW = 0;
        //float halfH = 0;

        List<Vector2> cpList = new List<Vector2>();
        List<Vector2> tpList = new List<Vector2>();
        List<Vector2> nList = new List<Vector2>();

        bool colliding = false;

        public OBB(Texture2D testTexture, AnimatedSprite image, Vector2 position)
        {
            this.image = image;
            this.position = position;
            this.testTexture = testTexture;
        }

        public void AddVertex(Vector2 cp)
        {
            cpList.Add(cp);
        }

        public void Update()
        {
            image.Update();

            rotation += speedRotation;
            
            Matrix translateMatrix = Matrix.CreateTranslation(position.X, position.Y, 0);
            Matrix rotationMatrix = Matrix.CreateRotationZ(rotation);
            matrix = translateMatrix * rotationMatrix;

            tpList.Clear();
            foreach (Vector2 cp in cpList)
            {
                Vector2 tp = new Vector2();
                tp = Vector2.Transform(cp, matrix);
              //matrix.mult(cp, tp);
              tpList.Add(tp);
            }

            //matrix.mult(new PVector(-halfW, -halfH), tp1);
            //matrix.mult(new PVector(halfW, -halfH), tp2);
            //matrix.mult(new PVector(halfW, halfH), tp3);
            //matrix.mult(new PVector(-halfW, halfH), tp4);

            nList.Clear();

            for (int i = 0; i < tpList.Count; i++)
            {
              if (i + 1 == tpList.Count)
              {
                nList.Add(new Vector2(tpList[i].Y - tpList[0].Y, tpList[0].X - tpList[i].X));
              }
              else
              {
                  nList.Add(new Vector2(tpList[i].Y - tpList[i + 1].Y, tpList[i + 1].X - tpList[i].X));
              }
            }

    //for (PVector n : nList)
    //{
    //  n.normalize();
    //}

    //n1 = new PVector(tp1.y - tp2.y, tp2.x - tp1.x);
    //n2 = new PVector(tp2.y - tp3.y, tp3.x - tp2.x);
    //n3 = new PVector(tp3.y - tp4.y, tp4.x - tp3.x);
    //n4 = new PVector(tp4.y - tp1.y, tp1.x - tp4.x);
  }

        public void CheckCollision(OBB shape)
          {
            List<bool> chkList = new List<bool>();
            foreach (Vector2 n in nList)
            {
              chkList.Add(CheckCollisionAlongAxis(n, shape));
            }
            foreach (Vector2 n in shape.nList)
            {
              chkList.Add(shape.CheckCollisionAlongAxis(n, this));
            }

            bool check = true;
            foreach (bool chk in chkList)
            {
              if (!chk) check = false;
            }
            if (check)
            {
              colliding = true;
              shape.colliding = true;
            }
          }

          bool CheckCollisionAlongAxis(Vector2 axis, OBB shape)
          {
            MinMax mm1 = ProjectAlongAxis(axis);
            MinMax mm2 = shape.ProjectAlongAxis(axis);


            //DEBUG DRAW CODE
            //stroke(255);
            //PVector pp = PVector.mult(axis, mm1.center - mm2.center);
            //pp.add(box2.position);
            //line(shape.position.x, shape.position.y, pp.x, pp.y);
            //END

            if (mm2.min > mm1.max || mm1.min > mm2.max) return false;
            return true;
          }

          public void Draw(SpriteBatch spriteBatch)
          {
              

              if (image != null)
              {
                  if (colliding)
                  {
                      image.currentCellCol = 2;
                  }
                  else
                  {
                      image.currentCellCol = 1;
                  }
                  colliding = false;

                  spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, matrix);

                  image.Draw(spriteBatch);

                  spriteBatch.End();
              }

              //spriteBatch.Begin();
              //foreach (Vector2 tp in tpList)
              //{
              //    spriteBatch.Draw(testTexture, tp, Color.White);
              //}
              //spriteBatch.End();
            //if (colliding) fill(255, 0, 0);
            //else fill(255);
            //colliding = false;

            //noStroke();
            //beginShape();
            //for (PVector tp : tpList)
            //{
            //  vertex(tp.x, tp.y);
            //}
            //endShape();
          }

          MinMax ProjectAlongAxis(Vector2 axis)
          {
            MinMax mm = new MinMax();

            List<float> dList = new List<float>();
            foreach (Vector2 tp in tpList)
            {
              //dList.Add(Vector3.Dot(new Vector3(tp.X, tp.Y, 0), new Vector3(axis.X, axis.Y, 0)));
                dList.Add(tp.X * axis.X + tp.Y * axis.Y);
              
            }

            mm.min = dList[0];
            mm.max = dList[0];
            //mm.center = position.dot(axis);
            //mm.center = Vector3.Dot(new Vector3(position.X, position.Y, 0), new Vector3(axis.X, axis.Y, 0));
            mm.center = position.X * axis.X + position.Y * axis.Y;

            foreach (float d in dList)
            {
              if (d < mm.min) mm.min = d;
              if (d > mm.max) mm.max = d;
            }
            return mm;
          }


    }

class MinMax 
{
  public float min = 0;
  public float max = 0;
  public float center = 0;
}
}
