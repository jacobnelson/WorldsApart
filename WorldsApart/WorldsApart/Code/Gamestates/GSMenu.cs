using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Net;
//using Microsoft.Xna.Framework.GamerServices;

using WorldsApart.Code.Graphics;
using WorldsApart.Code.Controllers;

using System.Diagnostics;

namespace WorldsApart.Code.Gamestates
{
    class GSMenu : GameState
    {
        public GSMenu(GameStateManager gsm)
            : base(gsm)
        {
            gsm.SwitchToGSPlay();
        }
       

    }
}
