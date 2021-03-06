﻿using System;
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

    class TestLevel : Level
    {
        //Barf!
        InventoryItem item1;
        InventoryItem item2;
        InventoryItem item3;
        TriggerArea item1Area;
        TriggerArea item2Area;
        TriggerArea item3Area;
        TriggerArea final1;
        TriggerArea final2;
        TriggerArea final3;
        bool final1Unlock = false;
        bool final2Unlock = false;
        bool final3Unlock = false;
        Door finalDoor;
        FlipSwitch timedSwitch;

        PointLight player1Light;
        PointLight player2Light;

        

        public TestLevel(GSPlay gsPlay)
            : base(gsPlay)
        {
            
            levelDataTexture = gsPlay.LoadTexture("TestSprites/testLevelData");

            player1Pos = GridToPosition(175, 25);
            player2Pos = GridToPosition(24, 47);

            portalPos = GridToPosition(211, 12);
            pItemPos = GridToPosition(204, 12);

            renderCollision = true;

            SetupLevel();

            leftLimit = 32;
            rightLimit = levelWidth;

            //gsPlay.AddSpinningFireParticle(GridToPosition(7, 47));

            LightningChain bolt = gsPlay.AddLightning(GridToPosition(24, 42), GridToPosition(27, 42), Color.White);
            bolt.AddVertex(GridToPosition(27, 38));
            bolt.SetActive(true);


            SpriteIMG bg = new SpriteIMG(gsPlay.LoadTexture("TestSprites/testWash1"), new Vector2(levelWidth/2, levelHeight/2));
            gsPlay.AddParallax(bg, .5f);
            FadingBackground bg2 = new FadingBackground(gsPlay.LoadTexture("TestSprites/testWash2"), new Vector2(levelWidth / 2, levelHeight / 2));
            gsPlay.AddParallax(bg2, .75f);
            bg2.SetFading(120, 10);
            bg2.SetMoving(bg2.position, bg2.position + new Vector2(-1000, 0), 2, 0);
            
            

            //SpriteIMG s = new SpriteIMG(gsPlay.LoadTexture("bgSky"), new Vector2(levelWidth / 2, levelHeight / 2));
            //gsPlay.AddParallax(s, .5f);

            //SpriteIMG fg = new SpriteIMG(gsPlay.LoadTexture("TestSprites/environmentArt1"));
            //fg.origin = Vector2.Zero;
            //gsPlay.tileList.Add(fg);

            //fg = new SpriteIMG(gsPlay.LoadTexture("TestSprites/environmentArt2"), new Vector2(2048, 0));
            //fg.origin = Vector2.Zero;
            //gsPlay.tileList.Add(fg);

            //fg = new SpriteIMG(gsPlay.LoadTexture("TestSprites/environmentArt3"), new Vector2(4096, 0));
            //fg.origin = Vector2.Zero;
            //gsPlay.tileList.Add(fg);

            //fg = new SpriteIMG(gsPlay.LoadTexture("TestSprites/environmentArt4"), new Vector2(4096 + 2048, 0));
            //fg.origin = Vector2.Zero;
            //gsPlay.tileList.Add(fg);

            //fg = new SpriteIMG(gsPlay.LoadTexture("TestSprites/environmentArt5"), new Vector2(4096 + 4096, 0));
            //fg.origin = Vector2.Zero;
            //gsPlay.tileList.Add(fg);

            //fg = new SpriteIMG(gsPlay.LoadTexture("TestSprites/environmentArt0"), new Vector2(-512, 0));
            //fg.origin = Vector2.Zero;
            //gsPlay.tileList.Add(fg);

            //ParticleEmitter pe = gsPlay.AddEmitter(new AnimatedSprite(gsPlay.LoadTexture("TestSprites/puff")), GridToPosition(157, 15));
            //pe.speed = new Vector2(1, 0);

            atmosphereLight = new Color(150, 150, 150);

            Door dooor = gsPlay.AddOpeningDoor(Art.door, GridToPosition(36, 30), GridToPosition(36, 34), OpenState.Closed);
            FlipSwitch fs1 = gsPlay.AddSwitch(new EventTrigger(this, dooor), gsPlay.LoadTexture("TestSprites/switch"), GridToPosition(29, 35));
            fs1.SetPlayerMode(PlayerObjectMode.Two);
            LightningChain lc1 = gsPlay.AddLightning(GridToPosition(29, 35), dooor.position, Color.Green);
            lc1.ConvertEndPointToTarget(dooor);
            fs1.AddEvent(new EventTrigger(this, lc1));


            CircularPlatform cp1 = gsPlay.AddCircularPlatform(Art.platform, GridToPosition(36, 45), 100, 240);
            gsPlay.AddSwitch(new EventTrigger(this, cp1), gsPlay.LoadTexture("TestSprites/switch"), GridToPosition(42, 48));
           // LightningChain lc2 = new LightningChain(GridToPosition(42, 48), Vector2.Zero, Color.Green);
            LightningChain lc2 = gsPlay.AddLightning(GridToPosition(42, 48), Vector2.Zero, Color.Green);
            lc2.ConvertEndPointToTarget(cp1);
            lc2.SetActive(true);


            gsPlay.AddPointLight(gsPlay.LoadTexture("ShaderAssets/pointLight"), GridToPosition(30, 47), new Vector2(3));
            player1Light = gsPlay.AddPointLight(gsPlay.LoadTexture("ShaderAssets/pointLight"), Vector2.Zero, new Vector2(2));
            player1Light.SetGlowing(2, 2.4f, 120);
            player2Light = gsPlay.AddPointLight(gsPlay.LoadTexture("ShaderAssets/pointLight"), Vector2.Zero, new Vector2(2));
            player2Light.SetGlowing(2, 2.4f, 120);


            
            LightConsole console1 = gsPlay.AddLightConsole(gsPlay.LoadTexture("TestSprites/LightConsole"), GridToPosition(189, 39));
            console1.AddLight(player1Light);
            LightConsole console2 = gsPlay.AddLinkedLightConsole(console1, console1.texture, GridToPosition(189, 32));
            console2.AddLight(player2Light);
            LightningChain consoleLightning = gsPlay.AddLightning(GridToPosition(189, 39), GridToPosition(189, 32), Color.White);
            consoleLightning.SetActive(true);
            console1.AddEvent(new EventTrigger(this, consoleLightning));
            console2.AddEvent(new EventTrigger(this, consoleLightning));
            

            console1 = gsPlay.AddLightConsole(console1.texture, GridToPosition(220, 17));


            gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), GridToPosition(new Point(137, 46))).selfIlluminating = true;
            gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), Level.GridToPosition(new Point(75, 16))).selfIlluminating = true;
            
            MovingPlatform m1 = gsPlay.AddActivatePlatform(Art.platform, Level.GridToPosition(new Point(94, 44)), Level.GridToPosition(new Point(112, 44)));
            m1.duration = 480;
            Button bb1 = gsPlay.AddButton(new EventTrigger(this, m1), 1, gsPlay.LoadTexture("TestSprites/button"), Level.GridToPosition(new Point(88, 45)));
            LightningChain lc3 = gsPlay.AddLightning(GridToPosition(88, 45), GridToPosition(88, 46) + new Vector2(0, 16), Color.Green);
            lc3.AddVertex(GridToPosition(103, 46) + new Vector2(0, 16));
            lc3.AddVertex(Vector2.Zero);
            lc3.ConvertEndPointToTarget(m1);
            bb1.AddEvent(new EventTrigger(this, lc3));

            Door d1 = gsPlay.AddOpeningDoor(Art.platform, GridToPosition(new Point(152, 41)) + new Vector2(16, 16), GridToPosition(new Point(149, 41)) + new Vector2(16, 16), OpenState.Closed);
            
            EventTrigger e1 = new EventTrigger(this, d1);
            e1.eventID = 9;
            TimerObject t1 = gsPlay.AddTimer(e1, 240);
            timedSwitch = gsPlay.AddSwitch(new EventTrigger(this, t1), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(147, 35)));
            timedSwitch.onlyTriggered = true;

            d1 = gsPlay.AddFadingDoor(Art.platform, GridToPosition(new Point(157, 36)) + new Vector2(16,16), OpenState.Closed);
            d1.traceID = 99;
            gsPlay.AddButton(new EventTrigger(this, d1), 1, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(162, 44)));
            //d1 = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/doorThick"), GridToPosition(new Point(191, 31)), GridToPosition(new Point(191, 31)), OpenState.Open);
            //d1.SetPlayerMode(PlayerObjectMode.Two);
            //d1.visible = false;
            //d1 = gsPlay.AddOpeningDoor(gsPlay.LoadTexture("TestSprites/doorThick"), GridToPosition(new Point(191, 38)), GridToPosition(new Point(191, 38)), OpenState.Open);
            //d1.SetPlayerMode(PlayerObjectMode.Two);
            //d1.visible = false;

            //Door d2 = gsPlay.AddFadingDoor(Art.door, GridToPosition(191, 31), OpenState.Closed);
            ////gsPlay.AddTriggerArea(new EventTrigger(this, d2), gsPlay.LoadTexture("TestSprites/switch"), GridToPosition(187, 39)).traceID = 27;
            //FlipSwitch fs1 = gsPlay.AddSwitch(new EventTrigger(this, d2), gsPlay.LoadTexture("TestSprites/switch"), GridToPosition(187, 39));
            //fs1.pressureCooker = true;

            d1 = gsPlay.AddFadingDoor(Art.door, GridToPosition(204, 38), OpenState.Closed);
            gsPlay.AddButton(new EventTrigger(this, d1), 1, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(200, 33));
            d1 = gsPlay.AddFadingDoor(Art.door, GridToPosition(204, 31), OpenState.Closed);
            gsPlay.AddButton(new EventTrigger(this, d1), 1, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(209, 40));
            d1 = gsPlay.AddFadingDoor(Art.door, GridToPosition(217, 38), OpenState.Closed);
            gsPlay.AddButton(new EventTrigger(this, d1), 1, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(213, 33));
            d1 = gsPlay.AddFadingDoor(Art.door, GridToPosition(217, 31), OpenState.Closed);
            gsPlay.AddButton(new EventTrigger(this, d1), 1, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(222, 40));

            //gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), GridToPosition(new Point(175, 24)));

            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(168, 2)), .8f);
            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(177, 25)), .8f);
            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(26, 47)), .8f);

            item1 = new InventoryItem(gsPlay.LoadTexture("TestSprites/item1"));
            itemList.Add(item1);
            item1Area = gsPlay.AddTriggerArea(new EventTrigger(this, 1), item1.itemImage.texture, GridToPosition(3, 25));
            item1Area.selfIlluminating = true;
            item2 = new InventoryItem(gsPlay.LoadTexture("TestSprites/item2"));
            itemList.Add(item2);
            item2Area = gsPlay.AddTriggerArea(new EventTrigger(this, 2), item2.itemImage.texture, GridToPosition(248, 2));
            item2Area.selfIlluminating = true;
            item3 = new InventoryItem(gsPlay.LoadTexture("TestSprites/item3"));
            itemList.Add(item3);
            item3Area = gsPlay.AddTriggerArea(new EventTrigger(this, 3), item3.itemImage.texture, GridToPosition(227, 31));
            item3Area.selfIlluminating = true;
            //gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(54, 27)), .8f);
            //gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(45, 27)), .8f);
            //gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(69, 12)), .8f);
            //gsPlay.AddPlatform(Art.platform, Level.GridToPosition(new Point(41, 16)), Level.GridToPosition(new Point(50, 16)));

            finalDoor = gsPlay.AddFadingDoor(Art.platform, GridToPosition(207, 13) + new Vector2(16, 16), OpenState.Closed);
            //gsPlay.AddSwitch(new EventTrigger(this, gsPlay.AddEventPlatform(Art.platform, Level.GridToPosition(new Point(93, 26)), Level.GridToPosition(new Point(100, 26)))), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(88, 28)));
            //gsPlay.AddTriggerArea(new EventTrigger(this, 0), gsPlay.LoadTexture("TestSprites/LightConsole"), Level.GridToPosition(new Point(207, 11)) + new Vector2(16,16));


            final1 = gsPlay.AddTriggerArea(new EventTrigger(this, 5), gsPlay.LoadTexture("TestSprites/lock1"), GridToPosition(202, 16) + new Vector2(16, 16));
            final1.SetAnimationStuff(1, 2, 1, 2, 64, 64, 2, 5);
            final1.isAnimating = false;
            final2 = gsPlay.AddTriggerArea(new EventTrigger(this, 6), gsPlay.LoadTexture("TestSprites/lock2"), GridToPosition(207, 16) + new Vector2(16, 16));
            final2.SetAnimationStuff(1, 2, 1, 2, 64, 64, 2, 5);
            final2.isAnimating = false;
            final3 = gsPlay.AddTriggerArea(new EventTrigger(this, 7), gsPlay.LoadTexture("TestSprites/lock3"), GridToPosition(212, 16) + new Vector2(16, 16));
            final3.SetAnimationStuff(1, 2, 1, 2, 64, 64, 2, 5);
            final3.isAnimating = false;


        }

        public override void ActivateEvent(int eventID, TriggerState triggerState)
        {
            switch (eventID)
            {
                case 0:
                    if (triggerState == TriggerState.Triggered)
                    {
                        gsPlay.player1.visible = false;
                        gsPlay.player2.visible = false;
                        gsPlay.gameStateManager.currentLevel = 7;
                        gsPlay.gameStateManager.TransitionToGameState(gsPlay, GameStateType.GSPlay, 30);
                    }
                    break;
                case 1:
                    if (item1.hasItem == false && triggerState == TriggerState.Triggered)
                    {
                        item1.hasItem = true;
                        item1Area.visible = false;
                    }
                    break;
                case 2:
                    if (item2.hasItem == false && triggerState == TriggerState.Triggered)
                    {
                        item2.hasItem = true;
                        item2Area.visible = false;
                    }
                    break;
                case 3:
                    if (item3.hasItem == false && triggerState == TriggerState.Triggered)
                    {
                        item3.hasItem = true;
                        item3Area.visible = false;
                    }
                    break;
                case 4:
                    if (finalDoor.triggerState == TriggerState.Untriggered )
                        finalDoor.ActivateEvent(TriggerState.Triggered);
                    break;
                case 5:
                    if (triggerState == TriggerState.Triggered)
                    {
                        if (item1.hasItem)
                        {
                            final1.selfIlluminating = true;
                            final1.currentCellCol = 1;
                            final1Unlock = true;

                            ActivateEvent(8, triggerState);
                        }
                    }
                    break;
                case 6:
                    if (triggerState == TriggerState.Triggered)
                    {
                        if (item2.hasItem)
                        {
                            final2.selfIlluminating = true;
                            final2.currentCellCol = 1;
                            final2Unlock = true;

                            ActivateEvent(8, triggerState);
                        }
                    }
                    break;
                case 7:
                    if (triggerState == TriggerState.Triggered)
                    {
                        if (item3.hasItem)
                        {
                            final3.selfIlluminating = true;
                            final3.currentCellCol = 1;
                            final3Unlock = true;

                            ActivateEvent(8, triggerState);
                        }
                    }
                    break;
                case 8:
                    if (final1Unlock && final2Unlock && final3Unlock)
                    {
                        if (finalDoor.openState == OpenState.Closed)
                            finalDoor.Open();
                    }
                    break;
                case 9:
                    if (triggerState == TriggerState.Untriggered)
                    {
                        timedSwitch.triggerState = TriggerState.Untriggered;
                        timedSwitch.currentCellCol = 1;
                    }
                    break;
            }

        }
    }
}
