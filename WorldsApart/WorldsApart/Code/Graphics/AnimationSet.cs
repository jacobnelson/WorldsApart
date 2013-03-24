using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WorldsApart.Code.Graphics
{
    class AnimationSet
    {
        public Texture2D texture;
        public int minRow = 1;
        public int minCol = 1;
        public int rows = 1;
        public int cols = 1;
        public int frames = 1;
        public Point endingRowCol = new Point(1, 1);
        public int animationRate = 1;

        public AnimationSet(Texture2D texture, int minRow, int minCol, int rows, int cols, int frames, int animationRate)
        {
            this.texture = texture;
            this.minRow = minRow;
            this.minCol = minCol;
            this.rows = rows;
            this.cols = cols;
            this.frames = frames;
            this.animationRate = animationRate;
        }

        public AnimationSet(Texture2D texture, int minRow, int minCol, int rows, int cols, int frames, int animationRate, Point endingRowCol)
            : this(texture, minRow, minCol, rows, cols, frames, animationRate)
        {
            this.endingRowCol = endingRowCol;
        }
    }
}
