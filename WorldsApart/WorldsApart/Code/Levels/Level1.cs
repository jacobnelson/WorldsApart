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

            player1Pos = GridToPosition(10, 4);
            player2Pos = GridToPosition(597, 4);

            portalPos = GridToPosition(681, 20);
            pItemPos = GridToPosition(672, 11);

            SetupLevel();

            SpriteIMG s = new SpriteIMG(gsPlay.LoadTexture("bgSky"), new Vector2(64, levelHeight / 2));
            gsPlay.AddParallax(s, .5f);

            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), GridToPosition(427, 28), .8f);
        }

        public override void ActivateEvent(int eventID, TriggerState triggerState)
        {
            base.ActivateEvent(eventID, triggerState);

            switch (eventID)
            {
                case 0:
                    if (triggerState == TriggerState.Triggered)
                    {
                        bool isGood = true;
                        foreach (Portal portal in gsPlay.portalList)
                        {
                            if (portal.goodMode == false) isGood = false;
                        }
                        if (isGood) gsPlay.gameStateManager.goodness++;
                        else gsPlay.gameStateManager.goodness--;
                        gsPlay.gameStateManager.currentLevel = 2;
                        gsPlay.gameStateManager.SwitchToGSPlay();
                    }
                    break;
            }
        }
    }
}
