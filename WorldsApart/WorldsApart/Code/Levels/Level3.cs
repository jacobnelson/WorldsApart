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
    class Level3 : Level
    {
        public Level3(GSPlay gsPlay)
            : base(gsPlay)
        {
            levelDataTexture = gsPlay.LoadTexture("Levels/level3Data");

            player1Pos = GridToPosition(23, 150);
            player2Pos = GridToPosition(20, 150);

            //player1Pos = GridToPosition(485, 32);
            //player2Pos = player1Pos;
            

            portalPos = GridToPosition(492, 32);
            pItemPos = GridToPosition(152, 90) + new Vector2(-16, 0);

            renderCollision = true;

            SetupLevel();

            rightLimit = levelWidth - 32;


            Vector2 bgPosition = new Vector2(-400, -400);
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    SpriteIMG bg1 = new SpriteIMG(gsPlay.LoadTexture("BGs/mountainBackdropBase1"), bgPosition);
                    bg1.SetPlayerMode(PlayerObjectMode.One);
                    gsPlay.AddParallax(bg1, .1f);
                    SpriteIMG bg2 = new SpriteIMG(gsPlay.LoadTexture("BGs/mountainBackdropBase2"), bgPosition);
                    bg2.SetPlayerMode(PlayerObjectMode.Two);
                    gsPlay.AddParallax(bg2, .1f);

                    bgPosition.Y += 1024;
                }
                bgPosition.Y = -400;
                bgPosition.X += 1024;
            }

            bgPosition = new Vector2(-300, -300);
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    SpriteIMG bg1 = new SpriteIMG(gsPlay.LoadTexture("BGs/mountainBackdropOverlay1"), bgPosition);
                    bg1.SetPlayerMode(PlayerObjectMode.One);
                    gsPlay.AddParallax(bg1, .2f);
                    SpriteIMG bg2 = new SpriteIMG(gsPlay.LoadTexture("BGs/mountainBackdropOverlay2"), bgPosition);
                    bg2.SetPlayerMode(PlayerObjectMode.Two);
                    gsPlay.AddParallax(bg2, .2f);

                    bgPosition.Y += 1024;
                }

                bgPosition.Y = -300;
                bgPosition.X += 1024;
            }

            bgPosition = new Vector2(-300, -300);
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    SpriteIMG bg1 = new SpriteIMG(gsPlay.LoadTexture("BGs/mountainMatte1"), bgPosition);
                    bg1.SetPlayerMode(PlayerObjectMode.One);
                    gsPlay.AddParallax(bg1, .5f);
                    SpriteIMG bg2 = new SpriteIMG(gsPlay.LoadTexture("BGs/mountainMatte2"), bgPosition);
                    bg2.SetPlayerMode(PlayerObjectMode.Two);
                    gsPlay.AddParallax(bg2, .5f);

                    bgPosition.Y += 1024;
                }

                bgPosition.Y = -300;
                bgPosition.X += 1024;
            }

            //part1
            Door d1 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(137, 101)) + new Vector2(-16, 0), OpenState.Closed);
            FlipSwitch s1 = gsPlay.AddOnSwitch(new EventTrigger(this, d1), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(144, 111)));
            s1.SetPlayerMode(PlayerObjectMode.Two);
            Door bd1 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(137, 101)) + new Vector2(-16, 0), OpenState.Closed);
            bd1.SetPlayerMode(PlayerObjectMode.Two);
            LightningChain lc1 = gsPlay.AddLightning(s1.position, s1.position + new Vector2(0, -240), Color.LightBlue);
            lc1.AddVertex(d1.position + new Vector2(0, 80));
            lc1.AddVertex(d1.position + new Vector2(0, 64));
            s1.AddEvent(new EventTrigger(this, lc1));
            lc1.defaultActive = true;
            lc1.SetActive(true);

            Door d2 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(157, 107)) + new Vector2(-16, -16), OpenState.Closed);
            FlipSwitch s2 = gsPlay.AddOnSwitch(new EventTrigger(this, d2), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(148, 101)));
            s2.SetPlayerMode(PlayerObjectMode.One);
            Door bd2 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(157, 107)) + new Vector2(-16, -16), OpenState.Closed);
            bd2.SetPlayerMode(PlayerObjectMode.One);
            LightningChain lc2 = gsPlay.AddLightning(s2.position, s2.position + new Vector2(0, 80), Color.Red);
            lc2.AddVertex(s2.position + new Vector2(96, 80));
            lc2.AddVertex(d2.position + new Vector2(-80, 0));
            lc2.AddVertex(d2.position + new Vector2(-48, 0));
            s2.AddEvent(new EventTrigger(this, lc2));
            lc2.defaultActive = true;
            lc2.SetActive(true);



            //part2
            Door d3 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(249, 87)), OpenState.Closed);
            FlipSwitch s3 = gsPlay.AddSwitch(new EventTrigger(this, d3), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(247, 88)) + new Vector2(-16, 0));
            s3.SetPlayerMode(PlayerObjectMode.Two);
            Door bd3 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(249, 87)), OpenState.Closed);
            bd3.SetPlayerMode(PlayerObjectMode.Two);
            LightningChain lc3 = gsPlay.AddLightning(s3.position, d3.position, Color.Orange);
            s3.AddEvent(new EventTrigger(this, lc3));

            Door d4 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(247, 85)) + new Vector2(-16, -16), OpenState.Closed);
            FlipSwitch s4 = gsPlay.AddSwitch(new EventTrigger(this, d4), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(252, 88)) + new Vector2(-16, 0));
            s2.SetPlayerMode(PlayerObjectMode.One);
            Door bd4 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(247, 85)) + new Vector2(-16, -16), OpenState.Closed);
            bd4.SetPlayerMode(PlayerObjectMode.One);
            lc3.AddVertex(s4.position);
            LightningChain lc4 = gsPlay.AddLightning(s4.position, d4.position, Color.Orange);
            s4.AddEvent(new EventTrigger(this, lc4));

            Door d5 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(252, 85)) + new Vector2(-16, -16), OpenState.Closed);
            FlipSwitch s5 = gsPlay.AddSwitch(new EventTrigger(this, d5), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(247, 82)) + new Vector2(-16, 0));
            lc4.AddVertex(s5.position);
            LightningChain lc5 = gsPlay.AddLightning(s5.position, d5.position, Color.Orange);
            s5.AddEvent(new EventTrigger(this, lc5));

            Door d6 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(247, 80)) + new Vector2(-16, -16), OpenState.Closed);
            FlipSwitch s6 = gsPlay.AddSwitch(new EventTrigger(this, d6), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(252, 78)) + new Vector2(-16, 0));
            lc5.AddVertex(s6.position);
            LightningChain lc6 = gsPlay.AddLightning(s6.position, d6.position, Color.Orange);
            s6.AddEvent(new EventTrigger(this, lc6));

            Door d7 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(252, 76)) + new Vector2(-16, -16), OpenState.Closed);
            FlipSwitch s7 = gsPlay.AddSwitch(new EventTrigger(this, d7), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(247, 74)) + new Vector2(-16, 0));
            lc6.AddVertex(s7.position);
            LightningChain lc7 = gsPlay.AddLightning(s7.position, d7.position, Color.Orange);
            s7.AddEvent(new EventTrigger(this, lc7));

            Door d8 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(247, 72)) + new Vector2(-16, -16), OpenState.Closed);
            FlipSwitch s8 = gsPlay.AddSwitch(new EventTrigger(this, d8), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(252, 70)) + new Vector2(-16, 0));
            lc7.AddVertex(s8.position);
            LightningChain lc8 = gsPlay.AddLightning(s8.position, d8.position, Color.Orange);
            s8.AddEvent(new EventTrigger(this, lc8));

            Door d9 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(252, 68)) + new Vector2(-16, -16), OpenState.Closed);
            FlipSwitch s9 = gsPlay.AddSwitch(new EventTrigger(this, d9), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(247, 64)) + new Vector2(-16, 0));
            lc8.AddVertex(s9.position);
            LightningChain lc9 = gsPlay.AddLightning(s9.position, d9.position, Color.Orange);
            s9.AddEvent(new EventTrigger(this, lc9));

            //part3

            Door d10 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(327, 68)) + new Vector2(-16, 0), OpenState.Closed);
            FlipSwitch s10 = gsPlay.AddSwitch(new EventTrigger(this, d10), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(330, 61)));
            d10.SetPlayerMode(PlayerObjectMode.One);
            s10.SetPlayerMode(PlayerObjectMode.Two);
            LightningChain lc10 = gsPlay.AddLightning(s10.position, d10.position, Color.Orange);
            s10.AddEvent(new EventTrigger(this, lc10));

            Door d11 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(334, 62)) + new Vector2(-16, 0), OpenState.Closed);
            FlipSwitch s11 = gsPlay.AddSwitch(new EventTrigger(this, d11), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(337, 68)));
            d11.SetPlayerMode(PlayerObjectMode.Two);
            s11.SetPlayerMode(PlayerObjectMode.One);
            lc10.AddVertex(s11.position);
            LightningChain lc11 = gsPlay.AddLightning(s11.position, d11.position, Color.Orange);
            s11.AddEvent(new EventTrigger(this, lc11));

            Door d12 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(341, 68)) + new Vector2(-16, 0), OpenState.Closed);
            FlipSwitch s12 = gsPlay.AddSwitch(new EventTrigger(this, d12), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(344, 61)));
            d12.SetPlayerMode(PlayerObjectMode.One);
            s12.SetPlayerMode(PlayerObjectMode.Two);
            lc11.AddVertex(s12.position);
            LightningChain lc12 = gsPlay.AddLightning(s12.position, d12.position, Color.Orange);
            s12.AddEvent(new EventTrigger(this, lc12));

            Door d13 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(348, 62)) + new Vector2(-16, 0), OpenState.Closed);
            FlipSwitch s13 = gsPlay.AddSwitch(new EventTrigger(this, d13), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(351, 68)));
            d13.SetPlayerMode(PlayerObjectMode.Two);
            s13.SetPlayerMode(PlayerObjectMode.One);
            lc12.AddVertex(s13.position);
            LightningChain lc13 = gsPlay.AddLightning(s13.position, d13.position, Color.Orange);
            s13.AddEvent(new EventTrigger(this, lc13));

            //part4
            MovingPlatform mp1 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(399, 25)) + new Vector2(0, -16), Level.GridToPosition(new Point(407, 25)) + new Vector2(0, -16));
            mp1.SetPlayerMode(PlayerObjectMode.One);
            FlipSwitch ms1 = gsPlay.AddSwitch(new EventTrigger(this, mp1), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(403, 29)));
            ms1.SetPlayerMode(PlayerObjectMode.Two);
            LightningChain lc114 = gsPlay.AddLightning(ms1.position, mp1.position, Color.LightBlue);
            lc114.ConvertEndPointToTarget(mp1);
            ms1.AddEvent(new EventTrigger(this, lc114)); 

            Door d14 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(393, 9)) + new Vector2(-16, 0), OpenState.Closed);
            FlipSwitch s14 = gsPlay.AddSwitch(new EventTrigger(this, d14), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(398, 21)));
            LightningChain lc14 = gsPlay.AddLightning(s14.position, GridToPosition(393, 21), Color.Red);
            lc14.AddVertex(d14.position);
            s14.AddEvent(new EventTrigger(this, lc14));

            Door d15 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(366, 33)) + new Vector2(0, -16), OpenState.Closed);
            Button s15 = gsPlay.AddButton(new EventTrigger(this, d15), 1, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(370, 32)));
            LightningChain lc15 = gsPlay.AddLightning(s15.position, GridToPosition(370, 33) + new Vector2(0, -16), Color.Orange);
            lc15.AddVertex(GridToPosition(367, 33) + new Vector2(0, -16));
            s15.AddEvent(new EventTrigger(this, lc15));
            lc15.defaultActive = true;
            lc15.SetActive(true);

            Door d16 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(444, 69)) + new Vector2(0, -16), OpenState.Closed);
            FlipSwitch s16 = gsPlay.AddSwitch(new EventTrigger(this, d16), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(440, 71)));
            s16.SetPlayerMode(PlayerObjectMode.Two);
            LightningChain lc16 = gsPlay.AddLightning(s16.position, GridToPosition(440, 69) + new Vector2(0, -16), Color.Orange);
            lc16.AddVertex(GridToPosition(443, 69) + new Vector2(0, -16));
            s16.AddEvent(new EventTrigger(this, lc16));
            lc16.defaultActive = true;
            lc16.SetActive(true);


            gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), GridToPosition(new Point(426, 68))).selfIlluminating = true;
            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(429, 46)), .8f);
            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(388, 10)), .8f);
            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(366, 31)), .8f);
            gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(443, 71 )), .8f);

            //secret
            Door mp2 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(152, 69)) + new Vector2(-16, -16), OpenState.Closed);
            FlipSwitch ms2 = gsPlay.AddSwitch(new EventTrigger(this, mp2), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(152, 67)) + new Vector2(-16, 0));
            ms2.pressureCooker = true;
            ms2.SetPlayerMode(PlayerObjectMode.One);
            LightningChain lc17 = gsPlay.AddLightning(ms2.position, mp2.position + new Vector2(64, 0), Color.LightBlue);
            ms2.AddEvent(new EventTrigger(this, lc17));

            Door mp3 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(152, 69)) + new Vector2(-16, -16), OpenState.Closed);
            FlipSwitch ms3 = gsPlay.AddSwitch(new EventTrigger(this, mp3), gsPlay.LoadTexture("TestSprites/switch"), Level.GridToPosition(new Point(152, 67)) + new Vector2(-16, 0));
            ms3.pressureCooker = true;
            ms3.SetPlayerMode(PlayerObjectMode.Two);
            LightningChain lc18 = gsPlay.AddLightning(ms3.position, mp3.position + new Vector2(-64, 0), Color.Red);
            ms3.AddEvent(new EventTrigger(this, lc18));

            Door bd5 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(150, 66)) + new Vector2(-16, 0), OpenState.Closed);
            bd5.SetPlayerMode(PlayerObjectMode.One);
            Door bd6 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(154, 66)) + new Vector2(-16, 0), OpenState.Closed);
            bd6.SetPlayerMode(PlayerObjectMode.Two);

            AudioManager.PlayMusic("Mountain");

        }

        public override void ActivateEvent(int eventID, TriggerState triggerState)
        {
            base.ActivateEvent(eventID, triggerState);

            switch (eventID)
            {
                case 0:
                    if (triggerState == TriggerState.Triggered && !gsPlay.gameStateManager.screenTransition) 
                    {
                        bool isGood = true;
                        foreach (Portal portal in gsPlay.portalList)
                        {
                            if (portal.goodMode == false) isGood = false;
                        }
                        if (isGood) gsPlay.gameStateManager.goodness++;
                        else gsPlay.gameStateManager.goodness--;
                        gsPlay.player1.visible = false;
                        gsPlay.player2.visible = false;
                        GSPlay.AddCheckpointParticles(gsPlay.player1.position, true);
                        GSPlay.AddCheckpointParticles(gsPlay.player2.position, false);
                        gsPlay.gameStateManager.currentLevel = 50;
                        //gsPlay.gameStateManager.SwitchToGSPlay();
                        gsPlay.gameStateManager.TransitionToGameState(gsPlay, GameStateType.GSPlay, 30);
                    }
                    break;
            }
        }
    }
}
