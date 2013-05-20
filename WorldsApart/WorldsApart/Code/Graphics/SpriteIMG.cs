using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using WorldsApart.Code.Controllers;

namespace WorldsApart.Code.Graphics
{
    class SpriteIMG : Sprite
    {
        public Texture2D texture;
        public Texture2D auraTexture;
        public Color auraColor = Color.Red;
        public float auraScale = 1.1f;
        public float auraScaleMin = 1.1f;
        public float auraScaleMax = 1.3f;
        public bool auraScalingUp = true;

        public bool selfIlluminating = false;
        public bool illuminatingAllTheTime = false;

        public SpriteIMG(Texture2D texture, Vector2 position)
            : base(position)
        {
            if (texture != null)
            {
                this.texture = texture;
                this.crop = texture.Bounds;
                this.origin = new Vector2(texture.Width / 2, texture.Height / 2); //Automatically set origin to center of object
            }
        }

        public SpriteIMG(Texture2D texture)
            : this(texture, Vector2.Zero)
        {
        }

        public override void Update()
        {
            base.Update();
            if (illuminatingAllTheTime)
            {
                selfIlluminating = true;
            }
        }

        public virtual void DrawAura(SpriteBatch spriteBatch, Vector2 screenOrigin)
        {
            if (texture != null)
                spriteBatch.Draw(texture, screenOrigin, crop, color, rotation, origin, auraScale, spriteEffects, 0);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!visible) return;
            base.Draw(spriteBatch);
            if (auraTexture != null) spriteBatch.Draw(auraTexture, position * 2, Color.White);
            if (texture != null) spriteBatch.Draw(texture, sPosition * 2, crop, color, rotation, origin, scale, spriteEffects, 0);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Rectangle textureArea = new Rectangle((int)position.X - texture.Width / 2, (int)position.Y - texture.Width / 2, texture.Width, texture.Height);

            if (textureArea.Intersects(camera.visibleArea))
            {
                Draw(spriteBatch);
            }

            //if (position.X * 2 - texture.Width > camera.visibleArea.Right || position.X * 2 + texture.Width < camera.visibleArea.Left || position.Y * 2 - texture.Height > camera.visibleArea.Bottom || position.Y * 2 + texture.Height < camera.visibleArea.Top)
            //{
            //    return;
            //}
            //else
            //{
            //    Draw(spriteBatch);
            //}
        }
    }
}
