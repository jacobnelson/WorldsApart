using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WorldsApart.Code.Graphics
{
    class ScrollingBackground : SpriteIMG
    {
        public Vector2 screenPosition, textureSize; //These... are the same as the position and crop, basically. But I copied and pasted this code, so whatever. 
        public int screenHeight = Game1.screenHeight; //Height of the viewport
        public int screenWidth = Game1.screenWidth;

        public bool scrollingVertical = true;

        public ScrollingBackground(Texture2D texture, Vector2 position, bool scrollingVertical)
            : base(texture, position)
        {

            this.scrollingVertical = scrollingVertical;
            //screenHeight = h;
            //screenWidth = w;
            if (scrollingVertical)
            {

                origin = new Vector2(texture.Width / 2, 0);
                textureSize = new Vector2(0, texture.Height);
                screenPosition = new Vector2(screenWidth / 2, 0);
            }
            else
            {
                origin = new Vector2(0, texture.Height / 2);
                textureSize = new Vector2(-texture.Width, 0);
                screenPosition = new Vector2(0, screenHeight / 2);
            }

        }

        public ScrollingBackground(Texture2D texture, bool scrollingVertical)
            : this(texture, Vector2.Zero, scrollingVertical)
        {
        }

        public void UpdateScroll(float delta) //This repositions where the background is being drawn, and makes sure that it doesn't keep going
        {
            if (scrollingVertical)
            {
                screenPosition.Y += delta;
                screenPosition.Y = screenPosition.Y % texture.Height;
            }
            else
            {
                screenPosition.X += delta;
                screenPosition.X = screenPosition.X % texture.Width;
            }
        }

        public override void Draw(SpriteBatch theSpriteBatch) //Special draw method for scrollin'
        {
            color.A = alpha;
            if (scrollingVertical)
            {
                if (screenPosition.Y < screenHeight) //If we have part of the image not covered by background, then we draw another background.
                {
                    theSpriteBatch.Draw(texture, screenPosition, null, color, 0, origin, 1, spriteEffects, 0f);
                }
            }
            else
            {
                if (screenPosition.X < screenWidth)
                {
                    theSpriteBatch.Draw(texture, screenPosition, null, color, 0, origin, 1, spriteEffects, 0f);
                }
            }
            //Then we draw the background original, too. 
            theSpriteBatch.Draw(texture, screenPosition - textureSize, null, color, 0, origin, 1, SpriteEffects.None, 0f);
        }
    }
}