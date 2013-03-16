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
    class Level2 : Level
    {
        public Level2(GSPlay gsPlay)
            : base(gsPlay)
        {
            levelDataTexture = gsPlay.LoadTexture("Levels/level2Data");
            SetupLevel();

            gsPlay.player1 = new Player(PlayerObjectMode.One, gsPlay.LoadTexture("player1"), Level.GridToPosition(new Point(3, 11)));
            gsPlay.player1.SetAnimationStuff(1, 1, 3, 3, 64, 64, 9, 5);
            gsPlay.player1.SetCollisionBox(52, 44, Vector2.Zero);
            gsPlay.player2 = new Player(PlayerObjectMode.Two, gsPlay.LoadTexture("player2"), Level.GridToPosition(new Point(3, 11)));
            gsPlay.player2.SetAnimationStuff(1, 1, 3, 3, 64, 64, 9, 5);
            gsPlay.player2.SetCollisionBox(52, 44, Vector2.Zero);


            Portal glados = gsPlay.AddPortal(new EventTrigger(this, 0), gsPlay.LoadTexture("TestSprites/portal"), GridToPosition(28, 11));
            glados.SetAnimationStuff(1, 2, 1, 2, 48, 96, 2, 5);
            glados.isAnimating = false;
            Collectible goody = gsPlay.AddCollectible(new EventTrigger(this, glados), gsPlay.LoadTexture("TestSprites/Cursor"), GridToPosition(23, 12));
            goody.selfIlluminating = true;
            goody.SetAnimationStuff(1, 1, 1, 2, 64, 64, 2, 10);
            goody.SetCollisionBox(32, 32, Vector2.Zero);

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
                        gsPlay.gameStateManager.currentLevel = 3;
                        gsPlay.gameStateManager.SwitchToGSPlay();
                    }
                    break;
            }
        }
    }
}
