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
            gsPlay.player2 = new Player(PlayerObjectMode.Two, gsPlay.LoadTexture("player2"), Level.GridToPosition(new Point(597, 4)));
            gsPlay.player2.SetAnimationStuff(1, 1, 3, 3, 64, 64, 9, 5);
            gsPlay.player2.SetCollisionBox(52, 44, Vector2.Zero);

            Portal glados = gsPlay.AddPortal(new EventTrigger(this, 0), gsPlay.LoadTexture("TestSprites/portal"), GridToPosition(681, 20));
            glados.SetAnimationStuff(1, 2, 1, 2, 48, 96, 2, 5);
            glados.isAnimating = false;
            Collectible goody = gsPlay.AddCollectible(new EventTrigger(this, glados), gsPlay.LoadTexture("TestSprites/Cursor"), GridToPosition(672, 11));
            goody.selfIlluminating = true;
            goody.SetAnimationStuff(1, 1, 1, 2, 64, 64, 2, 10);
            goody.SetCollisionBox(32, 32, Vector2.Zero);

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
