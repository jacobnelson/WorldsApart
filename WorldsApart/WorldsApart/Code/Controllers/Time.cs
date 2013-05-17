using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace WorldsApart.Code.Controllers
{
    static class Time
    {
        static public GameTime gameTime;

        static public void UpdateTime(GameTime gt)
        {
            gameTime = gt;
        }

        static public float GetSeconds()
        {
            return GetMillis() / 1000.0f;
        }

        static public float GetMillis()
        {
            return (float)gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}
