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

namespace WorldsApart.Code.Levels
{
    class FinalBridge : Level
    {
        bool beforeCutscene = true;
        bool goodEnding = false;

        int sceneCounter = 0;

        AnimatedSprite player1Ending;
        AnimatedSprite player2Ending;

        AnimatedSprite player1FadeFrame;
        AnimatedSprite player2FadeFrame;

        SpriteIMG finalField;


        public FinalBridge(GSPlay gsPlay)
            : base(gsPlay)
        {
            levelDataTexture = gsPlay.LoadTexture("Levels/bridge");

            //player1Pos = GridToPosition(79, 44);
            player2Pos = GridToPosition(6, 44);
            player1Pos = GridToPosition(131, 44);

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

            for (int x = 1; x <= 5; x++)
            {
                SpriteIMG cloud = gsPlay.AddFrontFGTile(gsPlay.LoadTexture("LevelTiles/Bridge/clouds" + x + "x1"), new Vector2(1024 * (x - 1), 0));
                cloud.illuminatingAllTheTime = true;
                cloud.origin = Vector2.Zero;
            }

            Vector2 bridgePos = new Vector2(2160, 613.5f);
            gsPlay.AddFrontFGTile(gsPlay.LoadTexture("GameObjects/bridgeFull"), bridgePos);

            atmosphereLight = new Color(100, 100, 100);

            PointLight sun1 = gsPlay.AddPointLight(Art.whitePixel, GridToPosition(0, 0) + new Vector2(0, -200), new Vector2(2048, 1850));
            PointLight sun2 = gsPlay.AddPointLight(Art.whitePixel, GridToPosition(32, 0) + new Vector2(0, -200), new Vector2(2048, 1850));
            PointLight sun3 = gsPlay.AddPointLight(Art.whitePixel, GridToPosition(64, 0) + new Vector2(0, -200), new Vector2(2048, 1850));
            PointLight sun4 = gsPlay.AddPointLight(Art.whitePixel, GridToPosition(96, 0) + new Vector2(0, -200), new Vector2(2048, 1850));
            PointLight sun5 = gsPlay.AddPointLight(Art.whitePixel, GridToPosition(128, 0) + new Vector2(0, -200), new Vector2(2048, 1850));
            sun1.screenCull = false;
            sun2.screenCull = false;
            sun3.screenCull = false;
            sun4.screenCull = false;
            sun5.screenCull = false;


            finalField = gsPlay.AddBackFGTile(gsPlay.LoadTexture("Cutscene/finalField"), new Vector2(2175, 451));
            finalField.visible = false;
            

            Vector2 introPos = GridToPosition(41, 25) + new Vector2(0, 20);
            player1Ending = new AnimatedSprite(gsPlay.LoadTexture("Cutscene/cutscenePlayers"), introPos);
            player2Ending = new AnimatedSprite(gsPlay.LoadTexture("Cutscene/cutscenePlayers"), introPos);
            player1Ending.SetAnimationStuff(1, 1, 6, 8, 256, 256, 4, 12);
            player2Ending.SetAnimationStuff(1, 5, 6, 8, 256, 256, 4, 12);
            player1Ending.SetPlayerMode(PlayerObjectMode.One);
            player2Ending.SetPlayerMode(PlayerObjectMode.Two);
            gsPlay.frontFGList.Add(player1Ending);
            gsPlay.frontFGList.Add(player2Ending);
            player1Ending.visible = false;
            player2Ending.visible = false;

            player1FadeFrame = new AnimatedSprite(gsPlay.LoadTexture("player1Ideal"), Vector2.Zero);
            player2FadeFrame = new AnimatedSprite(gsPlay.LoadTexture("player2Ideal"), Vector2.Zero);
            player1FadeFrame.SetAnimationStuff(1, 1, 1, 6, 256, 256, 6, 8);
            player2FadeFrame.SetAnimationStuff(1, 1, 1, 6, 256, 256, 6, 8);
            player1FadeFrame.SetPlayerMode(PlayerObjectMode.Two);
            player2FadeFrame.SetPlayerMode(PlayerObjectMode.One);
            gsPlay.frontFGList.Add(player1FadeFrame);
            gsPlay.frontFGList.Add(player2FadeFrame);
            player1FadeFrame.visible = false;
            player2FadeFrame.visible = false;

            TriggerArea update = gsPlay.AddTriggerArea(new EventTrigger(this, 99), Art.smoke, Vector2.Zero);
            update.visible = false;
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
                        gsPlay.gameStateManager.currentLevel = 0;
                        gsPlay.player1.am.StartFade(15, 255, 0);
                        gsPlay.player2.am.StartFade(15, 255, 0);
                        //gsPlay.gameStateManager.SwitchToGSPlay();
                        gsPlay.gameStateManager.TransitionToGameState(gsPlay, GameStateType.GSPlay, 30);
                    }
                    break;
                case 99:

