using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using WorldsApart.Code.Entities;
using WorldsApart.Code.Gamestates;
using WorldsApart.Code.Controllers;
using WorldsApart.Code.Graphics;

namespace WorldsApart.Code.Levels
{
    class Level1 : Level
    {
        public Level1(GSPlay gsPlay)
            : base(gsPlay)
        {
            levelDataTexture = gsPlay.LoadTexture("Levels/level1Data");
            SetupLevel();

            gsPlay.player1 = new Player(PlayerObjectMode.One, gsPlay.LoadTexture("player1"), Level.GridToPosition(new Point(597, 4)));
            gsPlay.player1.SetAnimationStuff(1, 1, 3, 3, 64, 64, 9, 5);
            gsPlay.player1.SetCollisionBox(52, 44, Vector2.Zero);
            gsPlay.player2 = new Player(PlayerObjectMode.Two, gsPlay.LoadTexture("player2"), Level.GridToPosition(new Point(45, 26)));
            gsPlay.player2.SetAnimationStuff(1, 1, 3, 3, 64, 64, 9, 5);
            gsPlay.player2.SetCollisionBox(52, 44, Vector2.Zero);
        }

        public override void ActivateEvent(int eventID, TriggerState triggerState)
        {
            base.ActivateEvent(eventID, triggerState);
        }
    }
}
