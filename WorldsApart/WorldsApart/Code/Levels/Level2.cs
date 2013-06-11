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

            player1Pos = GridToPosition(26, 85);
            player2Pos = GridToPosition(22, 85);

            //player1Pos = GridToPosition(372, 50);
            //player2Pos = GridToPosition(372, 50);

            portalPos = GridToPosition(457, 67);
            pItemPos = GridToPosition(241, 79);

            SetupLevel();

            Sprite cameraTarget = new Sprite(GridToPosition(137, 86));
            gsPlay.cameraPlayer1.AddTarget(cameraTarget);
            gsPlay.cameraPlayer2.AddTarget(cameraTarget);

            cameraTarget = new Sprite(GridToPosition(201, 83));
            gsPlay.cameraPlayer1.AddTarget(cameraTarget);
            gsPlay.cameraPlayer2.AddTarget(cameraTarget);

            atmosphereLight = new Color(150, 150, 150);

            //puzzle 1
            Door d1 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/doorThick"), GridToPosition(new Point(101, 87)), OpenState.Closed);
            d1.SetPlayerMode(PlayerObjectMode.One);
            gsPlay.AddButton(new EventTrigger(this, d1), 1, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(105, 81)));
            gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), GridToPosition(new Point(137, 46))).selfIlluminating = true;

            //puzzle 2



            //puzzle 3
            MovingPlatform p11 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(201, 84)), Level.GridToPosition(new Point(201, 84)));
            p11.SetPlayerMode(PlayerObjectMode.One);
            MovingPlatform p12 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(201, 90)), Level.GridToPosition(new Point(201, 84)));
            gsPlay.AddSwitch(new EventTrigger(this, p12), gsPlay.LoadTexture("TestSprites/switch"), GridToPosition(new Point(212, 81)));

            //Moveable m1 = gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(211, 81)), .8f);
            //Moveable m2 = gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(201, 84)), .8f);
            //m2.SetPlayerMode(PlayerObjectMode.One);

            //puzzle 4
            Door d2 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/doorThick"), GridToPosition(new Point(241, 84)), OpenState.Closed);
            gsPlay.AddButton(new EventTrigger(this, d2), 2, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(237, 77)));
            gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), GridToPosition(new Point(226, 49))).selfIlluminating = true;
            gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), GridToPosition(new Point(238, 55))).selfIlluminating = true;

            //puzzle 5
            Moveable m3 = gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(261, 85)), .8f);
            m3.SetPlayerMode(PlayerObjectMode.Two);
            Moveable m4 = gsPlay.AddMoveable(gsPlay.LoadTexture("TestSprites/moveable"), Level.GridToPosition(new Point(282, 76)), .8f);

            Door d3 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/doorThick"), GridToPosition(new Point(315, 78)), OpenState.Closed);
            d3.SetPlayerMode(PlayerObjectMode.One);
            gsPlay.AddButton(new EventTrigger(this, d3), 1, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(39, 25)));

            Door d4 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/Platform"), GridToPosition(new Point(39, 23)), OpenState.Closed);
            gsPlay.AddButton(new EventTrigger(this, d4), 1, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(44, 25)));
            gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), GridToPosition(new Point(39, 20))).selfIlluminating = true;

            MovingPlatform p1 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(44, 24)), Level.GridToPosition(new Point(44, 50)));
            gsPlay.AddButton(new EventTrigger(this, p1), 1, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(308, 80)));
            MovingPlatform p2 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(44, 24)), Level.GridToPosition(new Point(44, 50)));
            gsPlay.AddButton(new EventTrigger(this, p2), 1, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(321, 80)));
            gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), GridToPosition(new Point(44, 20))).selfIlluminating = true;

            //secret puzzle
            MovingPlatform p4 = gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(259, 63)), Level.GridToPosition(new Point(278, 63)));

            //red platforms
            MovingPlatform p5 = gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(272, 50)), Level.GridToPosition(new Point(272, 59)));
            p5.SetPlayerMode(PlayerObjectMode.One);
            MovingPlatform p6 = gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(278, 48)), Level.GridToPosition(new Point(289, 48)));
            p6.SetPlayerMode(PlayerObjectMode.One);
            MovingPlatform p7 = gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(304, 38)), Level.GridToPosition(new Point(297, 45)));
            p7.SetPlayerMode(PlayerObjectMode.One);

            //blue platforms
            MovingPlatform p8 = gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(284, 50)), Level.GridToPosition(new Point(284, 59)));
            p8.SetPlayerMode(PlayerObjectMode.Two);
            MovingPlatform p9 = gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(278, 48)), Level.GridToPosition(new Point(267, 48)));
            p9.SetPlayerMode(PlayerObjectMode.Two);
            MovingPlatform p10 = gsPlay.AddPlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(252, 38)), Level.GridToPosition(new Point(259, 45)));
            p10.SetPlayerMode(PlayerObjectMode.Two);

            gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), GridToPosition(new Point(245, 32))).selfIlluminating = true;
            gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), GridToPosition(new Point(312, 32))).selfIlluminating = true;

            Door d5 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door2"), GridToPosition(new Point(242, 79)) + new Vector2(16,0), OpenState.Closed);
            gsPlay.AddButton(new EventTrigger(this, d5), 2, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(278, 39)));

            //6
            //Door d6 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(342, 52)) + new Vector2(-16, 0), OpenState.Closed);
            //gsPlay.AddButton(new EventTrigger(this, d6), 1, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(97, 10)));
            //Door d7 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(337, 34)) + new Vector2(-16, 0), OpenState.Closed);
           // d7.SetPlayerMode(PlayerObjectMode.One);
            //Button b2 = gsPlay.AddButton(new EventTrigger(this, d7), 1, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(339, 55)));
            //b2.SetPlayerMode(PlayerObjectMode.Two);

            //blockers
            //Door d9 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(337, 52)) + new Vector2(-16, 0), OpenState.Closed);
            //d9.SetPlayerMode(PlayerObjectMode.Two);
            //Door d8 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(342, 52)) + new Vector2(-16, 0), OpenState.Closed);
            //d8.SetPlayerMode(PlayerObjectMode.One);
            //or gate

            //MovingPlatform p13 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(96, 10)), Level.GridToPosition(new Point(96, 12)));
            //Button b1 = gsPlay.AddButton(new EventTrigger(this, p13), 1, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(339, 55)));
            //b1.SetPlayerMode(PlayerObjectMode.One);
            //MovingPlatform p14 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(99, 10)), Level.GridToPosition(new Point(99, 12)));
            //gsPlay.AddButton(new EventTrigger(this, p14), 1, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(339, 44)));
            gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), GridToPosition(new Point(97, 9))).selfIlluminating = true;
            gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), GridToPosition(new Point(98, 9))).selfIlluminating = true;

            //part2
            Door d10 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(362, 74)) + new Vector2(-16, 0), OpenState.Closed);
            d10.SetPlayerMode(PlayerObjectMode.One); 
            Door d11 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(362, 27)) + new Vector2(-16, 0), OpenState.Closed);
            d11.SetPlayerMode(PlayerObjectMode.Two);
            
            //64 40

            Door d14 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(367, 27)) + new Vector2(-16, 0), OpenState.Closed);
            d14.SetPlayerMode(PlayerObjectMode.Two);
            //NEEDS TO BE TIMED!!!
            FlipSwitch s1 = gsPlay.AddSwitch(new EventTrigger(this, d14), gsPlay.LoadTexture("TestSprites/switch"), GridToPosition(new Point(364, 40)));
            s1.SetPlayerMode(PlayerObjectMode.One);
            Door d15 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/door"), GridToPosition(new Point(367, 74)) + new Vector2(-16, 0), OpenState.Closed);
            d15.SetPlayerMode(PlayerObjectMode.One);
            //NEEDS TO BE TIMED!!!
            FlipSwitch s2 = gsPlay.AddSwitch(new EventTrigger(this, d15), gsPlay.LoadTexture("TestSprites/switch"), GridToPosition(new Point(364, 65)));
            s2.SetPlayerMode(PlayerObjectMode.Two);

            PickUpObj o1 = gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), GridToPosition(new Point(364, 28)));
            o1.SetPlayerMode(PlayerObjectMode.Two);
            PickUpObj o2 = gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), GridToPosition(new Point(364, 75)));
            o2.SetPlayerMode(PlayerObjectMode.One);

            MovingPlatform d12 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(389, 42)) + new Vector2(-16, 0), Level.GridToPosition(new Point(389, 40)) + new Vector2(-16, 0));
            MovingPlatform d23 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(390, 46)) + new Vector2(-16, 0), Level.GridToPosition(new Point(390, 48)) + new Vector2(-16, 0));
            //d12.SetPlayerMode(PlayerObjectMode.Two);
            Button b3 = gsPlay.AddButton(new EventTrigger(this, d12), .5f, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(391, 36)));
            b3.AddEvent(new EventTrigger(this, d23));
            b3.SetPlayerMode(PlayerObjectMode.One);
            MovingPlatform d13 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(389, 46)) + new Vector2(-16, 0), Level.GridToPosition(new Point(389, 48)) + new Vector2(-16, 0));
            MovingPlatform d22 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(390, 42)) + new Vector2(-16, 0), Level.GridToPosition(new Point(390, 40)) + new Vector2(-16, 0));
            //d13.SetPlayerMode(PlayerObjectMode.One);
            Button b4 = gsPlay.AddButton(new EventTrigger(this, d13), .5f, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(391, 60)));
            b4.AddEvent(new EventTrigger(this, d22));
            b4.SetPlayerMode(PlayerObjectMode.Two);

            //MovingPlatform d22 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(390, 42)) + new Vector2(-16, 0), Level.GridToPosition(new Point(390, 40)) + new Vector2(-16, 0));
            ////d12.SetPlayerMode(PlayerObjectMode.Two);
            //Button b5 = gsPlay.AddButton(new EventTrigger(this, d22), .5f, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(391, 60)));
            //b5.SetPlayerMode(PlayerObjectMode.Two);
            //MovingPlatform d23 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(390, 46)) + new Vector2(-16, 0), Level.GridToPosition(new Point(390, 48)) + new Vector2(-16, 0));
            ////d13.SetPlayerMode(PlayerObjectMode.One);
            //Button b6 = gsPlay.AddButton(new EventTrigger(this, d23), .5f, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(391, 36)));
            //b6.SetPlayerMode(PlayerObjectMode.One);

            //MovingPlatform p15 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(367, 49)) + new Vector2(-16, 0), Level.GridToPosition(new Point(367, 47)) + new Vector2(-16, 0));
            //FlipSwitch b5 = gsPlay.AddSwitch(new EventTrigger(this, p15), gsPlay.LoadTexture("TestSprites/switch"), GridToPosition(new Point(365, 45)));
            //b5.pressureCooker = true;
            //MovingPlatform p16 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/door"), Level.GridToPosition(new Point(367, 53)) + new Vector2(-16, 0), Level.GridToPosition(new Point(367, 55)) + new Vector2(-16, 0));
            //FlipSwitch b6 = gsPlay.AddSwitch(new EventTrigger(this, p16), gsPlay.LoadTexture("TestSprites/switch"), GridToPosition(new Point(365, 60)));
            //b6.pressureCooker = true;

            //gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), GridToPosition(new Point(294, 70))).selfIlluminating = true;

            ////and gate
            //MovingPlatform p17 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(370, 34)), Level.GridToPosition(new Point(370, 36)));
            //FlipSwitch b8 = gsPlay.AddSwitch(new EventTrigger(this, p17), gsPlay.LoadTexture("TestSprites/switch"), GridToPosition(new Point(365, 45)));
            //b8.pressureCooker = true;
            ////b8.SetPlayerMode(PlayerObjectMode.One);
            //MovingPlatform p18 = gsPlay.AddReversePlatform(gsPlay.LoadTexture("TestSprites/platform"), Level.GridToPosition(new Point(370, 34)), Level.GridToPosition(new Point(370, 36)));
            //FlipSwitch b9 = gsPlay.AddSwitch(new EventTrigger(this, p18), gsPlay.LoadTexture("TestSprites/switch"), GridToPosition(new Point(365, 60)));
            //b9.pressureCooker = true;
            ////b9.SetPlayerMode(PlayerObjectMode.Two);

            //gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), GridToPosition(new Point(370, 33))).selfIlluminating = true;

            //Door d17 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/platform"), GridToPosition(new Point(375, 34)), OpenState.Closed);
            //gsPlay.AddButton(new EventTrigger(this, d17), 1, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(370, 35)));
            //gsPlay.AddBouncyBall(.5f, gsPlay.LoadTexture("TestSprites/pickUp"), GridToPosition(new Point(375, 33))).selfIlluminating = true;


            //end door

            //Door d16 = gsPlay.AddFadingDoor(gsPlay.LoadTexture("TestSprites/doorThick"), GridToPosition(new Point(390, 44)), OpenState.Closed);
            //Button b7 = gsPlay.AddButton(new EventTrigger(this, d16), 2, gsPlay.LoadTexture("TestSprites/button"), GridToPosition(new Point(364, 53)));

            //364, 53
            //Button (365, 45)
            //Button (365, 60)


            SpriteIMG s = new SpriteIMG(gsPlay.LoadTexture("bgSky"), new Vector2(32, levelHeight / 4));
            gsPlay.AddParallax(s, .5f);

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
                        //gsPlay.gameStateManager.SwitchToGSPlay();
                        gsPlay.gameStateManager.TransitionToGameState(gsPlay, GameStateType.GSPlay, 30);
                    }
                    break;
            }
        }
    }
}
