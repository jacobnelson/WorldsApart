using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

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

            rightLimit = levelWidth;

            Vector2 bgPosition = new Vector2(-400, -400);
            SpriteIMG bg1 = new SpriteIMG(gsPlay.LoadTexture("BGs/mountainBackdropBaseBoth"), bgPosition);
            bg1.scale = new Vector2(1.5f);
            gsPlay.AddParallax(bg1, .1f);
            bgPosition = new Vector2(-600, -380);
            SpriteIMG bg2 = new SpriteIMG(gsPlay.LoadTexture("BGs/mountainBackdropOverlayBoth"), bgPosition);
            bg2.scale = new Vector2(2);
            gsPlay.AddParallax(bg2, .2f);

            for (int x = 1; x <= 5; x++)
            {
                for (int y = 1; y <= 2; y++)
                {
                    if (x == 5 && y == 1) continue;

                    SpriteIMG matte = new SpriteIMG(gsPlay.LoadTexture("BGs/bridgeMatte" + x + "x" + y), new Vector2(1024 * (x - 1) - 1078, 1024 * (y - 1) - 100));
                    gsPlay.AddParallax(matte, .5f);


                }
            }

            for (int x = 1; x <= 5; x++)
            {
                for (int y = 1; y <= 3; y++)
                {
                    string fgName = "LevelTiles/Bridge/bridgeFG" + x + "x" + y;
                    if (File.Exists("Content/" + fgName + ".xnb"))
                    {
                        SpriteIMG fg = gsPlay.AddFrontFGTile(gsPlay.LoadTexture(fgName), new Vector2(1024 * (x - 1), 1024 * (y - 1)));
                        fg.origin = Vector2.Zero;
                    }

                    string bgName = "LevelTiles/Bridge/bridgeBG" + x + "x" + y;
                    if (File.Exists("Content/" + bgName + ".xnb"))
                    {
                        SpriteIMG bg = gsPlay.AddBackFGTile(gsPlay.LoadTexture(bgName), new Vector2(1024 * (x - 1), 1024 * (y - 1)));
                        bg.origin = Vector2.Zero;
                    }

                }
            }

            

            barrier = gsPlay.AddTriggerArea(new EventTrigger(this, 1), gsPlay.LoadTexture("bridgeBarrier"), GridToPosition(67, 7) + new Vector2(16, 16));
            barrier.visible = false;
            breaker = gsPlay.AddTriggerArea(new EventTrigger(this, 2), gsPlay.LoadTexture("bridgeBreakTrigger"), GridToPosition(67, 14) + new Vector2(16, 0));
            breaker.visible = false;
            gsPlay.AddTriggerArea(new EventTrigger(this, 0), gsPlay.LoadTexture("bridgeBreakTrigger"), GridToPosition(67, 50) + new Vector2(16, 0)).visible = false;

            AudioManager.PlayMusic("Bridge");
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
                        gsPlay.player1.am.StartFade(15, 255, 0);
                        gsPlay.player2.am.StartFade(15, 255, 0);
                        //gsPlay.gameStateManager.SwitchToGSPlay();
                        gsPlay.gameStateManager.TransitionToGameState(gsPlay, GameStateType.GSPlay, 30);
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
