using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WorldsApart.Code.Graphics
{
    class AnimatedSprite : SpriteIMG
    {
        public int animationRate = 1; //This is how fast the animation will trigger
        public int animationCounter = 0; //This keeps track of the life of each frame

        public bool isAnimating = false;

        //Now we create a matrix of spritely goodness! 
        public int minRow = 1; //This is the starting row of the current animation
        public int minCol = 1; //This is the starting col of the current animation
        public int frames = 1;
        public int frameCounter = 0;
        public int cols = 1; //This is how many cols there are overall
        public int rows = 1; //How many rows there are overall
        public int cellW = 0; //The width of each cell
        public int cellH = 0; //The height of each cell

        public int currentCellRow = 1; //The current row
        public int currentCellCol = 1; //The current column

        public AnimatedSprite(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            //By default, the cell size is the size of the texture
            if (texture != null)
            {
                cellW = texture.Width;
                cellH = texture.Height;
            }
        }

        public AnimatedSprite(Texture2D texture)
            : this(texture, Vector2.Zero)
        {
        }

        public virtual void SetAnimationStuff(int _minRow, int _minCol, int _rows, int _cols, int _cellW, int _cellH, int _frames, int _animationRate) //This function sets all the animation values that we need to know
        {
            ChangeAnimationBounds(_minRow, _minCol, _frames);
            rows = _rows;
            cols = _cols;
            cellW = _cellW;
            cellH = _cellH;
            animationRate = _animationRate;
            origin = new Vector2(cellW / 2, cellH / 2);
            isAnimating = true;
        }

        public void ChangeAnimationBounds(int _minRow, int _minCol, int _frames) //This one can change what animation you're on if you have multiple animations on the same sheet
        {
            minRow = _minRow;
            minCol = _minCol;
            frames = _frames;
            currentCellCol = minCol;
            currentCellRow = minRow;
            frameCounter = 0;
            animationCounter = 0;
        }

        public virtual void nextCell() //Goes to the next cell, and loops based on your mins and maxes
        {
            currentCellCol++;
            if (currentCellCol > cols)
            {
                currentCellRow++;
                currentCellCol = 1;
            }
            frameCounter++;
            if (frameCounter >= frames)
            {
                frameCounter = 0;
                currentCellCol = minCol;
                currentCellRow = minRow;
            }
        }

        public override void Update() 
        {
            base.Update();
            Animate();
            int x = cellW * (currentCellCol - 1);
            int y = cellH * (currentCellRow - 1);

            crop = new Rectangle(x, y, cellW, cellH); 
        }

        public void Animate()
        {
            if (isAnimating)
            {
                animationCounter++;
                if (animationCounter >= animationRate) //Goes to the next frame based on the animation rate
                {
                    nextCell();
                    animationCounter = 0;
                }

            }
        }

        public override void DrawAura(SpriteBatch spriteBatch, Vector2 screenOrigin)
        {
            //int x = cellW * (currentCellCol - 1);
            //int y = cellH * (currentCellRow - 1);

            //crop = new Rectangle(x, y, cellW, cellH); //Set the size

            if (texture != null) spriteBatch.Draw(texture, screenOrigin, crop, Color.White, rotation, origin, auraScale, spriteEffects, depth);
        }

        public override void Draw(SpriteBatch theSpriteBatch) //This draws the stuffs. And it's more complex than just the Sprite version.
        {
            if (!visible) return;
            color.A = alpha;
            //We only draw the current cell we're in, so this calculates the position of where that is in the sprite sheet.
            //int x = cellW * (currentCellCol - 1);
            //int y = cellH * (currentCellRow - 1);

            //crop = new Rectangle(x, y, cellW, cellH); //Set the size

            
            //And then we draw! 
            if (auraTexture != null) theSpriteBatch.Draw(auraTexture, sPosition * positionModifier, auraTexture.Bounds, Color.White, 0, new Vector2(auraTexture.Width / 2, auraTexture.Height / 2), 1, SpriteEffects.None, depth);
            if (texture != null) theSpriteBatch.Draw(texture, sPosition * positionModifier, crop, color, rotation, origin, scale, spriteEffects, depth);
            
        }
    }
}
