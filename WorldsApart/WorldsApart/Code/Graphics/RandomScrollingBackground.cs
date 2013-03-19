using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using WorldsApart.Code.Controllers;

namespace WorldsApart.Code.Graphics
{
    class RandomScrollingBackground
    {
        public List<Texture2D> backgroundTextures = new List<Texture2D>();
        public List<FadingBackground> bgList = new List<FadingBackground>();

        public RandomScrollingBackground()
        {

        }
    }
}
