using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WorldsApart.Code.Graphics
{
    class GaussianTargets
    {

        public RenderTarget2D blurX;
        public RenderTarget2D blurY;
        public RenderTarget2D target;
        public Vector2 auraTargetOrigin = Vector2.Zero;

        public GaussianTargets(GraphicsDevice gd)
        {
            target = new RenderTarget2D(gd, Game1.screenWidth, Game1.screenHeight);
            blurX = new RenderTarget2D(gd,
                Game1.screenWidth, Game1.screenHeight, false,
                gd.PresentationParameters.BackBufferFormat,
                DepthFormat.None);
            blurY = new RenderTarget2D(gd,
                Game1.screenWidth, Game1.screenHeight, false,
                gd.PresentationParameters.BackBufferFormat,
                DepthFormat.None);
            auraTargetOrigin = new Vector2(Game1.screenWidth / 2, Game1.screenHeight / 2);
        }
    }
}