                    //64 * 32


                    for (int i = 0; i < 2; i++)
                    {
                        Particle rain = gsPlay.AddRain(true);
                        rain.position = new Vector2(rain.position.X, 762);
                        rain = gsPlay.AddRain(false);
                        rain.position = new Vector2(rain.position.X, 762);
                    }

                    if (gsPlay.player2.position.X > 2048)
                    {
                        gsPlay.player2.superStopInput = true;
                        gsPlay.player2.speed.X = 0;
                    }
                    if (gsPlay.player1.position.X < 2272)
                    {
                        gsPlay.player1.superStopInput = true;
                        gsPlay.player1.speed.X = 0;
                    }

                    if (beforeCutscene)
                    {
                        if (gsPlay.player1.superStopInput && gsPlay.player2.superStopInput)
                        {
                            if (gsPlay.gameStateManager.goodness > 0)
                            {
                                beforeCutscene = false;
                                goodEnding = true;
                                GSOverlay.FadeIn(120, Color.White);
                            }
                            else
                            {
                                beforeCutscene = false;
                                goodEnding = false;
                                GSOverlay.FadeIn(120, Color.Black);
                            }
                        }
                    }
                    else
                    {
                        sceneCounter++;
                        if (goodEnding)
                        {
                            if (sceneCounter == 120)
                            {
                                gsPlay.player1.visible = false;
                                gsPlay.player2.visible = false;
                                gsPlay.alphaDot.visible = false;

                                player1Ending.visible = true;
                                player2Ending.visible = true;

                                finalField.visible = true;
                                gsPlay.frontFGList.Clear();
                                gsPlay.frontFGList.Add(player1Ending);
                                gsPlay.frontFGList.Add(player2Ending);

                                //67.5 x 13.5
                                player1Ending.position = GridToPosition(67, 13) + new Vector2(16, 16);
                                player2Ending.position = player1Ending.position;
                                player1Ending.ChangeAnimationBounds(6, 1, 4);
                                player2Ending.ChangeAnimationBounds(6, 1, 4);

                                gsPlay.player1.position = player1Ending.position;
                                gsPlay.player2.position = player1Ending.position;

                                //player1Ending.spriteEffects = SpriteEffects.FlipHorizontally;
                                //player2Ending.spriteEffects = SpriteEffects.FlipHorizontally;

                                //TODO: unified background

                                GSOverlay.FadeOut(120);
                            }
                        }
                        else
                        {
                            if (sceneCounter == 120)
                            {
                                gsPlay.player1.visible = false;
                                gsPlay.player2.visible = false;
                                gsPlay.alphaDot.visible = false;

                                player1Ending.visible = true;
                                player2Ending.visible = true;
                                player2Ending.ChangeAnimationBounds(4, 1, 8);
                                player1Ending.ChangeAnimationBounds(5, 1, 8);
                                player1Ending.position = gsPlay.player1.position;
                                player2Ending.position = gsPlay.player2.position;
                                //player1Ending.spriteEffects = SpriteEffects.FlipHorizontally;
                                player2Ending.spriteEffects = SpriteEffects.FlipHorizontally;

                                GSOverlay.FadeOut(120);

                                //player1FadeFrame.visible = true;
                                //player2FadeFrame.visible = true;
                                //player1Ending.position = gsPlay.player1.position;
                                //player2Ending.position = gsPlay.player2.position;

                                //TODO: broken bridge bits

                            }
                        }


                        if (sceneCounter == 540)
                        {
                            gsPlay.gameStateManager.currentLevel = 1;
                            gsPlay.gameStateManager.TransitionToGameState(gsPlay, GameStateType.GSWin, 30);
                        }
                    }

                    break;
            }
        }

        public void ShatterBridge()
        {
            

        }
    }
}
