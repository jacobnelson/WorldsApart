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
    class Level5 : Level
    {
        public Level5(GSPlay gsPlay)
            : base(gsPlay)
        {
            levelDataTexture = gsPlay.LoadTexture("Levels/level5Data");


            player1Pos = GridToPosition(3, 11);
            player2Pos = GridToPosition(3, 11);

            portalPos = GridToPosition(28, 11);
            pItemPos = GridToPosition(23, 12);

            SetupLevel();

            rightLimit = levelWidth;

            //672,11
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
                        gsPlay.gameStateManager.currentLevel = 6;
                        gsPlay.gameStateManager.SwitchToGSPlay();
                    }
                    break;
            }
        }
    }
}
