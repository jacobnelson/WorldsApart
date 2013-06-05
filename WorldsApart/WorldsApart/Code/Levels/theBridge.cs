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

using System.Diagnostics;


namespace WorldsApart.Code.Levels
{
    class theBridge : Level
    {
        TriggerArea barrier;
        TriggerArea breaker;
        

        int player1Count = 0;
        int player2Count = 0;
        bool player1Hit = false;
        bool player2Hit = false;
        float barrierCounter = 0;
        float barrierRate = 1;

        public theBridge(GSPlay gsPlay)
            : base(gsPlay)
        {
            levelDataTexture = gsPlay.LoadTexture("Levels/bridge");

            //player1Pos = GridToPosition(79, 44);
            player1Pos = GridToPosition(6, 44);
            player2Pos = GridToPosition(131, 44);

            hasPortal = false;

            //portalPos = GridToPosition(681, 20);
            //pItemPos = GridToPosition(672

            SetupLevel();

            barrier = gsPlay.AddTriggerArea(new EventTrigger(this, 1), gsPlay.LoadTexture("bridgeBarrier"), GridToPosition(67, 7) + new Vector2(16, 16));
            barrier.visible = false;
            breaker = gsPlay.AddTriggerArea(new EventTrigger(this, 2), gsPlay.LoadTexture("bridgeBreakTrigger"), GridToPosition(67, 14) + new Vector2(16, 0));
            breaker.visible = false;
            gsPlay.AddTriggerArea(new EventTrigger(this, 0), gsPlay.LoadTexture("bridgeBreakTrigger"), GridToPosition(67, 67) + new Vector2(16, 0)).visible = false;
        }

        public override void ActivateEvent(int eventID, TriggerState triggerState)
        {
            base.ActivateEvent(eventID, triggerState);

            switch (eventID)
            {
                case 0:
                    if (triggerState == TriggerState.Triggered)
                    {
                        gsPlay.gameStateManager.currentLevel = 4;
                        gsPlay.gameStateManager.SwitchToGSPlay();
                    }
                    break;
                case 1:

                    atmosphereLight = Color.White;
                    //barrierCounter += Time.GetSeconds();
                    //if (barrierCounter >= barrierRate)
                    //{
                    //    player1Hit = false;
                    //    player2Hit = false;
                    //}

                    //if (player1Hit && player2Hit)
                    //{
                    //    ShatterBridge();
                    //}
                    

                    if (barrier.hitBox.CheckCollision(gsPlay.player1.hitBox))
                    {
                        gsPlay.player1.nextForce.X = -5;
                        Vector2 pos = new Vector2(barrier.position.X, gsPlay.player1.position.Y);
                        Particle p = gsPlay.AddParticle(Art.barrier, pos);
                        p.startScale = .8f;
                        p.endScale = 2f;
                        p.startAlpha = 255;
                        p.endAlpha = 0;
                        p.StartParticleSystems();
                        player1Count++;
                        atmosphereLight = Color.Black;
                    }
                    if (barrier.hitBox.CheckCollision(gsPlay.player2.hitBox))
                    {
                        gsPlay.player2.nextForce.X = 5;
                        Vector2 pos = new Vector2(barrier.position.X, gsPlay.player2.position.Y);
                        Particle p = gsPlay.AddParticle(Art.barrier, pos);
                        p.startScale = .8f;
                        p.endScale = 2f;
                        p.startAlpha = 255;
                        p.endAlpha = 0;
                        p.StartParticleSystems();
                        player2Count++;
                        atmosphereLight = Color.Black;
                    }
                    break;
                case 2:
                    if (player1Count >= 5 && player2Count >= 5 && breaker.hitBox.CheckCollision(gsPlay.player1.hitBox) && breaker.hitBox.CheckCollision(gsPlay.player2.hitBox))
                    {
                        ShatterBridge();
                    }
                    break;
                case 3:

                    break;
            }
        }

        public void ShatterBridge()
        {
            //62, 15
            environmentData[62, 15].isSolid = false;
            environmentData[63, 15].isSolid = false;
            environmentData[64, 15].isSolid = false;
            environmentData[65, 15].isSolid = false;
            environmentData[66, 15].isSolid = false;
            environmentData[67, 15].isSolid = false;
            environmentData[68, 15].isSolid = false;
            environmentData[69, 15].isSolid = false;
            environmentData[70, 15].isSolid = false;
            environmentData[71, 15].isSolid = false;
            environmentData[72, 15].isSolid = false;

        }
    }
}
