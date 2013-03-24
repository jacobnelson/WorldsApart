using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WorldsApart.Code.Graphics
{
    class IdealAnimationSet : AnimationSet
    {
        public Texture2D idealTexture;

        public IdealAnimationSet(Texture2D texture, Texture2D idealTexture, int minRow, int minCol, int rows, int cols, int frames, int animationRate) : base(texture, minRow, minCol, rows, cols, frames, animationRate)
        {
            this.idealTexture = idealTexture;
        }

        public IdealAnimationSet(Texture2D texture, Texture2D idealTexture, int minRow, int minCol, int rows, int cols, int frames, int animationRate, Point endingRowCol)
            : this(texture, idealTexture, minRow, minCol, rows, cols, frames, animationRate)
        {
            this.endingRowCol = endingRowCol;
        }
    }
}
